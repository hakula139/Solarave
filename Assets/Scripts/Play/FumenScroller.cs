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
    public float Speed => bpm * baseSpeed * hiSpeed / 2.4e7f;
    public float SpeedRatio => baseSpeed * hiSpeed / 100f;
    public PreciseTime currentTime = new();
    public float lastNoteTime;  // ms
    public float offset;        // ms
    public float TimeLeft => lastNoteTime - currentTime.DataMilli;
    public float progressBarTrackLength;

    private void Awake() {
      instance = this;
    }

    private void Update() {
      if (isEnabled) {
        _ = currentTime.Add(Time.deltaTime);

        float deltaTimeMilli = Time.deltaTime * 1000f;
        MoveDown(Speed * deltaTimeMilli);
        float progressBarSpeed = progressBarTrackLength * deltaTimeMilli / lastNoteTime;
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
      separatorClone.transform.Translate(SpeedRatio * y * Vector3.up);
      separatorClone.SetActive(true);

      SeparatorObject separatorObject = separatorClone.GetComponent<SeparatorObject>();
      separatorObject.time = y * 2.4e5f / bpm;
    }

    public void MoveDown(float y) {
      transform.Translate(y * Vector3.down);
    }
  }
}
