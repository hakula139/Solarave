using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class AudioLoader : MonoBehaviour {
  private AudioSource[] audioSources = new AudioSource[36 * 36];
  public float delay;  // ms

  public void Start() {
    audioSources = audioSources.Select(_ => {
      GameObject audioPlayer = new("Audio");
      audioPlayer.transform.parent = gameObject.transform;
      return audioPlayer.AddComponent<AudioSource>();
    }).ToArray();
  }

  public IEnumerator Load(string wavPath, int wavId, AudioType type) {
    using UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + wavPath, type);
    yield return uwr.SendWebRequest();

    if (uwr.result != UnityWebRequest.Result.Success) {
      Debug.LogWarningFormat("failed to load audio file, error=<{0}>", uwr.error);
    } else {
      audioSources[wavId].clip = DownloadHandlerAudioClip.GetContent(uwr);
      yield return null;
    }
  }

  public void Play(int wavId, float time) {
    if (audioSources[wavId].clip is not null) {
      audioSources[wavId].PlayScheduled((time + delay) / 1000f);
      Debug.LogFormat("play key sound: wavId=<{0}> time=<{1}>", wavId, time + delay);
    }
  }
}
