public class BgmObject : NoteObject {
  public new void Update() {
    float currentTime = FumenScroller.instance.currentTime;
    float d = currentTime - time;

    if (d >= -FumenManager.instance.poorRange) {
      ((BgmController)lane).PlayKeySound();
      gameObject.SetActive(false);
    }
  }
}
