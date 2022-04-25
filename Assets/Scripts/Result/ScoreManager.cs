using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Result {
  public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance;

    public Image djLevelLargeSprite;

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

    private void Awake() {
      instance = this;
    }

    private void Start() {
      Play.GameManager.instance.UpdateResult();
      InitializeUI();
    }

    private void Update() {
      if (Input.anyKeyDown) {
        SceneTransitionManager.instance.EnterScene("Select");
      }
    }

    private void InitializeUI() {
      djLevelLargeSprite.sprite = SpriteAssetHelper.instance.GetDjLevelLargeSprite(Play.GameManager.instance.ScoreDjLevel);
      djLevelLargeSprite.SetNativeSize();

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
  }
}
