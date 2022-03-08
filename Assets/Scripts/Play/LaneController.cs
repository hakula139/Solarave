using UnityEngine;

public class LaneController : MonoBehaviour {
  private SpriteRenderer sr;

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
    var noteObj = Instantiate(notePrefab, transform);
    noteObj.transform.Translate(Vector3.up * (start + length * note.position));
    noteObj.keyAssigned = keyAssigned;
    noteObj.time = 0;
  }
}
