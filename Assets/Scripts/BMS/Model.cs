using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BMS {
  public class Model {
    public HeaderSection header = new();
    public ChannelSection content = new();

    public static Model Parse(string path) {
      string realPath = Path.Join(Application.streamingAssetsPath, path);
      if (!File.Exists(realPath)) {
        Debug.LogErrorFormat("file not found, path=<{0}>", path);
        return null;
      }

      Model model = new();
      try {
        using (StreamReader sr = new(realPath)) {
          while (!sr.EndOfStream) {
            model.ReadLine(sr.ReadLine());
          }
        }
        return model;
      } catch (Exception e) {
        Debug.LogErrorFormat("failed to parse bms file, path=<{0}> exception=<{1}>", path, e.ToString());
        return null;
      }
    }

    protected void ReadLine(string line) {
      // Debug.LogFormat("parsing: line=<{0}>", line);
      if (line.Length < 2 || !line.StartsWith('#')) {
        return;
      }

      if (!IntegerHelper.IsInteger(line.Substring(1, 3))) {
        // Header section logic.
        string[] lineSplit = line.Split(' ', 2);
        string keyword = lineSplit[0][1..].ToLower();
        string value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ReadHeaderLine(keyword, value);
      } else {
        // Channel section logic.
        string[] lineSplit = line.Split(':', 2);
        string channel = lineSplit[0][1..].ToLower();
        string value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ReadChannelLine(channel, value);
      }
    }

    private void ReadHeaderLine(string keyword, string value) {
      if (string.IsNullOrEmpty(value)) {
        return;
      }

      switch (keyword) {
        case "player":
          header.player = int.Parse(value);
          break;
        case "genre":
          header.genre = value;
          break;
        case "title":
          header.title = value;
          break;
        case "artist":
          header.artist = value;
          break;
        case "subartist":
          header.subartist = value;
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
        default:
          if (keyword.StartsWith("wav")) {
            int wavId = IntegerHelper.ParseBase36(keyword[3..]);
            if (!IntegerHelper.InBounds(wavId, header.wavPaths)) {
              Debug.LogWarningFormat("wav index overflow, keyword=<{0}>", keyword);
              break;
            }
            header.wavPaths[wavId] = value;
          } else if (keyword.StartsWith("bmp")) {
            int bgaId = IntegerHelper.ParseBase36(keyword[3..]);
            if (!IntegerHelper.InBounds(bgaId, header.bgaPaths)) {
              Debug.LogWarningFormat("bmp index overflow, keyword=<{0}>", keyword);
              break;
            }
            header.bgaPaths[bgaId] = value;
          } else {
            Debug.LogWarningFormat("failed to parse header line, keyword=<{0}>", keyword);
          }
          break;
      }
    }

    private void ReadChannelLine(string channel, string value) {
      if (string.IsNullOrEmpty(value)) {
        return;
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
          ReadValue(measureId, channelId, value);
          break;
      }
    }

    private bool ValidateValue(int measureId, Channel channelId, string value) {
      if (value.Length % 2 != 0) {
        Debug.LogWarningFormat("invalid syntax: incorrect length, measureId=<{0}> channelId=<{1}> value=<{2}>", measureId, channelId, value);
        return false;
      }
      if (value.All(c => c == '0')) {
        // Empty measure.
        return false;
      }
      return true;
    }

    private void ReadValue(int measureId, Channel channelId, string value) {
      if (!ValidateValue(measureId, channelId, value)) {
        return;
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
              bgaPath = header.bgaPaths[id],
              position = (float)i / meter,
            });
            break;
          default:
            content.measures[measureId].notes.Add(new Note {
              channelId = channelId,
              wavPath = header.wavPaths[id],
              position = (float)i / meter,
            });
            break;
        }
      }
    }
  }
}