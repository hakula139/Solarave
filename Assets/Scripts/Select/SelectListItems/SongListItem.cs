using UnityEngine;
using UnityEngine.EventSystems;

public class SongListItem : MonoBehaviour, IPointerClickHandler {
  public string path;

  public void OnPointerClick(PointerEventData eventData) {
    if (eventData.button == PointerEventData.InputButton.Left) {
      Debug.LogFormat("entering song, path=<{0}>", path);
    }
  }
}
