using System.IO;
using System.Linq;
using UnityEngine;

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
    foreach (string file in Directory.GetFiles(baseDir)) {
      string path = Path.Combine(baseDir, file);
      if (Directory.Exists(path)) {
        // Add folder to select list.
        GameObject listItemClone = Instantiate(folderListItemPrefab, selectListArea.transform);
      } else if (extensions.Any(ext => Path.GetExtension(file) == ext)) {
        // Add song to select list.
        GameObject listItemClone = Instantiate(songListItemPrefab, selectListArea.transform);
      }
    }
  }
}
