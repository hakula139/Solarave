using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour {
  public static FpsCounter instance;

  public TMP_Text fpsTMP;

  private readonly Queue<float> fpsHistory = new();
  private static readonly int HistorySize = 1000;
  private float totalFps;
  public int AvgFps => Mathf.FloorToInt(totalFps / HistorySize);

  private static readonly int UpdateInterval = 600;
  private int currentStep;

  private void Awake() {
    instance = this;
  }

  private void Update() {
    float fps = 1f / Time.deltaTime;
    fpsHistory.Enqueue(fps);
    totalFps += fps;
    if (fpsHistory.Count > HistorySize) {
      totalFps -= fpsHistory.Dequeue();
    }
    if (currentStep++ >= UpdateInterval) {
      fpsTMP.text = AvgFps.ToString();
      currentStep -= UpdateInterval;
    }
  }
}
