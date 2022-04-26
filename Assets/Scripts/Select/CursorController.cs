using UnityEngine;

namespace Select {
  public class CursorController : MonoBehaviour {
    public static CursorController instance;

    private void Awake() {
      instance = this;
    }

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
