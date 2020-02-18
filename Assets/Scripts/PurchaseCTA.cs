using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DeltaDNA {

public class PurchaseCTA : MonoBehaviour
{
    public void OnImageMessageBtn_Clicked() {
        var customParams = new Params()
            .AddParam("userLevel", 4)
            .AddParam("experience", 1000)
            .AddParam("missionName", "Disco Volante");

        DDNA.Instance.EngageFactory.RequestImageMessage("unityImageMessage", customParams, (imageMessage) => {

            // Check we got an engagement with a valid image message.
            if (imageMessage != null) {
                Debug.Log("Engage returned a valid image message.");

                // This example will show the image as soon as the background
                // and button images have been downloaded.
                imageMessage.OnDidReceiveResources += () => {
                    Debug.Log("Image Message loaded resources.");
                    imageMessage.Show();
                };

                // Add a handler for the 'dismiss' action.
                imageMessage.OnDismiss += (ImageMessage.EventArgs obj) => {
                    Debug.Log("Image Message dismissed by " + obj.ID);
                };

                // Add a handler for the 'action' action.
                imageMessage.OnAction += (ImageMessage.EventArgs obj) => {
                    Debug.Log("Image Message actioned by " + obj.ID + " with command " + obj.ActionValue);
                };

                // Download the image message resources.
                imageMessage.FetchResources();
            } else {
                Debug.Log("Engage didn't return an image message.");
            }
        });
    }
}
}