using UnityEngine;

namespace Select {
  public class SoundEffectsManager : MonoBehaviour {
    public static SoundEffectsManager instance;

    public AudioSource openSoundEffect;
    public AudioSource closeSoundEffect;
    public AudioSource selectSoundEffect;
    public AudioSource enterSoundEffect;

    private void Awake() {
      DontDestroyOnLoad(this);
      if (instance == null) {
        instance = this;
      } else {
        Destroy(gameObject);
      }
    }
  }
}
