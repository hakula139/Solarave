using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
  private SpriteRenderer sr;

  public KeyCode keyAssigned;

  // Start is called before the first frame update.
  void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame.
  void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0.25f);
    }
    if (Input.GetKeyUp(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0);
    }
  }
}
