using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  public AudioSource bgm;
  public bool hasStarted;
  public BeatScroller bs;

  public int exScore;
  public TMP_Text exScoreText;
  public int combo;
  public TMP_Text comboText;
  public int maxCombo;
  public TMP_Text maxComboText;

  void Start() {
    instance = this;
  }

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

    exScore += 1;
    exScoreText.text = $"{exScore:0000}";
    combo += 1;
    comboText.text = combo.ToString();
    maxCombo = Math.Max(combo, maxCombo);
    maxComboText.text = $"{maxCombo:0000}";
  }

  public void NoteMissed() {
    Debug.Log("POOR");

    combo = 0;
    comboText.text = "";
  }
}
