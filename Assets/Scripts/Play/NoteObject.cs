using UnityEngine;

public class NoteObject : MonoBehaviour {
  public bool isClickable = false;
  public float time;
  public KeyCode keyAssigned;

  public void Update() {
    if (isClickable && Input.GetKeyDown(keyAssigned)) {
      isClickable = false;

      var d = Mathf.Abs(transform.position.y - time);
      if (d < 0.1f) {
        GameManager.instance.PgreatJudge();
      } else if (d < 0.2f) {
        GameManager.instance.GreatJudge();
      } else if (d < 0.4f) {
        GameManager.instance.GoodJudge();
      } else {
        GameManager.instance.BadJudge();
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
