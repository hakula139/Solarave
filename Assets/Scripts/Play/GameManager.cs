using System.Text;
using UnityEngine;
using UnityEngine.UI;
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
    public TMP_Text gaugeTMP;
    public SpriteRenderer gaugeBarSr;
    private static readonly float GaugeBarWidth = 0.07f;
    public TMP_Text judgeTMP;
    public Image fsSprite;

    public int pgreatCount;
    public int greatCount;
    public int goodCount;
    public int badCount;
    public int poorCount;
    public int missCount;
    public int fastCount;
    public int slowCount;
    public int judgedCount;
    public int exScore;
    public int combo;
    public int maxCombo;
    public float gauge;
    public float minGauge;
    public float maxGauge = 100f;
    public float clearGauge;
    public float failedGauge = 2f;

    private float lastJudgeTime;
    private static readonly float JudgeDuration = 1000f;  // ms

    public int NotJudgedCount => FumenManager.instance.totalNotes - judgedCount;
    public int ComboBreakCount => badCount + missCount;
    public int TotalPoorCount => poorCount + missCount;
    public int TotalMissCount => ComboBreakCount + poorCount;
    public float ScoreRate => judgedCount > 0 ? (float)exScore / judgedCount * 50f : 0f;
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
    public int DisplayedGauge => Mathf.FloorToInt(gauge) + (Mathf.FloorToInt(gauge) % 2);
    public bool IsCleared => gauge >= clearGauge;  // stage cleared or not
    public bool IsFailed => gauge < failedGauge;  // sudden death in stage

    private void Awake() {
      instance = this;
    }

    private void Update() {
      if (lastJudgeTime > 0f && FumenScroller.instance.currentTime.DataMilli - lastJudgeTime > JudgeDuration) {
        ClearJudge();
        ClearFastSlow();
        lastJudgeTime = 0f;
      }

      if (FumenScroller.instance.TimeLeft <= 0 || Input.GetKeyDown(KeyCode.Escape)) {
        if (judgedCount == missCount || FumenManager.instance.isAutoMode) {
          // Directly return to Select scene if the player hits nothing or under autoplay mode.
          SceneTransitionManager.instance.EnterScene("Select");
        } else {
          FumenScroller.instance.Disable();
          UpdateResult();
          SceneTransitionManager.instance.EnterScene("Result");
        }
      }
    }

    protected void NoteJudge(Judge judge, int scoreAdded = 0, int comboAdded = 1, float gaugeAdded = 0) {
      UpdateScore(exScore + scoreAdded);
      UpdateCombo(combo + comboAdded);
      UpdateGauge(gauge + gaugeAdded);
      UpdateJudge(judge, displayCombo: comboAdded > 0);
    }

    public void PgreatJudge() {
      pgreatCount++;
      judgedCount++;
      pgreatCountTMP.text = pgreatCount.ToString();
      NoteJudge(judge: Judge.PGREAT, scoreAdded: 2, gaugeAdded: FumenManager.instance.pgreatGauge);
    }

    public void GreatJudge() {
      greatCount++;
      judgedCount++;
      greatCountTMP.text = greatCount.ToString();
      NoteJudge(judge: Judge.GREAT, scoreAdded: 1, gaugeAdded: FumenManager.instance.greatGauge);
    }

    public void GoodJudge() {
      goodCount++;
      judgedCount++;
      goodCountTMP.text = goodCount.ToString();
      NoteJudge(judge: Judge.GOOD, gaugeAdded: FumenManager.instance.goodGauge);
    }

    public void BadJudge() {
      badCount++;
      judgedCount++;
      badCountTMP.text = badCount.ToString();
      NoteJudge(judge: Judge.BAD, comboAdded: -combo, gaugeAdded: FumenManager.instance.badGauge);
    }

    // 空 POOR
    public void PoorJudge() {
      poorCount++;
      poorCountTMP.text = TotalPoorCount.ToString();
      NoteJudge(judge: Judge.POOR, comboAdded: 0, gaugeAdded: FumenManager.instance.poorGauge);
    }

    public void MissJudge() {
      missCount++;
      judgedCount++;
      poorCountTMP.text = TotalPoorCount.ToString();
      NoteJudge(judge: Judge.POOR, comboAdded: -combo, gaugeAdded: FumenManager.instance.missGauge);
    }

    public void UpdateScore(int exScore) {
      this.exScore = exScore;
      exScoreTMP.text = $"{exScore:D4}";
      scoreRateTMP.text = Mathf.FloorToInt(ScoreRate).ToString();
    }

    public void UpdateCombo(int combo) {
      this.combo = combo;
      maxCombo = Mathf.Max(combo, maxCombo);
      maxComboTMP.text = $"{maxCombo:D4}";
    }

    public void UpdateGauge(float gauge) {
      this.gauge = Mathf.Min(Mathf.Max(gauge, minGauge), maxGauge);
      gaugeTMP.text = SpriteAssetHelper.instance.GetInteger(DisplayedGauge);
      gaugeBarSr.size = new Vector2(DisplayedGauge / 2 * GaugeBarWidth, gaugeBarSr.size.y);
    }

    public void UpdateJudge(Judge judge, bool displayCombo) {
      lastJudgeTime = FumenScroller.instance.currentTime.DataMilli;
      StringBuilder judgeText = new(SpriteAssetHelper.instance.GetJudge(judge));
      if (displayCombo) {
        _ = judgeText.Append("  ").Append(SpriteAssetHelper.instance.GetComboInJudge(judge, combo));
      }
      judgeTMP.text = judgeText.ToString();
      judgeTMP.gameObject.SetActive(true);
    }

    public void ClearJudge() {
      judgeTMP.gameObject.SetActive(false);
    }

    public void UpdateFastSlow(bool isEarly) {
      if (isEarly) {
        fastCount++;
      } else {
        slowCount++;
      }
      fsSprite.sprite = SpriteAssetHelper.instance.GetFastSlowIndicatorSprite(isEarly);
      fsSprite.gameObject.SetActive(true);
    }

    public void ClearFastSlow() {
      fsSprite.gameObject.SetActive(false);
    }

    public void UpdateResult() {
      if (NotJudgedCount > 0) {
        missCount += NotJudgedCount;
        gauge = 0f;
      }
      judgedCount = FumenManager.instance.totalNotes;
    }
  }
}
