using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Play {
  public class AudioLoader : MonoBehaviour {
    public static AudioLoader instance;

    private AudioSource[] audioSources = new AudioSource[36 * 36];
    public float outputLatency;  // ms

    private void Awake() {
      instance = this;
    }

    private void Start() {
      audioSources = audioSources.Select(_ => {
        GameObject audioPlayer = new("Audio");
        audioPlayer.transform.parent = gameObject.transform;
        return audioPlayer.AddComponent<AudioSource>();
      }).ToArray();
    }

    public IEnumerator Load(string wavPath, int wavId, AudioType type, float volume = 1f) {
      using UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + wavPath, type);
      yield return uwr.SendWebRequest();

      if (uwr.result != UnityWebRequest.Result.Success) {
        Debug.LogWarningFormat("failed to load audio file, error=<{0}>", uwr.error);
      } else {
        audioSources[wavId].clip = DownloadHandlerAudioClip.GetContent(uwr);
        audioSources[wavId].volume = volume;
        yield return null;
      }
    }

    public void Play(int wavId, float time) {
      if (audioSources[wavId].clip != null) {
        audioSources[wavId].PlayScheduled((time + outputLatency + FumenScroller.instance.offset) / 1000f);
        // Debug.LogFormat("play key sound: wavId=<{0}> time=<{1}>", wavId, time + outputLatency);
      }
    }
  }
}
