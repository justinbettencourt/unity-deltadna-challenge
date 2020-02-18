using UnityEngine;
using UnityEngine.SceneManagement;
using DeltaDNA;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;
    public bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;

	public GameObject GameManagerObj;
	public int missionID;
	public string missionName;

	void Start() {
		GameManagerObj = GameObject.Find("GameManager");
		missionID = GameManagerObj.GetComponent<GameManager>().missionID;
		missionName = GameManagerObj.GetComponent<GameManager>().missionName;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    void Update ()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
			EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
            
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
				// Before reloading the scene, capture information for DDNA.
				DDNA_missionFailedEvent();
				if (missionID == 0) {
					SceneManager.LoadScene (0);
				}
                
				if (missionID == 1) {
					SceneManager.LoadScene (1);
				}

            }
            else
            {
				DDNA_missionCompletedEvent();
				if (missionID == 0) {
					SceneManager.LoadScene (1);
				}
                
				if (missionID == 1) {
					SceneManager.LoadScene (1);
				}
            }
        }
    }

	void DDNA_missionFailedEvent () {
		GameEvent missionFailedEvent = new GameEvent ("missionFailed")
			.AddParam("isTutorial", false)
			.AddParam("missionID", missionID.ToString())
			.AddParam("missionName", missionName)
			.AddParam("terminationReason", "Enemy Detection");

		DDNA.Instance.RecordEvent(missionFailedEvent);	
	}

	void DDNA_missionCompletedEvent () {
		GameEvent missionCompletedEvent = new GameEvent ("missionCompleted")
			.AddParam("isTutorial", false)
			.AddParam("missionID", missionID.ToString())
			.AddParam("missionName", missionName);

		DDNA.Instance.RecordEvent(missionCompletedEvent);
		
	}
}
