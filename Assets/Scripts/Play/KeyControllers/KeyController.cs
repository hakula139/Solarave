using UnityEngine;

public class KeyController : MonoBehaviour {
  protected SpriteRenderer sr;

  public KeyCode keyAssigned;
  public GameObject fumenArea;
  public GameObject notePrefab;

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
    GameObject noteClone = Instantiate(notePrefab, fumenArea.transform);
    noteClone.transform.Translate(Vector3.up * (start + (length * note.position)));
    noteClone.SetActive(true);
    NoteObject noteObject = noteClone.GetComponent<NoteObject>();
    noteObject.keyAssigned = keyAssigned;
    noteObject.time = 0;
    Debug.LogFormat("setup note: position=<{0}> keyAssigned=<{1}> time=<{2}>", noteClone.transform.position, noteObject.keyAssigned, noteObject.time);
  }
}
