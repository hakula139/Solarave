using UnityEngine;

public class BeatScroller : MonoBehaviour {
  public float bpm;
  public bool isEnabled;

  public void Start() {
    bpm /= 60f;  // converts to beats per second
  }

  public void Update() {
    if (isEnabled) {
      transform.Translate(Vector3.down * bpm * Time.deltaTime);
    }
  }
}
