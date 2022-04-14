using UnityEngine;

namespace Play {
  public class NoteObject : MonoBehaviour {
    protected SpriteRenderer sr;

    public bool isClickable = false;
    public float time;
    public KeyController lane;
    public float spawnY;
    public float despawnY;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      if (transform.position.y < despawnY) {
        sr.enabled = false;
      } else if (transform.position.y < spawnY) {
        sr.enabled = true;
      }

      float currentTime = FumenScroller.instance.currentTime;
      float d = currentTime - time;

      if (d > FumenManager.instance.badRange) {
        if (isClickable) {
          // Debug.LogFormat("miss: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, time);
          GameManager.instance.MissJudge();
          Disable();
        }
        gameObject.SetActive(false);
      } else if (d >= -FumenManager.instance.badRange) {
      } else if (d >= -FumenManager.instance.poorRange) {
        isClickable = true;
      }
    }

    public void Disable() {
      isClickable = false;
      _ = lane.notes.Dequeue();
      lane.SetCurrentKeySoundToTriggered();
    }
  }
}
