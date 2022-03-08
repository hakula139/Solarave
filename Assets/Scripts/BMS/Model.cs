using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BMS {
  public class Model {
    public HeaderSection header = new();
    public ChannelSection content = new();

    public static Model Parse(string path) {
      var realPath = Path.Join(Application.streamingAssetsPath, path);
      if (!File.Exists(realPath)) {
        Debug.LogErrorFormat("file not found, path=<{0}>", path);
        return null;
      }

      var model = new Model();
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
      Debug.LogFormat("parsing: line=<{0}>", line);
      if (line.Length < 2 || !line.StartsWith('#')) return;

      if (!IntegerHelper.IsInteger(line.Substring(1, 3))) {
        // Header section logic.
        var lineSplit = line.Split(' ', 2);
        var keyword = lineSplit[0][1..].ToLower();
        var value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ReadHeaderLine(keyword, value);
      } else {
        // Channel section logic.
        var lineSplit = line.Split(':', 2);
        var channel = lineSplit[0][1..].ToLower();
        var value = lineSplit.Length > 1 ? lineSplit[1] : "";
        ReadChannelLine(channel, value);
      }
    }

    private void ReadHeaderLine(string keyword, string value) {
      if (string.IsNullOrEmpty(value)) return;

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
            var wavId = IntegerHelper.ParseBase36(keyword[3..]);
            if (!IntegerHelper.InBounds(wavId, header.wavPaths)) {
              Debug.LogWarningFormat("wav index overflow, keyword=<{0}>", keyword);
              break;
            }
            header.wavPaths[wavId] = value;
          } else if (keyword.StartsWith("bmp")) {
            var bgaId = IntegerHelper.ParseBase36(keyword[3..]);
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
      if (string.IsNullOrEmpty(value)) return;

      var measureId = int.Parse(channel.Substring(0, 3));
      var channelId = (Channel)IntegerHelper.ParseBase36(channel[3..]);
      while (content.data.Count < measureId + 1) {
        content.data.Add(new Measure());
      }

      switch (channelId) {
        case Channel.LengthOfMeasure:
          content.data[measureId].length = float.Parse(value);
          return;
      }

      if (value.Length % 2 != 0) {
        Debug.LogWarningFormat("invalid syntax: incorrect length, channel=<{0}>, value=<{1}>", channel, value);
        return;
      }
      if (value.All(c => c == '0')) {
        // Empty measure.
        return;
      }

      var meter = value.Length / 2;
      for (int i = 0; i < meter; i++) {
        var wavId = IntegerHelper.ParseBase36(value.Substring(i * 2, 2));
        if (wavId == 0) continue;

        var note = new Note();
        note.channelId = channelId;
        note.wavPath = header.wavPaths[wavId];
        note.position = (float)i / meter;
      }
    }
  }
}
