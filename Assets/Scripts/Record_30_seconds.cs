using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verbalConversationPrompt
{
    // The time the prompt was created
    public int samplePosition;

    // The character the conversation is linked to
    public string character;

    // The AudioClip for the prompt
    public AudioClip clip;

    // Constructor for the verbalConversationPrompt class
    public verbalConversationPrompt(int samplePosition, string character, AudioClip clip)
    {
        this.samplePosition = samplePosition;
        this.character = character;
        this.clip = clip;
    }
}

public class MicrophoneRecorder : MonoBehaviour
{
    // This is the time from the scene at the instant recording from the microphone starts.
    private float startTime;

    //A dictionary linking the sample number to the conversation prompt.
    //Retains at most 5 minutes worth of responses being awaited from whisper (20 responses max with 4 responses per minute, 5 minutes worth)
    private Dictionary<int, verbalConversationPrompt> verbalPromptsSentToWhisper = new Dictionary<int, verbalConversationPrompt>(20);

    // The maximum recording time, in seconds.
    public float recordingTimeInterval = 30.0f;

    // The AudioClip object that will contain the recorded audio.
    private AudioClip recordingClip;

    // The name of the microphone device to use for recording.
    private string microphoneName;

    // A flag that indicates whether the microphone is currently recording.
    private bool isRecording = false;

    // Indicates what the previous recorded sample position was, so that we can accurately gauge the next set of samples that should
    // have a AudioClip made from (lastSamplePos, newSamplePos)
    private int lastSamplePos = 0;

    const int clipAudioChannels = 1;
    const int clipAudioFrequencies = 44100;


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
        var pos = lastSamplePos - Microphone.GetPosition(microphoneName);
        recordingClip = AudioClip.Create("Recording", pos, clipAudioChannels, clipAudioFrequencies, false);
        lastSamplePos = lastSamplePos + Microphone.GetPosition(microphoneName);
        //Allocate object on await whisper dictionary

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
        isRecording = false;
    }
}

