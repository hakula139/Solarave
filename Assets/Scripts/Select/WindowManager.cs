using UnityEngine;

namespace Select {
  public class WindowManager : MonoBehaviour {
    public float targetRatio = 16f / 9f;

    private void Start() {
      float currentRatio = (float)Screen.width / Screen.height;
      if (currentRatio < targetRatio) {
        Screen.SetResolution(Screen.width, (int)Mathf.Floor(Screen.width / targetRatio), Screen.fullScreenMode);
      } else if (currentRatio > targetRatio) {
        Screen.SetResolution((int)Mathf.Floor(Screen.height * targetRatio), Screen.height, Screen.fullScreenMode);
      }
    }
  }
}
