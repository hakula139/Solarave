using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour {
  public static SongManager instance;

  protected Transform container;
  public GameObject songListItemPrefab;
  public GameObject folderListItemPrefab;

  private Dictionary<BMS.Difficulty, Color> difficultyColorMap;

  public string songFolderBasePath;
  protected string currentPath;

  private void Awake() {
    instance = this;
  }

  public void Start() {
    container = transform.Find("Viewport/Container");

    difficultyColorMap = new() {
      { BMS.Difficulty.Unknown, new Color(0.5f, 0.5f, 0.5f, 1) },
      { BMS.Difficulty.Beginner, new Color(0.5f, 1, 0.5f, 1) },
      { BMS.Difficulty.Normal, new Color(0.25f, 0.5f, 1, 1) },
      { BMS.Difficulty.Hyper, new Color(1, 0.5f, 0.5f, 1) },
      { BMS.Difficulty.Another, new Color(1, 0.125f, 0.125f, 1) },
      { BMS.Difficulty.Insane, new Color(0.375f, 0.0625f, 0.75f, 1) },
    };

    songFolderBasePath = Path.Combine(Application.streamingAssetsPath, songFolderBasePath);
    ReadSongFolder(songFolderBasePath);
  }

  public void Update() {
    // Right click to return.
    if (Input.GetMouseButtonDown(1)) {
      string parentPath = Directory.GetParent(currentPath).FullName;
      Debug.LogFormat("returning to parent folder, path=<{0}>", parentPath);
      if (parentPath.StartsWith(songFolderBasePath)) {
        instance.ReadSongFolder(parentPath);
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
    FolderListItem folderListItem = folderListItemClone.GetComponent<FolderListItem>();
    folderListItem.path = path;

    TMP_Text titleTMP = folderListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = filename;
  }

  public void SetupSongListItem(string path) {
    GameObject songListItemClone = Instantiate(songListItemPrefab, container);
    SongListItem songListItem = songListItemClone.GetComponent<SongListItem>();
    songListItem.path = path;

    BMS.Model bms = BMS.Model.Parse(path, headerOnly: true);
    TMP_Text titleTMP = songListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
    titleTMP.text = bms.header.title;
    TMP_Text levelTMP = songListItemClone.transform.Find("Level").GetComponent<TMP_Text>();
    levelTMP.text = bms.header.level.ToString();
    levelTMP.color = difficultyColorMap[bms.header.difficulty];
  }

  public void ClearSelectList() {
    foreach (Transform child in container) {
      Destroy(child.gameObject);
    }
  }
}
