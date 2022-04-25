using UnityEngine;
namespace Play {
  public class ScratchController : KeyController {
    public KeyCode key2Assigned;

    private void Start() {
      sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
      SetupBombPool();
    }

    private void Update() {
      if (Input.GetKey(keyAssigned)) {
        sr.transform.Rotate(360f * Time.deltaTime * Vector3.forward);
      } else if (FumenScroller.instance.isEnabled || Input.GetKey(key2Assigned)) {
        sr.transform.Rotate(360f * Time.deltaTime * Vector3.back);
      }

      if (Input.GetKeyDown(keyAssigned) || Input.GetKeyDown(key2Assigned)) {
        laserPrefab.SetBool("KeyDown", true);
        JudgeNote();
        PlayKeySound();
      }

      if (Input.GetKeyUp(keyAssigned) || Input.GetKeyUp(key2Assigned)) {
        laserPrefab.SetBool("KeyDown", false);
      }
    }
  }
}
