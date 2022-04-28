using UnityEngine;

namespace Play {
  public class NoteObject : MonoBehaviour {
    protected SpriteRenderer sr;

    public bool isClickable = false;
    public float time;
    public KeyController lane;

    public static readonly float SpawnY = 4f;
    public static readonly float DespawnY = -1.3f;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      float y = transform.position.y;
      if (y < DespawnY) {
        sr.enabled = false;
      } else if (y < SpawnY) {
        sr.enabled = true;
      }

      float currentTime = FumenScroller.instance.currentTime.DataMilli;
      float d = currentTime - time;

      if (d > FumenManager.instance.badRange) {
        if (isClickable) {
          // Debug.LogFormat("miss: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, time);
          GameManager.instance.MissJudge();
          GameManager.instance.ClearFastSlow();
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
