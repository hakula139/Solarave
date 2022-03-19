using UnityEngine;

public class ScratchController : KeyController {
  public new void Update() {
    if (Input.GetKey(keyAssigned)) {
      sr.transform.Rotate(360f * Time.deltaTime * Vector3.forward);
    } else {
      sr.transform.Rotate(360f * Time.deltaTime * Vector3.back);
    }

    if (Input.GetKeyDown(keyAssigned)) {
      JudgeNote();
      PlayKeySound();
    }
  }
}
