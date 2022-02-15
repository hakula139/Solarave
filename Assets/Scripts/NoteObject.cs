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
    if (isClickable) {
      if (Input.GetKeyDown(keyAssigned)) {
        isClickable = false;
        GameManager.instance.NoteHit();
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
      GameManager.instance.NoteMissed();
    }
    gameObject.SetActive(false);
  }
}
