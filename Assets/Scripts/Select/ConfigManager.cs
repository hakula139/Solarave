using UnityEngine;

namespace Select {
  public enum GaugeMode {
    AEASY,
    EASY,
    NORMAL,
    HARD,
    EXHARD,
    GRADE,
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
