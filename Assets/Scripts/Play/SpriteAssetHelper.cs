using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Play {
  public class SpriteAssetHelper : MonoBehaviour {
    public static SpriteAssetHelper instance;

    public Sprite[] fastSlowIndicatorSprites = new Sprite[2];
    public Sprite[] difficultySprites = new Sprite[6];

    private void Awake() {
      instance = this;
    }

    public string ToSprite(int id) {
      return $"<sprite=\"{id}\">";
    }

    public string ToSpriteByName(string name) {
      return $"<sprite name=\"{name}\">";
    }

    public string ToSpriteByName(char name) {
      return $"<sprite name=\"{name}\">";
    }

    public string ToSpriteAnimation(int start, int stop, float frameRate = 60f) {
      return $"<sprite anim=\"{start},{stop},{frameRate}\">";
    }

    private List<int> SplitInteger(int integer, int digitBase = 10) {
      List<int> split = new();
      if (integer < 0 || digitBase < 1) {
        return split;
      }

      do {
        split.Add(integer % digitBase);
        integer /= digitBase;
      } while (integer > 0);

      split.Reverse();
      return split;
    }

    public string GetInteger(int integer) {
      StringBuilder text = new();
      foreach (int digit in SplitInteger(integer)) {
        _ = text.Append(ToSprite(digit));
      }
      return text.ToString();
    }

    public string GetJudge(Judge judge) {
      return judge switch {
        Judge.PGREAT => ToSpriteAnimation(40, 42),
        Judge.GREAT => ToSpriteByName("great"),
        Judge.GOOD => ToSpriteByName("good"),
        Judge.BAD => ToSpriteByName("bad"),
        Judge.POOR => ToSpriteByName("poor"),
        _ => "",  // should not reach here.
      };
    }

    public string GetComboInJudge(Judge judge, int combo) {
      StringBuilder text = new();
      if (judge == Judge.PGREAT) {
        foreach (int digit in SplitInteger(combo)) {
          int i = 10 + (3 * digit);
          _ = text.Append(ToSpriteAnimation(i, i + 2));
        }
      } else {
        foreach (int digit in SplitInteger(combo)) {
          _ = text.Append(ToSpriteByName(digit.ToString()));
        }
      }
      return text.ToString();
    }

    public Sprite GetFastSlowIndicatorSprite(bool isEarly) {
      return isEarly ? fastSlowIndicatorSprites[0] : fastSlowIndicatorSprites[1];
    }

    public Sprite GetDifficultySprite(BMS.Difficulty difficulty) {
      return difficulty switch {
        BMS.Difficulty.Unknown => difficultySprites[0],
        BMS.Difficulty.Beginner => difficultySprites[1],
        BMS.Difficulty.Normal => difficultySprites[2],
        BMS.Difficulty.Hyper => difficultySprites[3],
        BMS.Difficulty.Another => difficultySprites[4],
        BMS.Difficulty.Insane => difficultySprites[5],
        _ => difficultySprites[0],
      };
    }

    public Color GetDifficultyColor(BMS.Difficulty difficulty) {
      return difficulty switch {
        BMS.Difficulty.Unknown => new Color(1, 1, 1, 1),
        BMS.Difficulty.Beginner => new Color(0.0627f, 0.7608f, 0.3373f, 1),
        BMS.Difficulty.Normal => new Color(0.2941f, 0.4471f, 1, 1),
        BMS.Difficulty.Hyper => new Color(1, 0.7608f, 0.1647f, 1),
        BMS.Difficulty.Another => new Color(0.9569f, 0.1412f, 0.2510f, 1),
        BMS.Difficulty.Insane => new Color(0.9216f, 0.2941f, 0.9216f, 1),
        _ => new Color(1, 1, 1, 1),
      };
    }
  }
}
