using UnityEngine;

public class ButtonController : MonoBehaviour {
  private SpriteRenderer sr;

  public KeyCode keyAssigned;

  void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0.25f);
    }
    if (Input.GetKeyUp(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0);
    }
  }
}
