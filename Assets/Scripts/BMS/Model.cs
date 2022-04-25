using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BMS {
  public class Model {
    public HeaderSection header = new();
    public ChannelSection content = new();

    public bool Parse(string path, bool headerOnly = false) {
      string realPath = Path.Combine(Application.streamingAssetsPath, path);
      if (!File.Exists(realPath)) {
        Debug.LogErrorFormat("bms file not found, path=<{0}>", realPath);
        return false;
      }

      bool ret = true;
      using (StreamReader sr = new(realPath, encoding: System.Text.Encoding.GetEncoding(932))) {  // encoding: Shift-JIS
        // Debug.LogFormat("parsing bms file, path=<{0}> encoding=<{1}>", realPath, sr.CurrentEncoding);
        while (!sr.EndOfStream) {
          ret = ReadLine(sr.ReadLine(), headerOnly) && ret;
        }
      }

      if (!headerOnly) {
        foreach (Measure measure in content.measures) {
          measure.bgas.Sort((x, y) => x.position.CompareTo(y.position));
          measure.notes.Sort((x, y) => x.position.CompareTo(y.position));
        }
      }

      if (!ret) {
        Debug.LogWarningFormat("failed to parse bms file, path=<{0}>", realPath);
      }
      return ret;
    }

    protected bool ReadLine(string line, bool headerOnly = false) {
      // Debug.LogFormat("parsing: line=<{0}>", line);
      if (line.Length < 2 || line[0] != '#') {
        return true;
      }

      bool ret = true;
      if (!IntegerHelper.IsInteger(line.Substring(1, 3))) {
        // Header section logic.
        string[] lineSplit = line.Split(' ', 2);
        string keyword = lineSplit[0][1..].ToLower();
        string value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ret = ReadHeaderLine(keyword, value);
      } else if (!headerOnly) {
        // Channel section logic.
        string[] lineSplit = line.Split(':', 2);
        string channel = lineSplit[0][1..].ToLower();
        string value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ret = ReadChannelLine(channel, value);
      } else {
        // Skip channel section.
      }
      return ret;
    }

    private bool ReadHeaderLine(string keyword, string value) {
      if (string.IsNullOrEmpty(value)) {
        return true;
      }

      switch (keyword) {
        case "player":
          header.player = int.Parse(value);
          break;
        case "genre":
          header.genre = value;
          break;
        case "comment":
          if (value.Length >= 2 && value[0] == '\"' && value[^1] == '\"') {
            value = value[1..^1];
          }
          header.comment = value;
          break;
        case "title":
          (header.title, header.subtitle) = ReadTitle(value);
          break;
        case "subtitle":
          if (string.IsNullOrEmpty(header.subtitle)) {
            header.subtitle = value;
          } else {
            header.subtitle += $" {value}";
          }
          break;
        case "artist":
          header.artist = value;
          break;
        case "subartist":
          if (string.IsNullOrEmpty(header.subartist)) {
            header.subartist = value;
          } else {
            header.subartist += $" / {value}";
          }
          break;
        case "bpm":
          header.bpm = float.Parse(value);
          break;
        case "difficulty":
          header.difficulty = (Difficulty)int.Parse(value);
          break;
        case "playlevel":
          header.level = int.Parse(value);
          break;
        case "rank":
          header.rank = (JudgeRank)int.Parse(value);
          break;
        case "total":
          header.total = float.Parse(value);
          break;
        case "banner":
          header.banner = value;
          break;
        case "stagefile":
          header.stageFile = value;
          break;
        case "lnobj":
          header.lnObj = IntegerHelper.ParseBase36(value);
          break;
        case "volwav":
          header.volume = int.Parse(value) / 100f;
          break;
        default:
          if (keyword.StartsWith("wav")) {
            int wavId = IntegerHelper.ParseBase36(keyword[3..]);
            if (!IntegerHelper.InBounds(wavId, header.wavPaths)) {
              Debug.LogWarningFormat("wav index overflow, keyword=<{0}>", keyword);
              return false;
            }
            header.wavPaths[wavId] = value;
          } else if (keyword.StartsWith("bmp")) {
            int bgaId = IntegerHelper.ParseBase36(keyword[3..]);
            if (!IntegerHelper.InBounds(bgaId, header.bgaPaths)) {
              Debug.LogWarningFormat("bmp index overflow, keyword=<{0}>", keyword);
              return false;
            }
            header.bgaPaths[bgaId] = value;
          } else {
            Debug.LogWarningFormat("failed to parse header line, keyword=<{0}>", keyword);
            return false;
          }
          break;
      }
      return true;
    }

    private (string, string) ReadTitle(string value) {
      Regex pattern = new(@"\[.*\]|\(.*\)|-.*-|"".*""|～.*～|<.*>|  .*");
      Match match = pattern.Match(value);
      string subtitle = match.Success ? value[match.Index..].Trim() : null;
      string title = match.Success ? value.Substring(0, match.Index).Trim() : value;
      return (title, subtitle);
    }

    private bool ReadChannelLine(string channel, string value) {
      if (string.IsNullOrEmpty(value)) {
        return true;
      }

      int measureId = int.Parse(channel.Substring(0, 3));
      Channel channelId = (Channel)int.Parse(channel[3..]);
      while (content.measures.Count < measureId + 1) {
        content.measures.Add(new Measure());
      }

      switch (channelId) {
        case Channel.LengthOfMeasure:
          content.measures[measureId].length = float.Parse(value);
          break;
        default:
          if (!ReadValue(measureId, channelId, value)) {
            Debug.LogWarningFormat("failed to parse channel line, measureId=<{0}> channelId=<{1}>", measureId, channelId);
            return false;
          }
          break;
      }
      return true;
    }

    private bool ReadValue(int measureId, Channel channelId, string value) {
      if (value.Length % 2 != 0) {
        Debug.LogWarningFormat("invalid syntax: incorrect length, measureId=<{0}> channelId=<{1}> value=<{2}>", measureId, channelId, value);
        return false;
      }
      if (value.All(c => c == '0')) {
        // Empty measure.
        return true;
      }

      int meter = value.Length / 2;
      for (int i = 0; i < meter; i++) {
        int id = IntegerHelper.ParseBase36(value.Substring(i * 2, 2));
        if (id == 0) {
          continue;
        }

        switch (channelId) {
          case Channel.BgaBase:
          case Channel.BgaPoor:
          case Channel.BgaLayer:
            content.measures[measureId].bgas.Add(new Bga {
              channelId = channelId,
              bgaId = id,
              position = (float)i / meter,
            });
            break;
          default:
            content.measures[measureId].notes.Add(new Note {
              channelId = channelId,
              wavId = id,
              position = (float)i / meter,
            });
            break;
        }
      }
      return true;
    }
  }
}
