namespace Play {
  public class BgmController : KeyController {
    public new void Update() {
    }

    public override void SetupKeySound(int wavId, float time) {
      keySounds.Enqueue(new() {
        wavId = wavId,
        time = time,
      });
    }

    public override void PlayKeySound() {
      // Find the latest key sound to play.
      if (keySounds.TryDequeue(out KeySound keySound)) {
        if (keySound is not null) {
          AudioLoader.instance.Play(keySound.wavId, keySound.time);
        }
      }
    }
  }
}
