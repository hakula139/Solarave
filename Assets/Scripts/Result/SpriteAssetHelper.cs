using UnityEngine;

namespace Result {
  public class SpriteAssetHelper : MonoBehaviour {
    public static SpriteAssetHelper instance;

    public Sprite[] backgroundSprites;
    public Sprite[] titleSprites;

    public Sprite[] djLevelLargeSprites;
    public Sprite[] gaugeModeSprites;

    public Sprite[] clearTypeSprites;
    public Sprite[] djLevelSprites;

    private void Awake() {
      instance = this;
    }

    public Sprite GetBackgroundSprite(bool isCleared) {
      return isCleared ? backgroundSprites[1] : backgroundSprites[0];
    }

    public Sprite GetTitleSprite(bool isCleared) {
      return isCleared ? titleSprites[1] : titleSprites[0];
    }

    public Sprite GetDjLevelLargeSprite(Play.DjLevel djLevel) {
      return djLevel switch {
        Play.DjLevel.F => djLevelLargeSprites[0],
        Play.DjLevel.E => djLevelLargeSprites[1],
        Play.DjLevel.D => djLevelLargeSprites[2],
        Play.DjLevel.C => djLevelLargeSprites[3],
        Play.DjLevel.B => djLevelLargeSprites[4],
        Play.DjLevel.A => djLevelLargeSprites[5],
        Play.DjLevel.AA => djLevelLargeSprites[6],
        Play.DjLevel.AAA => djLevelLargeSprites[7],
        Play.DjLevel.MAX => djLevelLargeSprites[7],  // use AAA sprite
        _ => djLevelLargeSprites[0],
      };
    }

    public Sprite GetGaugeModeSprite(Select.GaugeMode gaugeMode) {
      return gaugeMode switch {
        Select.GaugeMode.EASY => gaugeModeSprites[0],
        Select.GaugeMode.NORMAL => gaugeModeSprites[1],
        Select.GaugeMode.HARD => gaugeModeSprites[2],
        _ => null,  // not implemented yet
      };
    }

    public Sprite GetClearTypeSprite(Select.GaugeMode gaugeMode, bool isCleared) {
      return !isCleared
          ? clearTypeSprites[0]
          : gaugeMode switch {
            Select.GaugeMode.EASY => clearTypeSprites[1],
            Select.GaugeMode.NORMAL => clearTypeSprites[2],
            Select.GaugeMode.HARD => clearTypeSprites[3],
            _ => null,  // not implemented yet
          };
    }

    public Sprite GetDjLevelSprite(Play.DjLevel djLevel) {
      return djLevel switch {
        Play.DjLevel.F => djLevelSprites[0],
        Play.DjLevel.E => djLevelSprites[1],
        Play.DjLevel.D => djLevelSprites[2],
        Play.DjLevel.C => djLevelSprites[3],
        Play.DjLevel.B => djLevelSprites[4],
        Play.DjLevel.A => djLevelSprites[5],
        Play.DjLevel.AA => djLevelSprites[6],
        Play.DjLevel.AAA => djLevelSprites[7],
        Play.DjLevel.MAX => djLevelSprites[8],
        _ => djLevelSprites[0],
      };
    }
  }
}
