using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  public AudioSource bgm;
  public bool hasStarted;
  public BeatScroller bs;

  // Start is called before the first frame update.
  void Start() {
    instance = this;
  }

  // Update is called once per frame.
  void Update() {
    if (!hasStarted) {
      if (Input.anyKeyDown) {
        hasStarted = true;
        bs.isEnabled = true;
        bgm.Play();
      }
    }
  }

  public void NoteHit() {
    Debug.Log("GREAT");
  }

  public void NoteMissed() {
    Debug.Log("POOR");
  }
}
