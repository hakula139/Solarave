using UnityEngine;

namespace Select {
  public class SpriteAssetHelper : MonoBehaviour {
    public static SpriteAssetHelper instance;

    private void Awake() {
      instance = this;
    }

    public Color GetDifficultyColor(BMS.Difficulty difficulty) {
      return difficulty switch {
        BMS.Difficulty.Unknown => new Color(0.5f, 0.5f, 0.5f, 1),
        BMS.Difficulty.Beginner => new Color(0.5f, 1, 0.5f, 1),
        BMS.Difficulty.Normal => new Color(0.25f, 0.5f, 1, 1),
        BMS.Difficulty.Hyper => new Color(1, 0.5f, 0.5f, 1),
        BMS.Difficulty.Another => new Color(1, 0.125f, 0.125f, 1),
        BMS.Difficulty.Insane => new Color(0.375f, 0.0625f, 0.75f, 1),
        _ => new Color(0.5f, 0.5f, 0.5f, 1),
      };
    }
  }
}
