using System;
using UnityEngine;
using TMPro;

public enum Judge {
  PGREAT,
  GREAT,
  GOOD,
  BAD,
  POOR,
}

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  public TMP_Text exScoreTMP;
  public TMP_Text maxComboTMP;
  public TMP_Text judgeTMP;

  private int exScore;
  private int combo;
  private int maxCombo;
  private float lastJudgeTime;
  private const float JudgeDuration = 1f;

  private int totalNotes;
  private int pgreatCount;
  private int greatCount;
  private int goodCount;
  private int badCount;
  private int poorCount;

  public void Start() {
    instance = this;
    totalNotes = FindObjectsOfType<NoteObject>().Length;
  }

  public void Update() {
    if (lastJudgeTime > 0f && Time.time - lastJudgeTime > JudgeDuration) {
      judgeTMP.text = " ";
      lastJudgeTime = 0f;
    }
  }

  protected void NoteJudge(Judge judge, int score = 0, bool isComboBreak = false) {
    exScore += score;
    combo = isComboBreak ? 0 : combo + 1;
    maxCombo = Math.Max(combo, maxCombo);
    lastJudgeTime = Time.time;

    exScoreTMP.text = $"{exScore:0000}";
    maxComboTMP.text = $"{maxCombo:0000}";
    judgeTMP.text = SpriteAssetHelper.GetJudge(judge) + (combo > 0 ? $"  {SpriteAssetHelper.GetInteger(judge, combo)}" : "");
  }

  public void PgreatJudge() {
    NoteJudge(judge: Judge.PGREAT, score: 2);
    pgreatCount++;
  }

  public void GreatJudge() {
    NoteJudge(judge: Judge.GREAT, score: 1);
    greatCount++;
  }

  public void GoodJudge() {
    NoteJudge(judge: Judge.GOOD);
    goodCount++;
  }

  public void BadJudge() {
    NoteJudge(judge: Judge.BAD, isComboBreak: true);
    badCount++;
  }

  public void PoorJudge() {
    NoteJudge(judge: Judge.POOR, isComboBreak: true);
    poorCount++;
  }
}
