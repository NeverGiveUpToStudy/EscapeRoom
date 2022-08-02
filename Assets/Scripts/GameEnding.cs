#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour {

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayImageDuration = 1f;
    [SerializeField] private GameObject player;

    [SerializeField] private CanvasGroup exitBackgroundImageCanvasGroup;
    private bool m_IsPlayerAtExit;
    private float m_Timer;

    [SerializeField] private CanvasGroup caughtBackgroundImageCanvasGroup;
    private bool m_IsPlayerCaught;


    [SerializeField] private AudioSource exitAudio;
    [SerializeField] private AudioSource caughtAudio;
    private bool m_HasAudioPlayed;

    public void CaughtPlayer() {
        m_IsPlayerCaught = true;
    }


    private void OnTriggerEnter(Collider other) {
        if (player == other.gameObject)
            m_IsPlayerAtExit = true;
    }

    private void Update() {
        if (m_IsPlayerAtExit)
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        else if (m_IsPlayerCaught)
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);

    }

    private void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource) {
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

        if (!m_HasAudioPlayed) {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

    }
}
