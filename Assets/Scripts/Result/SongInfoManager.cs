using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Result {
  public class SongInfoManager : MonoBehaviour {
    public static SongInfoManager instance;

    public TMP_Text titleTMP;

    private void Awake() {
      instance = this;
    }

    private void Start() {
      InitializeUI();
    }

    private void InitializeUI() {
      titleTMP.text = Play.FumenManager.instance.titleTMP.text;
    }
  }
}
