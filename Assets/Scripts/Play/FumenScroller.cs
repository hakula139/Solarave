using UnityEngine;

public class FumenScroller : MonoBehaviour {
  public bool isEnabled = false;
  public float bpm;
  public float baseSpeed = 1f;
  public float hiSpeed;
  protected float Speed => bpm * baseSpeed * hiSpeed / 24000f * Time.deltaTime;

  public void Update() {
    if (isEnabled) {
      transform.Translate(Vector3.down * Speed);
    }
  }
}
