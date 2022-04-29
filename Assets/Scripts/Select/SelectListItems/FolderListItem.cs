using UnityEngine;
using UnityEngine.EventSystems;

namespace Select {
  public class FolderListItem : MonoBehaviour, IPointerClickHandler {
    public string path;

    public void OnPointerClick(PointerEventData eventData) {
      if (eventData.button == PointerEventData.InputButton.Left) {
        SongManager.instance.EnterSongFolder(path);
      }
    }
  }
}
