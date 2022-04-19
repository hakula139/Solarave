using UnityEngine;

namespace Result {
  public class SpriteAssetHelper : MonoBehaviour {
    public static SpriteAssetHelper instance;

    public Sprite[] djLevelLargeSprites = new Sprite[8];
    public Sprite[] djLevelSprites = new Sprite[9];

    private void Awake() {
      instance = this;
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
