using UnityEngine;

public class KeyController : MonoBehaviour {
  protected SpriteRenderer sr;

  public KeyCode keyAssigned;
  public NoteObject notePrefab;

  public void Start() {
    sr = GetComponent<SpriteRenderer>();
  }

  public void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0.25f);
    }
    if (Input.GetKeyUp(keyAssigned)) {
      sr.color = new Color(1, 1, 1, 0);
    }
  }

  public void SetupNote(float start, float length, BMS.Note note) {
    NoteObject noteObj = Instantiate(notePrefab, transform);
    noteObj.transform.Translate(Vector3.up * (start + (length * note.position)));
    noteObj.keyAssigned = keyAssigned;
    noteObj.time = 0;
    Debug.LogFormat("setup note: position=<{0}> keyAssigned=<{1}> time=<{2}>", noteObj.transform.position, noteObj.keyAssigned, noteObj.time);
  }
}

public class ScratchController : KeyController {
  public new void Update() {
    if (Input.GetKeyDown(keyAssigned)) {
      sr.transform.Rotate(Vector3.back * Time.deltaTime);
    }
    if (Input.GetKeyUp(keyAssigned)) {
      sr.transform.Rotate(Vector3.forward * Time.deltaTime);
    }
  }
}

public class BgmController : KeyController {
  public new void Start() {
  }

  public new void Update() {
  }
}
