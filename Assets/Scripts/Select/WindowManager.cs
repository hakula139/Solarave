using UnityEngine;

namespace Select {
  public class WindowManager : MonoBehaviour {
    public static WindowManager instance;
    public float targetRatio = 16f / 9f;

    private void Awake() {
      instance = this;
    }

    private void Start() {
      float currentRatio = (float)Screen.width / Screen.height;
      if (currentRatio < targetRatio) {
        Screen.SetResolution(Screen.width, Mathf.FloorToInt(Screen.width / targetRatio), Screen.fullScreenMode);
      } else if (currentRatio > targetRatio) {
        Screen.SetResolution(Mathf.FloorToInt(Screen.height * targetRatio), Screen.height, Screen.fullScreenMode);
      }
    }
  }
}
