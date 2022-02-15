using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  public AudioSource bgm;
  public bool hasStarted;
  public BeatScroller bs;

  public int exScore;
  public TMP_Text exScoreText;

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
    exScore += 1;
    exScoreText.text = $"{exScore:0000}";
    Debug.Log("GREAT");
  }

  public void NoteMissed() {
    Debug.Log("POOR");
  }
}
