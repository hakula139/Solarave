using UnityEngine;

namespace Select {
  public enum GaugeMode {
    AEASY,
    EASY,
    NORMAL,
    HARD,
    EXHARD,
    DAN,
  }

  public class ConfigManager : MonoBehaviour {
    public static ConfigManager instance;

    public GaugeMode gaugeMode;
    public float hiSpeed;

    private void Awake() {
      instance = this;
    }
  }
}
