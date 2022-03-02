using UnityEngine;

public class BeatScroller : MonoBehaviour {
  public float bpm;
  public bool isEnabled;

  public void Start() {
    bpm /= 60f;  // converts to beats per second
  }

  public void Update() {
    if (isEnabled) {
      transform.position -= new Vector3(0f, bpm * Time.deltaTime, 0f);
    }
  }
}
