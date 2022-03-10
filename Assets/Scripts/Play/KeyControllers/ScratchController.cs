using UnityEngine;

public class ScratchController : KeyController {
  public new void Update() {
    if (Input.GetKey(keyAssigned)) {
      sr.transform.Rotate(Vector3.forward * 360f * Time.deltaTime);
    } else {
      sr.transform.Rotate(Vector3.back * 360f * Time.deltaTime);
    }

    if (Input.GetKeyDown(keyAssigned)) {
      GameObject note;
      if (notes.TryPeek(out note)) {
        JudgeNote(note);
      }
    }
  }
}
