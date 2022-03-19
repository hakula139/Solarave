using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {
  protected SpriteRenderer sr;
  public AudioLoader audioLoader;
  public FumenScroller scroller;

  public KeyCode keyAssigned;
  public GameObject fumenArea;
  public GameObject notePrefab;
  public readonly Queue<GameObject> notes = new();
  protected readonly Queue<KeySound> keySounds = new();
  protected KeySound currentKeySound;

  public void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  public void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0.25f);
      JudgeNote();
      PlayKeySound();
    }

    if (Input.GetKeyUp(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0);
    }
  }

  public virtual void SetupNote(float start, float length, BMS.Note note) {
    GameObject noteClone = Instantiate(notePrefab, fumenArea.transform);
    float y = start + (length * note.position);
    noteClone.transform.Translate(scroller.baseSpeed * scroller.hiSpeed * y * Vector3.up / 100f);
    noteClone.SetActive(true);
    notes.Enqueue(noteClone);

    NoteObject noteObject = noteClone.GetComponent<NoteObject>();
    noteObject.time = y * 240000f / scroller.bpm;
    // Debug.LogFormat("setup note: position=<{0}> keyAssigned=<{1}> time=<{2}>", noteClone.transform.position, keyAssigned, noteObject.time);

    SetupKeySound(note.wavId, noteObject.time);
  }

  protected class KeySound {
    public int wavId;
    public float time;
  }

  public virtual void SetupKeySound(int wavId, float time) {
    keySounds.Enqueue(new() {
      wavId = wavId,
      time = time - FumenManager.instance.poorRange,
    });
  }

  public virtual void PlayKeySound() {
    // Find the latest key sound to play.
    while (keySounds.TryPeek(out KeySound keySound) && scroller.currentTime >= keySound.time) {
      currentKeySound = keySound;
      _ = keySounds.Dequeue();
    }
    if (currentKeySound is not null) {
      audioLoader.Play(currentKeySound.wavId, scroller.currentTime);
    }
  }

  public void JudgeNote() {
    if (!notes.TryPeek(out GameObject note)) {
      return;
    }

    NoteObject noteObject = note.GetComponent<NoteObject>();
    float d = scroller.currentTime - noteObject.time - FumenManager.instance.inputLatency;
    float error = Mathf.Abs(d);
    bool isEarly = d < 0;

    if (noteObject.isClickable) {
      if (error <= FumenManager.instance.badRange) {
        if (error <= FumenManager.instance.pgreatRange) {
          Debug.LogFormat("pgreat: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
          GameManager.instance.PgreatJudge();
        } else if (error <= FumenManager.instance.greatRange) {
          Debug.LogFormat("great: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
          GameManager.instance.GreatJudge();
        } else if (error <= FumenManager.instance.goodRange) {
          Debug.LogFormat("good: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
          GameManager.instance.GoodJudge();
        } else {
          Debug.LogFormat("bad: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
          GameManager.instance.BadJudge();
        }
        noteObject.isClickable = false;
        _ = notes.Dequeue();
      } else if (error <= FumenManager.instance.poorRange && isEarly) {
        Debug.LogFormat("poor: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
        GameManager.instance.PoorJudge();
      }
    }
  }
}
