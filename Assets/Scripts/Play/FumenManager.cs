using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Play {
  public class FumenManager : MonoBehaviour {
    public static FumenManager instance;

    public TMP_Text titleTMP;
    public TMP_Text timeLeftTMP;
    public Image difficultyFrame;
    public TMP_Text levelTMP;
    public TMP_Text bpmTMP;

    private BMS.Model bms;
    private int totalNotes;
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

    private void Start() {
      ReadDataFromFile();
    }

    private void Update() {
      UpdateTimeLeft();
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

      UpdateTimeLeft();

      difficultyFrame.sprite = SpriteAssetHelper.instance.GetDifficultySprite(bms.header.difficulty);

      if (bms.header.difficulty != BMS.Difficulty.Unknown) {
        levelTMP.text = bms.header.level.ToString();
        levelTMP.color = SpriteAssetHelper.instance.GetDifficultyColor(bms.header.difficulty);
      }

      bpmTMP.text = bms.header.bpm.ToString();
    }

    private void InitializeFumenScroller() {
      FumenScroller.instance.bpm = bms.header.bpm;
      FumenScroller.instance.hiSpeed *= 150f / bms.header.bpm;  // fix hi-speed
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
      Dictionary<string, KeyController> keyMap = FindObjectsOfType<KeyController>().ToDictionary(l => l.name, l => l.name switch {
        "KeyBgm" => (BgmController)l,
        "KeyScratch" => (ScratchController)l,
        _ => l,
      });

      measure.notes.ForEach(note => {
        KeyController lane = note.channelId switch {
          BMS.Channel.Bgm => keyMap["KeyBgm"],
          BMS.Channel.Scratch => keyMap["KeyScratch"],
          BMS.Channel.Key1 => keyMap["Key1"],
          BMS.Channel.Key2 => keyMap["Key2"],
          BMS.Channel.Key3 => keyMap["Key3"],
          BMS.Channel.Key4 => keyMap["Key4"],
          BMS.Channel.Key5 => keyMap["Key5"],
          BMS.Channel.Key6 => keyMap["Key6"],
          BMS.Channel.Key7 => keyMap["Key7"],
          _ => null,
        };
        if (lane != null) {
          FumenScroller.instance.lastNoteTime = lane.SetupNote(startY, measure.length, note);
        } else {
          Debug.LogErrorFormat("failed to setup note, channelId=<{0}>", note.channelId);
        }

        if (note.channelId is >= BMS.Channel.Key1 and <= BMS.Channel.Key7) {  // scratch included
          totalNotes++;
        }
      });
    }

    private void StartPlaying() {
      FumenScroller.instance.offset = (float)AudioSettings.dspTime * 1000f;
      FumenScroller.instance.isEnabled = true;
    }

    public void UpdateTimeLeft() {
      float timeLeft = FumenScroller.instance.TimeLeft;
      if (timeLeft >= 0) {
        TimeSpan t = TimeSpan.FromMilliseconds(timeLeft);
        timeLeftTMP.text = $"{(int)t.TotalMinutes} : {t.Seconds:D2}";
      }
    }
  }
}
