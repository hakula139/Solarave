namespace Play {
  public class BgmObject : NoteObject {
    private void Update() {
      float currentTime = FumenScroller.instance.currentTime;
      float d = currentTime - time;

      if (d >= -FumenManager.instance.goodRange) {
        ((BgmController)lane).PlayKeySound();
        gameObject.SetActive(false);
      }
    }
  }
}
