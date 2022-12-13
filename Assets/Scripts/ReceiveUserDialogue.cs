using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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
                List<TextConversationStatement> data = JsonUtility.FromJson<List<TextConversationStatement>>(jsonData);
                //List<textConversationStatement> data = JsonUtility.FromJson<List<textConversationStatement>>(jsonData);

                if(data.Count != 0){
                    Debug.Log("Data " + data);
                    //Debug.Log("From User?: " + data[0].fromUser + " | Statement: " + data[0].textStatement);

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
        //StopAllCoroutines();



        //Use data to send back to UI backend
        Dialogue_Behavior dialogueBehavior = FindObjectOfType<Dialogue_Behavior>();
        //dialogueBehavior.updateText(data.fromUser, data.textStatement);
        //dialogueBehavior.updateText(data[0].fromUser, data[0].textStatement);
    }

}

/*public class textConversationStatement{
    // The time the prompt was created (gives order to the conversation, smaller numbers occurred earlier)
    public int samplePosition;
    // The character the conversation is linked to.
    public string character;
    // Indicates whether this is from the User talking or GPT-3 responding.
    public bool fromUser;
    //The actual text to be displayed in a textMeshPro Component.
    public string textStatement;
    // Constructor for the textConversationStatement class
    public textConversationStatement(int samplePosition, string character, bool fromUser, string textStatement)
    {
        this.samplePosition = samplePosition;
        this.character = character;
        this.fromUser = fromUser;
        this.textStatement = textStatement;
    }
}*/

public class TextConversationStatement
{
    public string character;
    public bool fromUser;
    public int samplePosition;
    public string textStatement;
}
