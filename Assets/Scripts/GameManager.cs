// REFERENCES
// 
// INTEGRATION
// Quick integration: https://docs.deltadna.com/quick-start-integration/quick-integration-video/
// 
// EVENT TRIGGERS
// Event Triggers: https://docs.deltadna.com/getting-started/tutorials/event-triggered-campaigns-tutorial/
// Setting up Event Trigger Campaigns: https://docs.deltadna.com/reference/engage/campaigns-3/event-triggered-campaigns/
// 
// DASHBOARDS
// Dashboard customization: https://docs.deltadna.com/reference/dashboard/custom-dashboards/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeltaDNA;

public class GameManager : MonoBehaviour {

	public int missionID;
	public string missionName;

    // Start is called before the first frame update
    void Start() {
		DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
		
        // Hook up callback to fire when DDNA SDK has received session config info, including Event Triggered campaigns.
        DDNA.Instance.NotifyOnSessionConfigured(true);

        // Allow multiple game parameter actions callbacks from a single event trigger        
        DDNA.Instance.Settings.MultipleActionsForEventTriggerEnabled = true;

        //Register default handlers for event triggered campaigns. These will be candidates for handling ANY Event-Triggered Campaigns. 
        //Any handlers added to RegisterEvent() calls with the .Add method will be evaluated before these default handlers. 
        DDNA.Instance.Settings.DefaultImageMessageHandler = new ImageMessageHandler(DDNA.Instance, imageMessage => {
                myImageMessageHandler(imageMessage);
        });

        DDNA.Instance.Settings.DefaultGameParameterHandler = new GameParametersHandler(gameParameters => {	
            myGameParameterHandler(gameParameters);
        });

        DDNA.Instance.StartSDK();
	
		if (missionID == 0) {
			missionName = "Demo Mission";
			Debug.Log(missionName);
		}

		if (missionID == 1) {
			missionName = "Demo Mission 2";
			Debug.Log(missionName);
		}

		var missionStartedEvent = new GameEvent("missionStarted")
            .AddParam("isTutorial", false)
			.AddParam("missionDifficulty", "EASY")
			.AddParam("missionID", missionID.ToString())
			.AddParam("missionName", missionName);

        // Record missionCompleted event and wire up handler callbacks
        DDNA.Instance.RecordEvent(missionStartedEvent);
    }

	public void missionStartedButtonClick()
    {
        Debug.Log("Mission Started Button Clicked");

        // Create a missionStarted event object
        var missionStartedEvent = new GameEvent("missionStarted")
            .AddParam("isTutorial", false)
			.AddParam("missionDifficulty", "EASY")
			.AddParam("missionID", missionID.ToString())
			.AddParam("missionName", missionName);

        // Record missionCompleted event and wire up handler callbacks
        DDNA.Instance.RecordEvent(missionStartedEvent).Run();
    }

   private void myGameParameterHandler(Dictionary<string,object> gameParameters)
    {
        // Parameters Received      
        Debug.Log("Received game parameters from event trigger: " + DeltaDNA.MiniJSON.Json.Serialize(gameParameters));
    }

    private void myImageMessageHandler(ImageMessage imageMessage)
    {                       
        // Add a handler for the 'dismiss' action.
        imageMessage.OnDismiss += (ImageMessage.EventArgs obj) => {
            Debug.Log("Image Message dismissed by " + obj.ID);

            // NB : parameters not processed in this example if player dismisses action
        };

        // Add a handler for the 'action' action.
        imageMessage.OnAction += (ImageMessage.EventArgs obj) => {
            Debug.Log("Image Message actioned by " + obj.ID + " with command " + obj.ActionValue);

            // Process parameters on image message if player triggers image message action
            if (imageMessage.Parameters != null) myGameParameterHandler(imageMessage.Parameters);
        };

        // the image message is already cached and prepared so it will show instantly
        imageMessage.Show();
    }
}
