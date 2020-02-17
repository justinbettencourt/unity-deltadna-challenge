using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeltaDNA;

public class GameManager : MonoBehaviour {

	public int missionID;
	public string missionName;

    // Start is called before the first frame update
    void Start() {
		// Configure SDK
		DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
		DDNA.Instance.StartSDK();

		if (missionID == 0) {
			missionName = "Demo Mission";
			Debug.Log(missionName);
		}

		GameEvent missionStartedEvent = new GameEvent ("missionStarted")
			.AddParam("isTutorial", false)
			.AddParam("missionDifficulty", "EASY")
			.AddParam("missionID", missionID.ToString())
			.AddParam("missionName", missionName);

		DDNA.Instance.RecordEvent(missionStartedEvent);
    }

    // Update is called once per frame
    void Update() {

    }
}
