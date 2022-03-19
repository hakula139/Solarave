public class BgmController : KeyController {
  public new void Start() {
  }

  public new void Update() {
  }

  public override void SetupNote(float start, float length, BMS.Note note) {
    float y = start + (length * note.position);
    float time = y * 240000f / scroller.bpm;

    SetupKeySound(note.wavId, time);
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
        audioLoader.Play(keySound.wavId, keySound.time);
      }
    }
  }
}
