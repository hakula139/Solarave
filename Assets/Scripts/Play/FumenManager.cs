using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Play {
  public class FumenManager : MonoBehaviour {
    public static FumenManager instance;

    public TMP_Text titleTMP;
    public TMP_Text bpmTMP;

    private BMS.Model bms;
    private int totalNotes;

    private Dictionary<string, KeyController> keyMap;
    private Dictionary<BMS.Channel, string> keyNameMap;

    public string fumenPath;

    public float startDelay;    // ms
    public float inputLatency;  // ms
    public float pgreatRange;   // ms
    public float greatRange;    // ms
    public float goodRange;     // ms
    public float badRange;      // ms
    public float poorRange;     // ms

    private void Awake() {
      instance = this;
      fumenPath = Select.SongManager.instance.currentFumenPath;
    }

    public void Start() {
      keyMap = FindObjectsOfType<KeyController>().ToDictionary(l => l.name, l => l.name switch {
        "KeyBgm" => (BgmController)l,
        "KeyScratch" => (ScratchController)l,
        _ => l,
      });
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
      bms = BMS.Model.Parse(fumenPath);
      Initialize();
    }

    public void Initialize() {
      InitializeUI();
      InitializeFumenScroller();
      InitializeKeySounds();

      _ = bms.content.measures.Aggregate(0f, (startY, measure) => {
        InitializeBgaByMeasure(measure);
        InitializeNotesByMeasure(measure, startY);
        return startY + measure.length;
      });

      Invoke(nameof(StartPlaying), startDelay / 1000f);
    }

    private void InitializeUI() {
      titleTMP.text = bms.header.FullTitle;
      bpmTMP.text = $"{bms.header.bpm:000}";
    }

    private void InitializeFumenScroller() {
      FumenScroller.instance.bpm = bms.header.bpm;
    }

    private void InitializeKeySounds() {
      string baseDir = Directory.GetParent(Path.Combine(Application.streamingAssetsPath, fumenPath)).FullName;
      foreach ((string relativeWavPath, int wavId) in bms.header.wavPaths.Select((item, i) => (item, i))) {
        if (string.IsNullOrEmpty(relativeWavPath)) {
          continue;
        }

        string wavPath = Path.Combine(baseDir, relativeWavPath);
        if (!File.Exists(wavPath) && !File.Exists(wavPath = wavPath.Replace(".wav", ".ogg"))) {
          Debug.LogWarningFormat("audio file not found, path=<{0}>", wavPath);
        } else if (wavPath.EndsWith(".wav")) {
          _ = StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.WAV));
        } else if (wavPath.EndsWith(".ogg")) {
          _ = StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.OGGVORBIS));
        }
      }
    }

    private void InitializeBgaByMeasure(BMS.Measure measure) {
      measure.bgas.ForEach(bga => {
        // TODO: Initialize BGA.
      });
    }

    private void InitializeNotesByMeasure(BMS.Measure measure, float startY) {
      measure.notes.ForEach(note => {
        try {
          keyMap[keyNameMap[note.channelId]].SetupNote(startY, measure.length, note);
        } catch (Exception e) {
          Debug.LogErrorFormat("failed to setup note, channelId=<{0}> exception=<{1}>", note.channelId, e.ToString());
        }
        if (note.channelId >= BMS.Channel.Key1 && note.channelId <= BMS.Channel.Key7) {  // scratch included
          totalNotes++;
        }
      });
    }

    public void StartPlaying() {
      FumenScroller.instance.offset = (float)AudioSettings.dspTime * 1000f;
      FumenScroller.instance.isEnabled = true;
    }
  }
}
