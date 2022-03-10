using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {
  protected SpriteRenderer sr;
  public FumenScroller scroller;

  public KeyCode keyAssigned;
  public GameObject fumenArea;
  public GameObject notePrefab;

  public Queue<GameObject> notes = new();

  public void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  public void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0.25f);
      GameObject note = notes.Peek();
      JudgeNote(note);
    }

    if (Input.GetKeyUp(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0);
    }
  }

  public void SetupNote(float start, float length, BMS.Note note) {
    GameObject noteClone = Instantiate(notePrefab, fumenArea.transform);
    float y = start + (length * note.position);
    noteClone.transform.Translate(Vector3.up * y * scroller.baseSpeed * scroller.hiSpeed / 100f);
    noteClone.SetActive(true);

    NoteObject noteObject = noteClone.GetComponent<NoteObject>();
    noteObject.time = y * 240000f / scroller.bpm;
    notes.Enqueue(noteClone);

    // Debug.LogFormat("setup note: position=<{0}> keyAssigned=<{1}> time=<{2}>", noteClone.transform.position, keyAssigned, noteObject.time);
  }

  public void JudgeNote(GameObject note) {
    NoteObject noteObject = note.GetComponent<NoteObject>();
    float d = scroller.currentTime - noteObject.time;
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
        notes.Dequeue();
      } else if (error <= FumenManager.instance.poorRange && isEarly) {
        Debug.LogFormat("poor: d=<{0}> currentTime=<{1}> noteTime=<{2}>", d, scroller.currentTime, noteObject.time);
        GameManager.instance.PoorJudge();
      }
    }
  }
}
