using UnityEngine;

namespace Play {
  public class SeparatorObject : MonoBehaviour {
    protected SpriteRenderer sr;

    public float time;
    public static readonly float SpawnY = 4f;
    public static readonly float DespawnY = -1.22f;
    public static readonly float EpsilonY = 0.01f;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      float y = transform.position.y;
      if (y < DespawnY) {
        FixOffset();
        gameObject.SetActive(false);
      } else if (y < SpawnY) {
        sr.enabled = true;
      }
    }

    private void FixOffset() {
      // Expected arriving time is 'time', but actual arriving time is 'currentTime'.
      float currentTime = FumenScroller.instance.currentTime.DataMilli;
      float offset = currentTime - time;
      Debug.LogFormat("currentTime=<{0}>, offset=<{1}>", currentTime, offset);
      if (offset > 0) {
        FumenScroller.instance.MoveDown(FumenScroller.instance.Speed * offset);
      }
    }
  }
}
