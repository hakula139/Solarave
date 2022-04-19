using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Play {

  public enum Judge {
    PGREAT,
    GREAT,
    GOOD,
    BAD,
    POOR,
  }

  public enum DjLevel {
    F,
    E,
    D,
    C,
    B,
    A,
    AA,
    AAA,
    MAX,
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

    public int exScore;
    private int combo;
    public int maxCombo;
    private float lastJudgeTime;
    private const float JudgeDuration = 1f;  // s

    public int pgreatCount;
    public int greatCount;
    public int goodCount;
    public int badCount;
    public int poorCount;
    public int totalCount;
    public int comboBreakCount;
    public int notJudgedCount;
    public int MissCount => badCount + poorCount + notJudgedCount;
    public float ScoreRate => totalCount > 0 ? (float)exScore / totalCount * 50f : 0f;
    public DjLevel ScoreDjLevel => ScoreRate switch {
      >= 100f => DjLevel.MAX,
      >= 800f / 9f => DjLevel.AAA,
      >= 700f / 9f => DjLevel.AA,
      >= 600f / 9f => DjLevel.A,
      >= 500f / 9f => DjLevel.B,
      >= 400f / 9f => DjLevel.C,
      >= 300f / 9f => DjLevel.D,
      >= 200f / 9f => DjLevel.E,
      _ => DjLevel.F
    };

    private void Awake() {
      instance = this;
    }

    private void Update() {
      if (lastJudgeTime > 0f && Time.time - lastJudgeTime > JudgeDuration) {
        judgeTMP.text = " ";
        lastJudgeTime = 0f;
      }

      if (FumenScroller.instance.TimeLeft <= 0) {
        SceneManager.LoadScene("Result");
      } else if (Input.GetKeyDown(KeyCode.Escape)) {
        if (totalCount == poorCount) {
          // Directly return to Select scene if the player hits nothing.
          SceneManager.LoadScene("Select");
        } else {
          SceneManager.LoadScene("Result");
        }
      }
    }

    protected void NoteJudge(Judge judge, int scoreAdded = 0, int comboAdded = 1) {
      exScore += scoreAdded;
      combo += comboAdded;
      maxCombo = Math.Max(combo, maxCombo);
      lastJudgeTime = Time.time;

      exScoreTMP.text = $"{exScore:D4}";
      scoreRateTMP.text = Math.Floor(ScoreRate).ToString();
      maxComboTMP.text = $"{maxCombo:D4}";
      string judgeText = SpriteAssetHelper.instance.GetJudge(judge);
      string comboText = comboAdded > 0 ? $"  {SpriteAssetHelper.instance.GetInteger(judge, combo)}" : "";
      judgeTMP.text = judgeText + comboText;
    }

    public void PgreatJudge() {
      NoteJudge(judge: Judge.PGREAT, scoreAdded: 2);
      pgreatCount++;
      totalCount++;
      pgreatCountTMP.text = pgreatCount.ToString();
    }

    public void GreatJudge() {
      NoteJudge(judge: Judge.GREAT, scoreAdded: 1);
      greatCount++;
      totalCount++;
      greatCountTMP.text = greatCount.ToString();
    }

    public void GoodJudge() {
      NoteJudge(judge: Judge.GOOD);
      goodCount++;
      totalCount++;
      goodCountTMP.text = goodCount.ToString();
    }

    public void BadJudge() {
      NoteJudge(judge: Judge.BAD, comboAdded: -combo);
      badCount++;
      totalCount++;
      comboBreakCount++;
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
      poorCount++;
      totalCount++;
      comboBreakCount++;
      poorCountTMP.text = poorCount.ToString();
    }

    public void UpdateResult() {
      notJudgedCount = FumenManager.instance.totalNotes - totalCount;
      totalCount = FumenManager.instance.totalNotes;
    }
  }
}
