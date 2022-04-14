using UnityEngine;
using UnityEngine.EventSystems;

namespace Select {
  public class FolderListItem : MonoBehaviour, IPointerClickHandler {
    public string path;

    public void OnPointerClick(PointerEventData eventData) {
      if (eventData.button == PointerEventData.InputButton.Left) {
        Debug.LogFormat("entering song folder, path=<{0}>", path);
        SongManager.instance.openSoundEffect.Play();
        SongManager.instance.ReadSongFolder(path);
      }
    }
  }
}
