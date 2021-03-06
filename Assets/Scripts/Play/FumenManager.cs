using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Play {
  public class FumenManager : MonoBehaviour {
    public static FumenManager instance;

    public KeyController[] lanes;

    public TMP_Text titleTMP;
    public TMP_Text timeLeftTMP;
    public Image difficultyFrame;
    public TMP_Text levelTMP;
    public TMP_Text bpmTMP;

    private readonly BMS.Model bms = new();
    public int totalNotes;
    public string fumenPath;
    public bool isAutoMode;
    public float startDelay;    // ms
    public float inputLatency;  // ms

    public float pgreatRange;   // ms
    public float greatRange;    // ms
    public float goodRange;     // ms
    public float badRange;      // ms
    public float poorRange;     // ms

    public float pgreatGauge;
    public float greatGauge;
    public float goodGauge;
    public float badGauge;
    public float poorGauge;
    public float missGauge;

    private void Awake() {
      instance = this;
      fumenPath = Select.SongManager.instance.currentFumenPath;
      isAutoMode = Select.SongManager.instance.isAutoMode;
    }

    private void Start() {
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
      InitializeGameManager();
      InitializeKeySounds();

      float startY = 0f;
      foreach (BMS.Measure measure in bms.content.measures) {
        // InitializeBgaByMeasure(measure);
        InitializeNotesByMeasure(measure, startY);
        yield return null;
        startY += measure.length;
        FumenScroller.instance.SetupSeparator(startY);
      }

      InitializeJudgeRange();
      InitializeGrooveGauge();

      Invoke(nameof(StartPlaying), startDelay / 1000f);
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
      FumenScroller.instance.hiSpeed = Select.ConfigManager.instance.hiSpeed * 150f / bms.header.bpm;  // fix hi-speed
    }

    private void InitializeGameManager() {
      switch (Select.ConfigManager.instance.gaugeMode) {
        case Select.GaugeMode.AEASY:
          GameManager.instance.minGauge = 2f;
          GameManager.instance.UpdateGauge(20f);
          GameManager.instance.clearGauge = 60f;
          break;
        case Select.GaugeMode.EASY:
        case Select.GaugeMode.NORMAL:
        default:
          GameManager.instance.minGauge = 2f;
          GameManager.instance.UpdateGauge(20f);
          GameManager.instance.clearGauge = 80f;
          break;
      }
    }

    private void InitializeJudgeRange() {
      switch (bms.header.rank) {
        case BMS.JudgeRank.Easy:
          pgreatRange = 21f;
          greatRange = 60f;
          goodRange = 120f;
          badRange = 200f;
          poorRange = 1000f;
          break;
        case BMS.JudgeRank.Hard:
          pgreatRange = 15f;
          greatRange = 30f;
          goodRange = 60f;
          badRange = 200f;
          poorRange = 1000f;
          break;
        case BMS.JudgeRank.VeryHard:
          pgreatRange = 8f;
          greatRange = 24f;
          goodRange = 40f;
          badRange = 200f;
          poorRange = 1000f;
          break;
        case BMS.JudgeRank.Normal:
        default:
          pgreatRange = 18f;
          greatRange = 40f;
          goodRange = 100f;
          badRange = 200f;
          poorRange = 1000f;
          break;
      }
    }

    private void InitializeGrooveGauge() {
      switch (Select.ConfigManager.instance.gaugeMode) {
        case Select.GaugeMode.AEASY:
        case Select.GaugeMode.EASY:
          pgreatGauge = bms.header.total / totalNotes * 1.2f;
          greatGauge = pgreatGauge;
          goodGauge = pgreatGauge / 2f;
          badGauge = -4f;
          poorGauge = -2f;
          missGauge = -6f;
          break;
        case Select.GaugeMode.NORMAL:
        default:
          pgreatGauge = bms.header.total / totalNotes;
          greatGauge = pgreatGauge;
          goodGauge = pgreatGauge / 2f;
          badGauge = -3.2f;
          poorGauge = -1.6f;
          missGauge = -4.8f;
          break;
      }
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
          _ = StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.WAV, bms.header.volume));
        } else if (wavPath.EndsWith(".ogg")) {
          _ = StartCoroutine(AudioLoader.instance.Load(wavPath, wavId, AudioType.OGGVORBIS, bms.header.volume));
        }
      }
    }

    private void InitializeNotesByMeasure(BMS.Measure measure, float startY) {
      foreach (BMS.Note note in measure.notes) {
        KeyController lane = note.channelId switch {
          BMS.Channel.Bgm => (BgmController)lanes[0],
          BMS.Channel.Key1 => lanes[1],
          BMS.Channel.Key2 => lanes[2],
          BMS.Channel.Key3 => lanes[3],
          BMS.Channel.Key4 => lanes[4],
          BMS.Channel.Key5 => lanes[5],
          BMS.Channel.Key6 => lanes[6],
          BMS.Channel.Key7 => lanes[7],
          BMS.Channel.Scratch => (ScratchController)lanes[8],
          _ => null,
        };
        if (lane != null) {
          FumenScroller.instance.lastNoteTime = lane.SetupNote(startY, measure.length, note) + 3000f;
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
