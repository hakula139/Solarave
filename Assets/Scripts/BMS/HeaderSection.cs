namespace BMS {
  public enum Difficulty {
    Unknown,
    Beginner,
    Normal,
    Hyper,
    Another,
    Insane,
  }

  public enum JudgeRank {
    VeryHard,
    Hard,
    Normal,
    Easy,
  }

  public class HeaderSection {
    public int player = 1;
    public string genre;
    public string title;
    public string subtitle;
    public string artist;
    public string subartist;
    public float bpm = 130;
    public Difficulty difficulty = Difficulty.Unknown;
    public int level = 0;
    public JudgeRank rank = JudgeRank.Normal;
    public float total = 160;
    public string banner;
    public string stageFile;
    public int? lnObj = null;

    public string[] wavPaths = new string[36 * 36];
    public string[] bgaPaths = new string[36 * 36];
  }
}
