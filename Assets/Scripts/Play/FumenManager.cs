using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Play {
  public class FumenManager : MonoBehaviour {
    public static FumenManager instance;

    private KeyController[] lanes;

    public TMP_Text titleTMP;
    public TMP_Text timeLeftTMP;
    public Image difficultyFrame;
    public TMP_Text levelTMP;
    public TMP_Text bpmTMP;

    private readonly BMS.Model bms = new();
    public int totalNotes;
    public string fumenPath;

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
      lanes = FindObjectsOfType<KeyController>();
      ReadDataFromFile();
    }

    private void Update() {
      UpdateTimeLeft();
    }

    public void ReadDataFromFile() {
      _ = bms.Parse(fumenPath);
      _ = StartCoroutine(Initialize());
    }

    public IEnumerator Initialize() {
      InitializeUI();
      InitializeFumenScroller();
      yield return StartCoroutine(InitializeKeySounds());

      float startY = 0f;
      foreach (BMS.Measure measure in bms.content.measures) {
        // InitializeBgaByMeasure(measure);
        InitializeNotesByMeasure(measure, startY);
        yield return null;
        startY += measure.length;
      }

      StartPlaying();
    }

    private void InitializeUI() {
      titleTMP.text = bms.header.FullTitle;

      UpdateTimeLeft();

      difficultyFrame.sprite = SpriteAssetHelper.instance.GetDifficultySprite(bms.header.difficulty);
      difficultyFrame.SetNativeSize();

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

    private IEnumerator InitializeKeySounds() {
      string baseDir = Directory.GetParent(Path.Combine(Application.streamingAssetsPath, fumenPath)).FullName;
      foreach ((string relativeWavPath, int wavId) in bms.header.wavPaths.Select((item, i) => (item, i))) {
        if (string.IsNullOrEmpty(relativeWavPath)) {
          continue;
        }

        string wavPath = Path.Combine(baseDir, relativeWavPath);
        if (!File.Exists(wavPath) && !File.Exists(wavPath = wavPath.Replace(".wav", ".ogg"))) {
          Debug.LogWarningFormat("audio file not found, path=<{0}>", wavPath);
        } else if (wavPath.EndsWith(".wav")) {
          yield return StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.WAV));
        } else if (wavPath.EndsWith(".ogg")) {
          yield return StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.OGGVORBIS));
        }
      }
    }

    private void InitializeNotesByMeasure(BMS.Measure measure, float startY) {
      Dictionary<string, KeyController> keyMap = lanes.ToDictionary(l => l.name, l => l.name switch {
        "KeyBgm" => (BgmController)l,
        "KeyScratch" => (ScratchController)l,
        _ => l,
      });

      foreach (BMS.Note note in measure.notes) {
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
      }
    }

    private void StartPlaying() {
      FumenScroller.instance.Enable();
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
