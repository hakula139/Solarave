using UnityEngine;
namespace Play {
  public class ScratchController : KeyController {
    private void Start() {
      sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
      SetupBombPool();
    }

    private void Update() {
      if (Input.GetKey(keyAssigned)) {
        sr.transform.Rotate(360f * Time.deltaTime * Vector3.forward);
      } else {
        sr.transform.Rotate(360f * Time.deltaTime * Vector3.back);
      }

      if (Input.GetKeyDown(keyAssigned)) {
        laserPrefab.SetBool("KeyDown", true);
        JudgeNote();
        PlayKeySound();
      }

      if (Input.GetKeyUp(keyAssigned)) {
        laserPrefab.SetBool("KeyDown", false);
      }
    }
  }
}
