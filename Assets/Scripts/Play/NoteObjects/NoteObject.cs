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
    float d = lane.scroller.currentTime - time;
    float error = Mathf.Abs(d);
    bool isLate = d > 0;

    if (isClickable) {
      if (Input.GetKeyDown(lane.keyAssigned)) {
        if (error <= FumenManager.instance.pgreatRange) {
          Debug.LogFormat("pgreat: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
          GameManager.instance.PgreatJudge();
        } else if (error <= FumenManager.instance.greatRange) {
          Debug.LogFormat("great: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
          GameManager.instance.GreatJudge();
        } else if (error <= FumenManager.instance.goodRange) {
          Debug.LogFormat("good: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
          GameManager.instance.GoodJudge();
        } else if (error <= FumenManager.instance.badRange) {
          Debug.LogFormat("bad: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
          GameManager.instance.BadJudge();
        } else if (error <= FumenManager.instance.poorRange && !isLate) {
          Debug.LogFormat("poor: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
          GameManager.instance.PoorJudge();
        }
        if (error <= FumenManager.instance.badRange) {
          isClickable = false;
        }
      }
    }

    if (d > FumenManager.instance.badRange) {
      if (isClickable) {
        Debug.LogFormat("miss: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, lane.scroller.currentTime, time);
        GameManager.instance.MissJudge();
        isClickable = false;
      }
      gameObject.SetActive(false);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Activator")) {
      sr.enabled = true;
      isClickable = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Activator")) {
      sr.enabled = false;
    }
  }
}
