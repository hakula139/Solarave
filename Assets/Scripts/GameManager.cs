using System;
using UnityEngine;
using TMPro;

public enum NoteExScores {
  PGREAT = 2,
  GREAT = 1,
  GOOD = 0,
  BAD = 0,
  POOR = 0,
}

public class GameManager : MonoBehaviour {
  public static GameManager instance;
  public BeatScroller bs;

  public AudioSource bgm;
  public bool hasStarted;

  public TMP_Text exScoreText;
  public TMP_Text judgeText;
  public TMP_Text maxComboText;

  private int exScore;
  private int combo;
  private int maxCombo;

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

  public void NoteJudge(NoteExScores score, bool isComboBreak = false) {
    exScore += (int)score;
    combo = isComboBreak ? 0 : combo + 1;
    maxCombo = Math.Max(combo, maxCombo);

    exScoreText.text = $"{exScore:0000}";
    maxComboText.text = $"{maxCombo:0000}";
  }

  public void PgreatJudge() {
    NoteJudge(NoteExScores.PGREAT);

    judgeText.text = $"GREAT  {combo}";
  }

  public void GreatJudge() {
    NoteJudge(NoteExScores.GREAT);

    judgeText.text = $"GREAT  {combo}";
  }

  public void GoodJudge() {
    NoteJudge(NoteExScores.GOOD);

    judgeText.text = $"GOOD  {combo}";
  }

  public void BadJudge() {
    NoteJudge(NoteExScores.BAD, true);

    judgeText.text = "BAD";
  }

  public void PoorJudge() {
    NoteJudge(NoteExScores.POOR, true);

    judgeText.text = "POOR";
  }
}
