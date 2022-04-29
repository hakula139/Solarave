using UnityEngine;

namespace Result {
  public class AudioAssetHelper : MonoBehaviour {
    public static AudioAssetHelper instance;

    public AudioClip[] bgmClips;

    private void Awake() {
      instance = this;
    }

    public AudioClip GetBgmClip(bool isCleared) {
      return isCleared ? bgmClips[1] : bgmClips[0];
    }
  }
}
