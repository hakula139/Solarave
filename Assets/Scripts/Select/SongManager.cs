using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour {
  public static SongManager instance;

  protected Transform container;
  public GameObject songListItemPrefab;
  public GameObject folderListItemPrefab;

  public string songFolderBasePath;
  protected string currentPath;

  private void Awake() {
    instance = this;
  }

  public void Start() {
    container = transform.Find("Viewport/Container");

    songFolderBasePath = Path.Combine(Application.streamingAssetsPath, songFolderBasePath);
    ReadSongFolder(songFolderBasePath);
  }

  public void Update() {
    // Right click to return.
    if (Input.GetMouseButtonDown(1)) {
      string parentPath = Directory.GetParent(currentPath).FullName;
      Debug.LogFormat("returning to parent folder, path=<{0}>", parentPath);
      if (parentPath.StartsWith(songFolderBasePath)) {
        SongManager.instance.ReadSongFolder(parentPath);
      }
    }
  }

  public void ReadSongFolder(string path) {
    if (!Directory.Exists(path)) {
      Debug.LogErrorFormat("song folder not found, path=<{0}>", path);
      return;
    }

    ClearSelectList();
    currentPath = path;

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
    GameObject folderListItemClone = Instantiate(folderListItemPrefab, container);
    TMP_Text titleTMP = folderListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;

    FolderListItem folderListItem = folderListItemClone.GetComponent<FolderListItem>();
    folderListItem.path = path;
  }

  public void SetupSongListItem(string path) {
    string filename = Path.GetFileNameWithoutExtension(path);
    GameObject songListItemClone = Instantiate(songListItemPrefab, container);
    TMP_Text titleTMP = songListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;
    TMP_Text levelTMP = songListItemClone.transform.Find("Level").GetComponent<TMP_Text>();
    int level = 98765;  // FIXME: remove mock data
    levelTMP.text = (level % 1000).ToString();

    SongListItem songListItem = songListItemClone.GetComponent<SongListItem>();
    songListItem.path = path;
  }

  public void ClearSelectList() {
    foreach (Transform child in container) {
      Destroy(child.gameObject);
    }
  }
}
