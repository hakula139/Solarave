using System;
using System.Collections;
using UnityEngine;
using TMPro;

public enum JudgeState {
  PGREAT = 5,
  GREAT = 4,
  GOOD = 3,
  BAD = 2,
  POOR = 1,
}

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
  public Animator judgeAnimator;

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

  IEnumerator ExitJudgeAnimation(float duration = 1) {
    yield return new WaitForSeconds(duration);
    judgeAnimator.SetInteger("state", 0);
  }

  public void NoteJudge(NoteExScores score, bool isComboBreak = false) {
    exScore += (int)score;
    combo = isComboBreak ? 0 : combo + 1;
    maxCombo = Math.Max(combo, maxCombo);

    exScoreText.text = $"{exScore:0000}";
    maxComboText.text = $"{maxCombo:0000}";

    StartCoroutine(ExitJudgeAnimation());
  }

  public void PgreatJudge() {
    NoteJudge(NoteExScores.PGREAT);
    judgeAnimator.SetInteger("state", (int)JudgeState.PGREAT);
    judgeText.text = $"PGREAT  {combo}";
  }

  public void GreatJudge() {
    NoteJudge(NoteExScores.GREAT);
    judgeAnimator.SetInteger("state", (int)JudgeState.GREAT);
    judgeText.text = $"GREAT  {combo}";
  }

  public void GoodJudge() {
    NoteJudge(NoteExScores.GOOD);
    judgeAnimator.SetInteger("state", (int)JudgeState.GOOD);
    judgeText.text = $"GOOD  {combo}";
  }

  public void BadJudge() {
    NoteJudge(NoteExScores.BAD, true);
    judgeAnimator.SetInteger("state", (int)JudgeState.BAD);
    judgeText.text = "BAD";
  }

  public void PoorJudge() {
    NoteJudge(NoteExScores.POOR, true);
    judgeAnimator.SetInteger("state", (int)JudgeState.POOR);
    judgeText.text = "POOR";
  }
}
