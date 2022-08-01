#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour {

    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;

    [SerializeField] private CanvasGroup exitBackgroundImageCanvasGroup;
    private bool m_IsPlayerAtExit;
    private float m_Timer;

    [SerializeField] private CanvasGroup caughtBackgroundImageCanvasGroup;
    private bool m_IsPlayerCaught;

    public void CaughtPlayer() {
        m_IsPlayerCaught = true;
    }


    private void OnTriggerEnter(Collider other) {
        if (player == other.gameObject)
            m_IsPlayerAtExit = true;
    }

    private void Update() {
        if (m_IsPlayerAtExit)
            EndLevel(exitBackgroundImageCanvasGroup, false);
        else if (m_IsPlayerCaught)
            EndLevel(caughtBackgroundImageCanvasGroup, true);

    }

    private void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart) {
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;
        if (m_Timer > fadeDuration + displayImageDuration) {
            if (doRestart) {
                SceneManager.LoadScene(0);
            } else {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

    }
}
