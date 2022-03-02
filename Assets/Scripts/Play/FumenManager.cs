using UnityEngine;

public class FumenManager : MonoBehaviour {
  public static FumenManager instance;
  public LaneController[] lanes;

  public string filePath;

  // In milliseconds.
  public float inputLatency;
  public float pgreatRange;
  public float greatRange;
  public float goodRange;
  public float badRange;
  public float poorRange;

  public float noteBaseY;
  public float noteSpawnY;
  public float noteDespawnY {
    get {
      return noteBaseY * 2 - noteSpawnY;
    }
  }

  public void Start() {
    instance = this;
    ReadDataFromFile();
  }

  public void ReadDataFromFile() {
    var bms = BMS.Model.Parse(filePath);
  }

  public void StartSong() {
  }

  public static float GetAudioSourceTime() {
    return 0;
  }
}
