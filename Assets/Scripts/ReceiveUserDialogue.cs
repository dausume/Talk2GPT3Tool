using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ReceiveUserDialogue : MonoBehaviour
{
    // The URL of the API endpoint
    //private string apiUrl = "http://localhost:5000/api/dialogue";
    private string apiUrl = "http://127.0.0.1:5000/api/getDialogText";

    int TimeInSeconds = 15; //time interval for coroutine

    void Start()
    {
        // Starts periodically sending requests.
        StartCoroutine(ListenForRequests());
    }


    //Gets bulk response from the API and then calls the TriggerAction method to send back to Unity UI.
    IEnumerator ListenForRequests()
    {
        // Loop indefinitely
        while (true)
        {
            // Create a new UnityWebRequest to receive the Get from the Flask server
            UnityWebRequest request = UnityWebRequest.Get(apiUrl);

            // Send the request and wait for a response
            request.SendWebRequest();
            while (!request.isDone)
            {
                // Wait for the response
                yield return null;
            }

            // Check if the request was successful
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                // There was an error, log the error message
                Debug.LogError(request.error);
            }
            else
            {
                // The request was successful, get the JSON data from the response
                string jsonData = request.downloadHandler.text;
                Debug.Log("Json Data: " + jsonData);


                // Parse the JSON data using the JsonUtility class
                List<TextConversationStatement> data = JsonConvert.DeserializeObject<List<TextConversationStatement>>(jsonData);

                //Debug.Log("Line 54 - Data: " + data);

                if(data.Count != 0){
                    // Trigger the action
                    TriggerAction(data);
                }
                else{
                    Debug.Log("The fracking list was empty! >:(");
                }

            }

            yield return new WaitForSeconds(TimeInSeconds);
        }
    }


    //Takes in the data from the coroutine and then sends it to the Unity UI via Dialogue_Behavior object.
    void TriggerAction(List<TextConversationStatement> data){
        //TODO Implement functionality to stop coroutine conversations when leaving a certain proximity around character
        //Stops ListenForRequests once one request is passed back to userUI.

        //Use data to send back to UI backend
        Dialogue_Behavior dialogueBehavior = FindObjectOfType<Dialogue_Behavior>();

        foreach(TextConversationStatement item in data){
            Debug.Log("Item Value: " + item.textStatement);
            dialogueBehavior.updateText(item.fromUser, item.textStatement);
        };
    }

}


public class TextConversationStatement
{
    public List<TextConversationStatement> items;
    public string character;
    public bool fromUser;
    public int samplePosition;
    public string textStatement;

}
