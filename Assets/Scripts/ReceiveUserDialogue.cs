using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ReceiveUserDialogue : MonoBehaviour
{
    // The URL of the API endpoint
    //private string apiUrl = "http://localhost:5000/api/dialogue";
    private string apiUrl = "https://64e21ebc-54f8-4538-8f03-d234ac707624.mock.pstmn.io";

    void Start()
    {
        // Start listening for incoming POST requests
        StartCoroutine(ListenForRequests());
    }

    IEnumerator ListenForRequests()
    {
        // Loop indefinitely
        while (true)
        {
            // Create a new UnityWebRequest to receive the POST from the Flask server
            UnityWebRequest request = UnityWebRequest.Post(apiUrl, "");

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

                // Parse the JSON data using the JsonUtility class
                textConversationStatement data = JsonUtility.FromJson<textConversationStatement>(jsonData);

                // Trigger the action
                TriggerAction(data);
            }
        }
    }

    void TriggerAction(textConversationStatement data){
        //Use data to send back to UI backend
        Dialogue_Behavior dialogueBehavior = FindObjectOfType<Dialogue_Behavior>();
        dialogueBehavior.updateText(data.fromUser, data.textStatement);
    }

}

public class textConversationStatement{
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
}