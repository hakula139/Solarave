using System.Linq;

public class SpriteAssetHelper {
  protected static string ToSprite(string name) {
    return $"<sprite name=\"{name}\">";
  }

  protected static string ToSpriteAnimation(int start, int stop, float frameRate = 60f) {
    return $"<sprite anim=\"{start},{stop},{frameRate}\">";
  }

  public static string GetJudge(Judge judge) {
    switch (judge) {
      case Judge.PGREAT: return ToSpriteAnimation(40, 42);
      case Judge.GREAT: return ToSprite("great");
      case Judge.GOOD: return ToSprite("good");
      case Judge.BAD: return ToSprite("bad");
      default: return ToSprite("poor");
    }
  }

  public static string GetInteger(Judge judge, int integer) {
    if (judge != Judge.PGREAT) {
      return integer.ToString().Aggregate("", (acc, digit) => acc + ToSprite(digit.ToString()));
    } else {
      return integer.ToString().Aggregate("", (acc, digit) => {
        var start = 10 + 3 * (digit - '0');
        return acc + ToSpriteAnimation(start, start + 2);
      });
    }
  }
}
