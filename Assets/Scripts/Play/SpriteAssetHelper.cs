using System.Linq;

namespace Play {
  public class SpriteAssetHelper {
    protected static string ToSprite(string name) {
      return $"<sprite name=\"{name}\">";
    }

    protected static string ToSpriteAnimation(int start, int stop, float frameRate = 60f) {
      return $"<sprite anim=\"{start},{stop},{frameRate}\">";
    }

    public static string GetJudge(Judge judge) {
      return judge switch {
        Judge.PGREAT => ToSpriteAnimation(40, 42),
        Judge.GREAT => ToSprite("great"),
        Judge.GOOD => ToSprite("good"),
        Judge.BAD => ToSprite("bad"),
        Judge.POOR => ToSprite("poor"),
        _ => "",  // should not reach here.
      };
    }

    public static string GetInteger(Judge judge, int integer) {
      return judge != Judge.PGREAT
          ? integer.ToString().Aggregate("", (acc, digit) => acc + ToSprite(digit.ToString()))
          : integer.ToString().Aggregate("", (acc, digit) => {
            int start = 10 + (3 * (digit - '0'));
            return acc + ToSpriteAnimation(start, start + 2);
          });
    }
  }
}
