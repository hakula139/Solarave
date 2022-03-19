public class BgmController : KeyController {
  public new void Start() {
  }

  public new void Update() {
  }

  protected new void SetupKeySound(int wavId, float time) {
    keySounds.Enqueue(new() {
      wavId = wavId,
      time = time,
    });
  }
}
