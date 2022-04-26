using UnityEngine;

namespace Play {
  public class SeparatorObject : MonoBehaviour {
    protected SpriteRenderer sr;

    public float spawnY;
    public float despawnY;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      float y = transform.position.y;
      if (y < despawnY) {
        gameObject.SetActive(false);
      } else if (y < spawnY) {
        sr.enabled = true;
      }
    }
  }
}
