using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour {
  public string songFolderBasePath;
  public GameObject selectListArea;
  public GameObject songListItemPrefab;
  public GameObject folderListItemPrefab;

  public void Start() {
    ReadSongFolder();
  }

  public void Update() {
  }

  public void ReadSongFolder() {
    string baseDir = Path.Combine(Application.streamingAssetsPath, songFolderBasePath);
    if (!Directory.Exists(baseDir)) {
      Debug.LogErrorFormat("song folder not found, path=<{0}>", baseDir);
      return;
    }

    string[] extensions = new[] { ".bms", ".bme" };
    foreach (string path in Directory.GetDirectories(baseDir)) {
      SetupFolderListItem(path);
    }
    foreach (string path in Directory.GetFiles(baseDir)) {
      if (extensions.Any(ext => Path.GetExtension(path) == ext)) {
        SetupSongListItem(path);
      }
    }
  }

  public void SetupFolderListItem(string path) {
    string filename = Path.GetFileName(path);
    GameObject folderListItemClone = Instantiate(folderListItemPrefab, selectListArea.transform);
    TMP_Text titleTMP = folderListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;
  }

  public void SetupSongListItem(string path) {
    string filename = Path.GetFileNameWithoutExtension(path);
    GameObject songListItemClone = Instantiate(songListItemPrefab, selectListArea.transform);
    TMP_Text titleTMP = songListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;
    TMP_Text levelTMP = songListItemClone.transform.Find("Level").GetComponent<TMP_Text>();
    int level = 98765;
    levelTMP.text = (level % 1000).ToString();
  }
}
