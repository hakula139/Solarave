using UnityEngine;

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
