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

  private void OnEnable() {
    instance = this;
  }

  public void Start() {
    totalNotes = FindObjectsOfType<NoteObject>().Length;
  }

  public void Update() {
    if (lastJudgeTime > 0f && Time.time - lastJudgeTime > JudgeDuration) {
      judgeTMP.text = " ";
      lastJudgeTime = 0f;
    }
  }

  protected void NoteJudge(Judge judge, int scoreAdded = 0, int comboAdded = 1) {
    exScore += scoreAdded;
    combo += comboAdded;
    maxCombo = Math.Max(combo, maxCombo);
    lastJudgeTime = Time.time;

    exScoreTMP.text = $"{exScore:0000}";
    maxComboTMP.text = $"{maxCombo:0000}";
    string judgeText = SpriteAssetHelper.GetJudge(judge);
    string comboText = comboAdded > 0 ? $"  {SpriteAssetHelper.GetInteger(judge, combo)}" : "";
    judgeTMP.text = judgeText + comboText;
  }

  public void PgreatJudge() {
    NoteJudge(judge: Judge.PGREAT, scoreAdded: 2);
    pgreatCount++;
  }

  public void GreatJudge() {
    NoteJudge(judge: Judge.GREAT, scoreAdded: 1);
    greatCount++;
  }

  public void GoodJudge() {
    NoteJudge(judge: Judge.GOOD);
    goodCount++;
  }

  public void BadJudge() {
    NoteJudge(judge: Judge.BAD, comboAdded: -combo);
    badCount++;
  }

  // 空 POOR
  public void PoorJudge() {
    NoteJudge(judge: Judge.POOR, comboAdded: 0);
    poorCount++;
  }

  public void MissJudge() {
    NoteJudge(judge: Judge.POOR, comboAdded: -combo);
    poorCount++;
  }
}
