using UnityEngine;
using UnityEngine.EventSystems;

namespace Select {
  public class SongListItem : MonoBehaviour, IPointerClickHandler {
    public string path;
    public BMS.Model bms;

    public void OnPointerClick(PointerEventData eventData) {
      if (eventData.button == PointerEventData.InputButton.Left) {
        if (eventData.clickCount > 1) {
          SongManager.instance.EnterSong();
        } else {
          SongManager.instance.SelectSong(path, bms);
        }
      }
    }
  }
}
