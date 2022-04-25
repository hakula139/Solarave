using UnityEngine;

namespace Play {
  public class FumenScroller : MonoBehaviour {
    public static FumenScroller instance;

    public Animator judgeLineLight;
    public Animator progressBar;
    public Animator difficultyFrame;

    public bool isEnabled;
    public float bpm;
    public float baseSpeed;
    public float hiSpeed;
    public float Speed => bpm * baseSpeed * hiSpeed / 24000f * Time.deltaTime;
    public float currentTime;   // ms
    public float lastNoteTime;  // ms
    public float offset;        // ms
    public float TimeLeft => lastNoteTime - currentTime;
    public float progressBarTrackLength;

    private void Awake() {
      instance = this;
    }

    private void Update() {
      if (isEnabled) {
        float deltaTime = Time.deltaTime * 1000f;
        currentTime += deltaTime;

        transform.Translate(Vector3.down * Speed);
        float progressBarSpeed = progressBarTrackLength * deltaTime / lastNoteTime;
        progressBar.gameObject.transform.Translate(Vector3.down * progressBarSpeed);
      }
    }

    public void Enable() {
      InitializeUI();
      offset = (float)AudioSettings.dspTime * 1000f;
      isEnabled = true;
    }

    public void Disable() {
      isEnabled = false;
    }

    private void InitializeUI() {
      float animSpeed = bpm / 120f;
      judgeLineLight.SetTrigger("IsEnabled");
      judgeLineLight.speed = animSpeed;
      progressBar.SetTrigger("IsEnabled");
      progressBar.speed = animSpeed;
      difficultyFrame.SetTrigger("IsEnabled");
      difficultyFrame.speed = animSpeed;
    }
  }
}
