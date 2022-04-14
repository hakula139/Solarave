using UnityEngine;
namespace Play {
  public class ScratchController : KeyController {
    public new void Start() {
      sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

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
}
