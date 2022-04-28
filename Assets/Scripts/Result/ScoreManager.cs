using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Result {
  public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance;

    public SpriteRenderer backgroundSr;
    public Image titleSprite;

    public Image djLevelLargeSprite;
    public Image gaugeModeSprite;
    public TMP_Text gaugeText;

    public Image clearTypeSprite;
    public Image djLevelSprite;
    public TMP_Text exScoreTMP;
    public TMP_Text missCountTMP;

    public TMP_Text fastTMP;
    public TMP_Text slowTMP;
    public TMP_Text comboTMP;
    public TMP_Text pgreatCountTMP;
    public TMP_Text greatCountTMP;
    public TMP_Text goodCountTMP;
    public TMP_Text badCountTMP;
    public TMP_Text poorCountTMP;
    public TMP_Text scoreRateTMP;

    public AudioSource bgm;

    private void Awake() {
      instance = this;
    }

    private void Start() {
      InitializeUI();
      InitializeAudio();
    }

    private void Update() {
      if (Input.anyKeyDown) {
        SceneTransitionManager.instance.EnterScene("Select");
      }
    }

    private void InitializeUI() {
      bool isCleared = Play.GameManager.instance.IsCleared;

      backgroundSr.sprite = SpriteAssetHelper.instance.GetBackgroundSprite(isCleared);
      titleSprite.sprite = SpriteAssetHelper.instance.GetTitleSprite(isCleared);

      djLevelLargeSprite.sprite = SpriteAssetHelper.instance.GetDjLevelLargeSprite(Play.GameManager.instance.ScoreDjLevel);
      djLevelLargeSprite.SetNativeSize();
      gaugeModeSprite.sprite = SpriteAssetHelper.instance.GetGaugeModeSprite(Select.ConfigManager.instance.gaugeMode);
      gaugeText.text = Play.GameManager.instance.DisplayedGauge.ToString();

      clearTypeSprite.sprite = SpriteAssetHelper.instance.GetClearTypeSprite(Select.ConfigManager.instance.gaugeMode, isCleared);
      djLevelSprite.sprite = SpriteAssetHelper.instance.GetDjLevelSprite(Play.GameManager.instance.ScoreDjLevel);
      djLevelSprite.SetNativeSize();
      exScoreTMP.text = Play.GameManager.instance.exScore.ToString();
      missCountTMP.text = Play.GameManager.instance.TotalMissCount.ToString();

      fastTMP.text = Play.GameManager.instance.fastCount.ToString();
      slowTMP.text = Play.GameManager.instance.slowCount.ToString();
      comboTMP.text = $"{Play.GameManager.instance.maxCombo} / {Play.FumenManager.instance.totalNotes} ({Play.GameManager.instance.ComboBreakCount})";
      pgreatCountTMP.text = Play.GameManager.instance.pgreatCount.ToString();
      greatCountTMP.text = Play.GameManager.instance.greatCount.ToString();
      goodCountTMP.text = Play.GameManager.instance.goodCount.ToString();
      badCountTMP.text = Play.GameManager.instance.badCount.ToString();
      poorCountTMP.text = Play.GameManager.instance.TotalPoorCount.ToString();
      scoreRateTMP.text = $"{Play.GameManager.instance.ScoreRate:0.00}%";
    }

    private void InitializeAudio() {
      bgm.clip = AudioAssetHelper.instance.GetBgmClip(Play.GameManager.instance.IsCleared);
      bgm.Play();
    }
  }
}
