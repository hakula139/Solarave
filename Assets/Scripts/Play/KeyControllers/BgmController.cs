namespace Play {
  public class BgmController : KeyController {
    private void Start() {
    }

    public override void PlayKeySound() {
      // Find the latest key sound to play.
      if (keySounds.TryDequeue(out KeySound keySound)) {
        if (keySound != null) {
          AudioLoader.instance.Play(keySound.wavId, keySound.time);
        }
      }
    }
  }
}
