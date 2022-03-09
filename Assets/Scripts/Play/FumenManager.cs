using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FumenManager : MonoBehaviour {
  public static FumenManager instance;
  public FumenScroller scroller;
  private BMS.Model bms;

  private Dictionary<string, KeyController> keyMap;
  private Dictionary<BMS.Channel, string> keyNameMap;

  public string filePath;

  public float startDelay;  // ms
  public float inputLatency;  // ms
  public float pgreatRange;  // ms
  public float greatRange;  // ms
  public float goodRange;  // ms
  public float badRange;  // ms
  public float poorRange;  // ms

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
    scroller.bpm = bms.header.bpm;
    _ = bms.content.measures.Aggregate(0f, (startY, measure) => {
      measure.bgas.ForEach(bga => {
        // TODO: Initialize BGA.
      });
      measure.notes.ForEach(note => {
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
