using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {
  public static SceneTransitionManager instance;

  public Animator transition;
  public const float transitionDuration = 0.5f;

  private void Awake() {
    instance = this;
  }

  private void Start() {
    TransitionIn();
  }

  public void EnterScene(string sceneName) {
    TransitionOut();
    _ = StartCoroutine(LoadScene(sceneName));
  }

  private void TransitionIn() {
    transition.SetTrigger("In");
    Invoke(nameof(DisableEffectLayer), transitionDuration);
  }

  private void TransitionOut() {
    EnableEffectLayer();
    transition.SetTrigger("Out");
  }

  private IEnumerator LoadScene(string sceneName) {
    yield return new WaitForSeconds(transitionDuration);
    SceneManager.LoadScene(sceneName);
  }

  private void EnableEffectLayer() {
    transition.gameObject.SetActive(true);
  }

  private void DisableEffectLayer() {
    transition.gameObject.SetActive(false);
  }
}
