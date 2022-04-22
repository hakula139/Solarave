using UnityEngine;

namespace Play {
  public class FumenScroller : MonoBehaviour {
    public static FumenScroller instance;

    public Animator judgeLineLight;

    protected bool isEnabled;
    public float bpm;
    public float baseSpeed;
    public float hiSpeed;
    public float Speed => bpm * baseSpeed * hiSpeed / 24000f * Time.deltaTime;
    public float currentTime;   // ms
    public float lastNoteTime;  // ms
    public float offset;        // ms
    public float TimeLeft => lastNoteTime - currentTime + 3000f;

    private void Awake() {
      instance = this;
    }

    private void Update() {
      if (isEnabled) {
        transform.Translate(Vector3.down * Speed);
        currentTime += Time.deltaTime * 1000f;
      }
    }

    public void Enable() {
      offset = (float)AudioSettings.dspTime * 1000f;
      judgeLineLight.SetTrigger("IsEnabled");
      isEnabled = true;
    }

    public void Disable() {
      isEnabled = false;
    }
  }
}
