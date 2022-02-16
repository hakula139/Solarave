using UnityEngine;

public class NoteObject : MonoBehaviour {
  public bool isClickable;
  public KeyCode keyAssigned;

  void Update() {
    if (isClickable) {
      if (Input.GetKeyDown(keyAssigned)) {
        isClickable = false;

        if (Mathf.Abs(transform.position.y) < 0.1f) {
          GameManager.instance.PgreatJudge();
        } else if (Mathf.Abs(transform.position.y) < 0.2f) {
          GameManager.instance.GreatJudge();
        } else if (Mathf.Abs(transform.position.y) < 0.4f) {
          GameManager.instance.GoodJudge();
        } else {
          GameManager.instance.BadJudge();
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Activator")) {
      isClickable = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (isClickable && other.CompareTag("Activator")) {
      isClickable = false;
      GameManager.instance.PoorJudge();
    }
    gameObject.SetActive(false);
  }
}
