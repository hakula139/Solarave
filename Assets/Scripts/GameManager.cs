using System;
using UnityEngine;
using TMPro;

public enum NoteExScore {
  PGREAT = 2,
  GREAT = 1,
  GOOD = 0,
  BAD = 0,
  POOR = 0,
}

public class GameManager : MonoBehaviour {
  public static GameManager instance;
  public BeatScroller bs;
  public Animator judgeAnimator;

  public AudioSource bgm;
  public bool hasStarted;

  public TMP_Text exScoreText;
  public TMP_Text maxComboText;
  public TMP_Text comboText;

  private int exScore;
  private int combo;
  private int maxCombo;
  private float lastJudgeTime = 0f;
  private const float JudgeDuration = 1f;

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

    if (lastJudgeTime > 0f && Time.time - lastJudgeTime > JudgeDuration) {
      judgeAnimator.Play("Idle");
      comboText.text = "";
      lastJudgeTime = 0f;
    }
  }

  public void NoteJudge(NoteExScore score, bool isComboBreak = false) {
    exScore += (int)score;
    combo = isComboBreak ? 0 : combo + 1;
    maxCombo = Math.Max(combo, maxCombo);
    lastJudgeTime = Time.time;

    exScoreText.text = $"{exScore:0000}";
    maxComboText.text = $"{maxCombo:0000}";
    comboText.text = combo > 0 ? combo.ToString() : "";
  }

  public void PgreatJudge() {
    NoteJudge(NoteExScore.PGREAT);
    judgeAnimator.Play("Pgreat");
  }

  public void GreatJudge() {
    NoteJudge(NoteExScore.GREAT);
    judgeAnimator.Play("Great");
  }

  public void GoodJudge() {
    NoteJudge(NoteExScore.GOOD);
    judgeAnimator.Play("Good");
  }

  public void BadJudge() {
    NoteJudge(NoteExScore.BAD, isComboBreak: true);
    judgeAnimator.Play("Bad");
  }

  public void PoorJudge() {
    NoteJudge(NoteExScore.POOR, isComboBreak: true);
    judgeAnimator.Play("Poor");
  }
}
