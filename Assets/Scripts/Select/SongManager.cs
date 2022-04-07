using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour {
  public static SongManager instance;

  public string songFolderBasePath;
  public GameObject selectListArea;
  public GameObject songListItemPrefab;
  public GameObject folderListItemPrefab;

  private void Awake() {
    instance = this;
  }

  public void Start() {
    string baseDir = Path.Combine(Application.streamingAssetsPath, songFolderBasePath);
    ReadSongFolder(baseDir);
  }

  public void Update() {
  }

  public void ReadSongFolder(string path) {
    if (!Directory.Exists(path)) {
      Debug.LogErrorFormat("song folder not found, path=<{0}>", path);
      return;
    }

    ClearSelectList();
    string[] extensions = new[] { ".bms", ".bme" };
    foreach (string childPath in Directory.GetDirectories(path)) {
      SetupFolderListItem(childPath);
    }
    foreach (string childPath in Directory.GetFiles(path)) {
      if (extensions.Any(ext => Path.GetExtension(childPath) == ext)) {
        SetupSongListItem(childPath);
      }
    }
  }

  public void SetupFolderListItem(string path) {
    string filename = Path.GetFileName(path);
    GameObject folderListItemClone = Instantiate(folderListItemPrefab, selectListArea.transform);
    TMP_Text titleTMP = folderListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;

    FolderListItem folderListItem = folderListItemClone.GetComponent<FolderListItem>();
    folderListItem.path = path;
  }

  public void SetupSongListItem(string path) {
    string filename = Path.GetFileNameWithoutExtension(path);
    GameObject songListItemClone = Instantiate(songListItemPrefab, selectListArea.transform);
    TMP_Text titleTMP = songListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;
    TMP_Text levelTMP = songListItemClone.transform.Find("Level").GetComponent<TMP_Text>();
    int level = 98765;  // FIXME: remove mock data
    levelTMP.text = (level % 1000).ToString();

    SongListItem songListItem = songListItemClone.GetComponent<SongListItem>();
    songListItem.path = path;
  }

  public void ClearSelectList() {
    foreach (Transform child in selectListArea.transform) {
      Destroy(child.gameObject);
    }
  }
}
