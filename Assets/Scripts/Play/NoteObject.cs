using UnityEngine;

public class NoteObject : MonoBehaviour {
  protected SpriteRenderer sr;

  public bool isClickable = false;
  public float time;
  public KeyController lane;

  public void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  public void Update() {
    float currentTime = FumenScroller.instance.currentTime;
    float d = currentTime - time;

    if (d > FumenManager.instance.badRange) {
      if (isClickable) {
        Debug.LogFormat("miss: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, time);
        GameManager.instance.MissJudge();
        isClickable = false;
        _ = lane.notes.Dequeue();
      }
      gameObject.SetActive(false);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Activator")) {
      sr.enabled = true;
      isClickable = true;
    } else if (other.CompareTag("BgmTrigger")) {
      ((BgmController)lane).PlayKeySound();
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Activator")) {
      sr.enabled = false;
    }
  }
}
