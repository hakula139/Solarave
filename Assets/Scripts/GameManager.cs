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
  public TMP_Text judgeText;
  public TMP_Text maxComboText;

  private int exScore;
  private int combo;
  private int maxCombo;
  private float lastJudgeTime;
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

    if (Time.time - lastJudgeTime > JudgeDuration) {
      judgeAnimator.Play("Idle");
    }
  }

  public void NoteJudge(NoteExScore score, bool isComboBreak = false) {
    exScore += (int)score;
    combo = isComboBreak ? 0 : combo + 1;
    maxCombo = Math.Max(combo, maxCombo);
    lastJudgeTime = Time.time;

    exScoreText.text = $"{exScore:0000}";
    maxComboText.text = $"{maxCombo:0000}";
  }

  public void PgreatJudge() {
    NoteJudge(NoteExScore.PGREAT);
    judgeAnimator.Play("Pgreat");
    judgeText.text = $"PGREAT  {combo}";
  }

  public void GreatJudge() {
    NoteJudge(NoteExScore.GREAT);
    judgeAnimator.Play("Great");
    judgeText.text = $"GREAT  {combo}";
  }

  public void GoodJudge() {
    NoteJudge(NoteExScore.GOOD);
    judgeAnimator.Play("Good");
    judgeText.text = $"GOOD  {combo}";
  }

  public void BadJudge() {
    NoteJudge(NoteExScore.BAD, isComboBreak: true);
    judgeAnimator.Play("Bad");
    judgeText.text = "BAD";
  }

  public void PoorJudge() {
    NoteJudge(NoteExScore.POOR, isComboBreak: true);
    judgeAnimator.Play("Poor");
    judgeText.text = "POOR";
  }
}
