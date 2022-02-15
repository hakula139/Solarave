using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour {
  public bool isClickable;
  public KeyCode keyAssigned;

  // Start is called before the first frame update.
  void Start() {
  }

  // Update is called once per frame.
  void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      if (isClickable) {
        gameObject.SetActive(false);
        GameManager.instance.NoteHit();
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Activator") {
      isClickable = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.tag == "Activator") {
      isClickable = false;
      GameManager.instance.NoteMissed();
    }
  }
}
