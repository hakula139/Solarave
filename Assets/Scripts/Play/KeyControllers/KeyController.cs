using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Play {
  public class KeyController : MonoBehaviour {
    protected SpriteRenderer sr;
    protected List<Animator> bombPool = new();

    protected bool isEnabled;
    public KeyCode keyAssigned;
    public GameObject fumenArea;
    public GameObject notePrefab;
    public Animator laserPrefab;
    public Animator bombPrefab;
    protected const int BombPoolSize = 4;
    protected const float BombDuration = 0.2f;  // s

    public readonly Queue<NoteObject> notes = new();
    protected readonly Queue<KeySound> keySounds = new();
    protected KeySound currentKeySound = null;

    private void Start() {
      sr = GetComponent<SpriteRenderer>();
      SetupBombPool();
    }

    private void Update() {
      if (isEnabled) {
        if (Input.GetKeyDown(keyAssigned)) {
          sr.color = new Color(1, 1, 1, 0.25f);
          laserPrefab.SetBool("KeyDown", true);
          JudgeNote();
          PlayKeySound();
        }

        if (Input.GetKeyUp(keyAssigned)) {
          sr.color = new Color(1, 1, 1, 0);
          laserPrefab.SetBool("KeyDown", false);
        }
      }
    }

    public void Enable() {
      isEnabled = true;
    }

    public void Disable() {
      isEnabled = false;
    }

    public float SetupNote(float start, float length, BMS.Note note) {
      GameObject noteClone = Instantiate(notePrefab, fumenArea.transform);
      float y = start + (length * note.position);
      float ratio = FumenScroller.instance.baseSpeed * FumenScroller.instance.hiSpeed / 100f;
      noteClone.transform.Translate(ratio * y * Vector3.up);
      noteClone.SetActive(true);

      NoteObject noteObject = noteClone.GetComponent<NoteObject>();
      noteObject.time = y * 240000f / FumenScroller.instance.bpm;
      notes.Enqueue(noteObject);

      // Debug.LogFormat("setup note: position=<{0}> keyAssigned=<{1}> time=<{2}>", noteClone.transform.position, keyAssigned, noteObject.time);

      SetupKeySound(note.wavId, noteObject.time);
      return noteObject.time;
    }

    protected class KeySound {
      public int wavId;
      public float time;
      public bool isTriggered = false;  // note has been judged
    }

    public void SetupKeySound(int wavId, float time) {
      keySounds.Enqueue(new() {
        wavId = wavId,
        time = time,
      });
    }

    public virtual void PlayKeySound() {
      float currentTime = FumenScroller.instance.currentTime;

      if (currentKeySound is null || currentKeySound.isTriggered) {
        // Try to find the latest key sound to play.
        while (keySounds.TryPeek(out KeySound keySound) && currentTime >= keySound.time - FumenManager.instance.poorRange) {
          currentKeySound = keySound;
          _ = keySounds.Dequeue();
          if (currentTime <= keySound.time + FumenManager.instance.badRange) {
            break;
          }
        }
      }

      if (currentKeySound != null) {
        AudioLoader.instance.Play(currentKeySound.wavId, currentTime);
      }
    }

    public void SetCurrentKeySoundToTriggered() {
      if (currentKeySound != null) {
        currentKeySound.isTriggered = true;
      }
    }

    public void JudgeNote() {
      if (!notes.TryPeek(out NoteObject noteObject)) {
        return;
      }

      float currentTime = FumenScroller.instance.currentTime;
      float d = currentTime - noteObject.time - FumenManager.instance.inputLatency;
      float error = Mathf.Abs(d);
      bool isEarly = d < 0;

      if (noteObject.isClickable) {
        if (error <= FumenManager.instance.badRange) {
          if (error <= FumenManager.instance.pgreatRange) {
            // Debug.LogFormat("pgreat: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, noteObject.time);
            GameManager.instance.PgreatJudge();
            TriggerBomb();
          } else if (error <= FumenManager.instance.greatRange) {
            // Debug.LogFormat("great: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, noteObject.time);
            GameManager.instance.GreatJudge();
            TriggerBomb();
          } else if (error <= FumenManager.instance.goodRange) {
            // Debug.LogFormat("good: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, noteObject.time);
            GameManager.instance.GoodJudge();
          } else {
            // Debug.LogFormat("bad: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, noteObject.time);
            GameManager.instance.BadJudge();
          }
          noteObject.Disable();
        } else if (error <= FumenManager.instance.poorRange && isEarly) {
          // Debug.LogFormat("poor: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, currentTime, noteObject.time);
          GameManager.instance.PoorJudge();
        }
      }
    }

    protected void SetupBombPool() {
      for (int i = 0; i < BombPoolSize; i++) {
        Animator bombClone = Instantiate(bombPrefab, transform);
        bombClone.gameObject.SetActive(false);
        bombPool.Add(bombClone);
      }
    }

    protected void TriggerBomb() {
      for (int i = 0; i < BombPoolSize; i++) {
        if (!bombPool[i].isActiveAndEnabled) {
          Animator bombClone = bombPool[i];
          bombClone.gameObject.SetActive(true);
          bombClone.Play("Bomb");
          _ = StartCoroutine(DisableBomb(bombClone));
          break;
        }
      }
    }

    protected IEnumerator DisableBomb(Animator bomb) {
      yield return new WaitForSeconds(BombDuration);
      bomb.gameObject.SetActive(false);
    }
  }
}
