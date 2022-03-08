using System.Collections.Generic;

namespace BMS {
  public enum Channel {
    Bgm = 1,
    LengthOfMeasure = 2,
    BgaBase = 4,
    BgaPoor = 6,
    BgaLayer = 7,
    Scratch = 16,
    Key1 = 11,
    Key2 = 12,
    Key3 = 13,
    Key4 = 14,
    Key5 = 15,
    Key6 = 18,
    Key7 = 19,
  };

  public class Bga {
    public Channel channelId;
    public string bgaPath;
    public float position;
  }

  public class Note {
    public Channel channelId;
    public string wavPath;
    public float position;
  }

  public class Measure {
    public float length = 1f;
    public List<Bga> bgas = new();
    public List<Note> notes = new();
  }

  public class ChannelSection {
    public List<Measure> measures = new();
  }
}
