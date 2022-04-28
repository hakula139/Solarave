using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Select {
  public class SongManager : MonoBehaviour {
    public static SongManager instance;

    public Transform container;
    public GameObject songListItemPrefab;
    public GameObject folderListItemPrefab;

    public TMP_Text genreTMP;
    public TMP_Text titleTMP;
    public TMP_Text subtitleTMP;
    public TMP_Text artistTMP;

    public string songFolderBasePath;
    public static string CurrentPath;
    public static string CurrentFumenPath;

    private void Awake() {
      instance = this;
    }

    private void Start() {
      songFolderBasePath = Path.Combine(Application.streamingAssetsPath, songFolderBasePath);
      if (string.IsNullOrEmpty(CurrentPath)) {
        CurrentPath = songFolderBasePath;
      }
      ReadSongFolder(CurrentPath);
    }

    private void Update() {
      // Right click to return.
      if (Input.GetMouseButtonDown(1)) {
        string parentPath = Directory.GetParent(CurrentPath).FullName;
        // Debug.LogFormat("returning to parent folder, path=<{0}>", parentPath);
        if (parentPath.StartsWith(songFolderBasePath)) {
          SoundEffectsManager.instance.closeSoundEffect.Play();
          ReadSongFolder(parentPath);
        }
      }
    }

    public void ReadSongFolder(string path) {
      if (!Directory.Exists(path)) {
        Debug.LogErrorFormat("song folder not found, path=<{0}>", path);
      }

      ClearSelectList();
      ClearSongInfo();
      CurrentPath = path;

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
      songListItem.bms = new();
      _ = songListItem.bms.Parse(path, headerOnly: true);

      BMS.HeaderSection header = songListItem.bms.header;
      TMP_Text titleTMP = songListItemClone.transform.Find("Title").GetComponent<TMP_Text>();
      titleTMP.text = header.FullTitle;
      TMP_Text levelTMP = songListItemClone.transform.Find("Level").GetComponent<TMP_Text>();
      levelTMP.text = header.level.ToString();
      levelTMP.color = SpriteAssetHelper.instance.GetDifficultyColor(header.difficulty);
    }

    public void ClearSelectList() {
      foreach (Transform child in container) {
        Destroy(child.gameObject);
      }
    }

    public void SetupSongInfo(BMS.Model bms) {
      BMS.HeaderSection header = bms.header;
      genreTMP.text = header.genre;
      titleTMP.text = header.title;
      subtitleTMP.text = header.subtitle;
      artistTMP.text = header.FullArtist;
    }

    public void ClearSongInfo() {
      genreTMP.text = "";
      titleTMP.text = "";
      subtitleTMP.text = "";
      artistTMP.text = "";
    }

    public void EnterPlayScene(string path) {
      CurrentFumenPath = path;
      SceneTransitionManager.instance.EnterScene("Play");
    }
  }
}
