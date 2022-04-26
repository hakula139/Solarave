using UnityEngine;

namespace Play {
  public class FumenScroller : MonoBehaviour {
    public static FumenScroller instance;

    public Animator judgeLineLight;
    public Animator progressBar;
    public Animator difficultyFrame;
    public GameObject separatorPrefab;

    public bool isEnabled;
    public float bpm;
    public float baseSpeed;
    public float hiSpeed;
    public float Speed => bpm * baseSpeed * hiSpeed / 24000f * Time.deltaTime;
    public float SpeedRatio => baseSpeed * hiSpeed / 100f;
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

        transform.Translate(Speed * Vector3.down);
        float progressBarSpeed = progressBarTrackLength * deltaTime / lastNoteTime;
        progressBar.gameObject.transform.Translate(progressBarSpeed * Vector3.down);
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

    public void SetupSeparator(float y) {
      GameObject separatorClone = Instantiate(separatorPrefab, transform);
      separatorClone.transform.Translate((SpeedRatio * y) * Vector3.up);
      separatorClone.SetActive(true);
    }
  }
}
