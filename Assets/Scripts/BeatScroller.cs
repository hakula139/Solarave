using UnityEngine;

public class BeatScroller : MonoBehaviour {
  public float bpm;
  public bool isEnabled;

  void Start() {
    bpm /= 60f;
  }

  void Update() {
    if (isEnabled) {
      transform.position -= new Vector3(0f, bpm * Time.deltaTime, 0f);
    }
  }
}
