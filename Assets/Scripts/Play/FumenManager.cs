using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FumenManager : MonoBehaviour {
  public static FumenManager instance;
  public AudioLoader audioLoader;
  public FumenScroller scroller;
  private BMS.Model bms;

  private Dictionary<string, KeyController> keyMap;
  private Dictionary<BMS.Channel, string> keyNameMap;

  public string filePath;

  public float startDelay;    // ms
  public float inputLatency;  // ms
  public float pgreatRange;   // ms
  public float greatRange;    // ms
  public float goodRange;     // ms
  public float badRange;      // ms
  public float poorRange;     // ms

  public void Start() {
    instance = this;
    keyMap = FindObjectsOfType<KeyController>().ToDictionary(l => l.name, l => l);
    keyNameMap = new() {
      { BMS.Channel.Bgm, "KeyBgm" },
      { BMS.Channel.Scratch, "KeyScratch" },
      { BMS.Channel.Key1, "Key1" },
      { BMS.Channel.Key2, "Key2" },
      { BMS.Channel.Key3, "Key3" },
      { BMS.Channel.Key4, "Key4" },
      { BMS.Channel.Key5, "Key5" },
      { BMS.Channel.Key6, "Key6" },
      { BMS.Channel.Key7, "Key7" },
    };
    ReadDataFromFile();
  }

  public void ReadDataFromFile() {
    bms = BMS.Model.Parse(filePath);
    Initialize();
  }

  public void Initialize() {
    string baseDir = Directory.GetParent(Path.Combine(Application.streamingAssetsPath, filePath)).FullName;

    // Initialize BPM.
    scroller.bpm = bms.header.bpm;

    // Initialize key sounds.
    foreach ((string relativeWavPath, int wavId) in bms.header.wavPaths.Select((item, i) => (item, i))) {
      if (string.IsNullOrEmpty(relativeWavPath)) {
        continue;
      }

      string wavPath = Path.Combine(baseDir, relativeWavPath);
      if (!File.Exists(wavPath) && !File.Exists(wavPath = wavPath.Replace(".wav", ".ogg"))) {
        Debug.LogWarningFormat("audio file not found, path=<{0}>", wavPath);
      } else if (wavPath.EndsWith(".wav")) {
        _ = StartCoroutine(audioLoader.Load(wavPath, wavId, AudioType.WAV));
      } else if (wavPath.EndsWith(".ogg")) {
        _ = StartCoroutine(audioLoader.Load(wavPath, wavId, AudioType.OGGVORBIS));
      }
    }

    _ = bms.content.measures.Aggregate(0f, (startY, measure) => {
      measure.bgas.ForEach(bga => {
        // TODO: Initialize BGA.
      });
      measure.notes.ForEach(note => {
        // Initialize notes.
        keyMap[keyNameMap[note.channelId]].SetupNote(startY, measure.length, note);
      });
      return startY + measure.length;
    });

    Invoke(nameof(StartPlaying), startDelay / 1000f);
  }

  public void StartPlaying() {
    scroller.isEnabled = true;
  }
}
