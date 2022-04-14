using System;
using UnityEngine;
using TMPro;

namespace Play {

  public enum Judge {
    PGREAT,
    GREAT,
    GOOD,
    BAD,
    POOR,
  }

  public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public TMP_Text pgreatCountTMP;
    public TMP_Text greatCountTMP;
    public TMP_Text goodCountTMP;
    public TMP_Text badCountTMP;
    public TMP_Text poorCountTMP;
    public TMP_Text scoreRateTMP;
    public TMP_Text exScoreTMP;
    public TMP_Text maxComboTMP;
    public TMP_Text judgeTMP;

    private int exScore;
    private int combo;
    private int maxCombo;
    private float lastJudgeTime;
    private const float JudgeDuration = 1f;

    private int pgreatCount;
    private int greatCount;
    private int goodCount;
    private int badCount;
    private int poorCount;
    private int totalCount = 0;
    private float scoreRate;

    private void Awake() {
      instance = this;
    }

    public void Update() {
      if (lastJudgeTime > 0f && Time.time - lastJudgeTime > JudgeDuration) {
        judgeTMP.text = " ";
        lastJudgeTime = 0f;
      }
    }

    protected void NoteJudge(Judge judge, int scoreAdded = 0, int comboAdded = 1) {
      exScore += scoreAdded;
      scoreRate = totalCount > 0 ? (float)exScore / totalCount * 50f : 0;
      combo += comboAdded;
      maxCombo = Math.Max(combo, maxCombo);
      lastJudgeTime = Time.time;

      exScoreTMP.text = $"{exScore:0000}";
      scoreRateTMP.text = Math.Floor(scoreRate).ToString();
      maxComboTMP.text = $"{maxCombo:0000}";
      string judgeText = SpriteAssetHelper.instance.GetJudge(judge);
      string comboText = comboAdded > 0 ? $"  {SpriteAssetHelper.instance.GetInteger(judge, combo)}" : "";
      judgeTMP.text = judgeText + comboText;
    }

    public void PgreatJudge() {
      NoteJudge(judge: Judge.PGREAT, scoreAdded: 2);
      totalCount++;
      pgreatCount++;
      pgreatCountTMP.text = pgreatCount.ToString();
    }

    public void GreatJudge() {
      NoteJudge(judge: Judge.GREAT, scoreAdded: 1);
      totalCount++;
      greatCount++;
      greatCountTMP.text = greatCount.ToString();
    }

    public void GoodJudge() {
      NoteJudge(judge: Judge.GOOD);
      totalCount++;
      goodCount++;
      goodCountTMP.text = goodCount.ToString();
    }

    public void BadJudge() {
      NoteJudge(judge: Judge.BAD, comboAdded: -combo);
      totalCount++;
      badCount++;
      badCountTMP.text = badCount.ToString();
    }

    // 空 POOR
    public void PoorJudge() {
      NoteJudge(judge: Judge.POOR, comboAdded: 0);
      poorCount++;
      poorCountTMP.text = poorCount.ToString();
    }

    public void MissJudge() {
      NoteJudge(judge: Judge.POOR, comboAdded: -combo);
      totalCount++;
      poorCount++;
      poorCountTMP.text = poorCount.ToString();
    }
  }
}
