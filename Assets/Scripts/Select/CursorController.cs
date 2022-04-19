using UnityEngine;

namespace Select {
  public class CursorController : MonoBehaviour {
    private void Start() {
      Cursor.visible = false;
    }

    private void Update() {
      Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePosition.z += Camera.main.nearClipPlane;
      transform.position = mousePosition;
    }
  }
}
