using UnityEngine;
using UnityEngine.EventSystems;

namespace Select {
  public class SongListItem : MonoBehaviour, IPointerClickHandler {
    public string path;
    public BMS.Model bms;

    public void OnPointerClick(PointerEventData eventData) {
      if (eventData.button == PointerEventData.InputButton.Left) {
        if (eventData.clickCount > 1) {
          Debug.LogFormat("entering song, path=<{0}>", path);
          SongManager.instance.EnterPlayScene(path);
        } else {
          Debug.LogFormat("selected song, path=<{0}>", path);
          SongManager.instance.SetupSongInfo(bms);
        }
      }
    }
  }
}
