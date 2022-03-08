using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FumenManager : MonoBehaviour {
  public static FumenManager instance;
  public BeatScroller bs;
  private BMS.Model bms;

  private Dictionary<string, KeyController> keyMap;
  private Dictionary<BMS.Channel, string> keyNameMap;

  public string filePath;

  // In milliseconds.
  public float startDelay;
  public float inputLatency;
  public float pgreatRange;
  public float greatRange;
  public float goodRange;
  public float badRange;
  public float poorRange;

  public float measureBaseLength = 1f;
  public float noteBaseY;
  public float noteSpawnY;
  public float NoteDespawnY => (noteBaseY * 2) - noteSpawnY;

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
    _ = bms.content.measures.Aggregate(0f, (startY, measure) => {
      measure.bgas.ForEach(bga => {
        // TODO: Initialize BGA.
      });
      measure.notes.ForEach(note => {
        keyMap[keyNameMap[note.channelId]].SetupNote(startY, measure.length, note);
      });
      return startY + (measure.length * measureBaseLength);
    });

    Invoke(nameof(StartPlaying), startDelay);
  }

  public void StartPlaying() {
    bs.bpm = bms.header.bpm;
    bs.isEnabled = true;
  }
}
