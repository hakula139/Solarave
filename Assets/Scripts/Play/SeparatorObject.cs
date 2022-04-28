using UnityEngine;

namespace Play {
  public class SeparatorObject : MonoBehaviour {
    protected SpriteRenderer sr;

    public float time;
    public static readonly float SpawnY = 4f;
    public static readonly float DespawnY = -1.5f;
    public static readonly float EpsilonY = 0.01f;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      float y = transform.position.y;
      if (y < DespawnY) {
        gameObject.SetActive(false);
      } else if (y < SpawnY) {
        sr.enabled = true;
      }

      // Adjust time when lost sync.
      if (Mathf.Abs(y + 0.05f) < EpsilonY) {
        Debug.LogFormat("currentTime=<{0}>, timeDiff=<{1}>", FumenScroller.instance.currentTime, time - FumenScroller.instance.currentTime);
      }
    }
  }
}
