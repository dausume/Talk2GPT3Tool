using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verbalConversationPrompt
{
    // The time the prompt was created
    public float creationTime;

    // The character the conversation is linked to
    public string character;

    // The order the prompt was created in
    public int order;

    // The AudioClip for the prompt
    public AudioClip clip;

    // Constructor for the verbalConversationPrompt class
    public verbalConversationPrompt(float time, string character, int order, AudioClip clip)
    {
        this.creationTime = time;
        this.character = character;
        this.order = order;
        this.clip = clip;
    }
}


public class MicrophoneRecorder : MonoBehaviour
{
    // This is the time from the scene at the instant recording from the microphone starts.
    private float startTime;
    // The maximum recording time, in seconds.
    public float recordingTimeInterval = 30.0f;

    // The AudioClip object that will contain the recorded audio.
    private AudioClip recordingClip;

    // The name of the microphone device to use for recording.
    private string microphoneName;

    // A flag that indicates whether the microphone is currently recording.
    private bool isRecording = false;

    //As soon as this script starts, which should be on scene load, get the microphone name.
    void Start()
    {
        // Get the name of the default microphone device.
        microphoneName = Microphone.devices[0];
    }

    void Update()
    {
        // Start the microphone if it isn't already running
        if (!Microphone.IsRecording(microphoneName))
        {
            // Run the microphone for as long as possible so we can periodically send out AudioClips.
            // Set the audio sampling value to be standard quality with 44100 sample points (decrease for smaller file sizes, lower quality)
            Microphone.Start(microphoneName, true, 999999, 44100);
        }
    }

    void SendAudioToAPI()
    {
        // Check the position of the microphone recording
        int pos = Microphone.GetPosition(null);

        // Create an AudioClip object from the microphone recording
        var clip = AudioClip.Create("Recording", pos, 1, 44100, false);

        // Send the AudioClip to the API...
    }

    void StartRecording()
    {
        // Start recording from the microphone.
        //recordingClip = Microphone.Start(microphoneName, false, maxRecordingTime, 44100);
        isRecording = true;
        startTime = Time.time;
    }

    void StopRecording()
    {
        // Stop the microphone and create the AudioClip object.
        Microphone.End(microphoneName);
        recordingClip = AudioClip.Create("Recording", recordingClip.samples, recordingClip.channels, recordingClip.frequency, false);
        isRecording = false;
    }
}

