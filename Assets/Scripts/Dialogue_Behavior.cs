using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue_Behavior : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI GPT3_textBox;
    [SerializeField] TextMeshProUGUI user_textbox;

    // Start is called before the first frame update
    void Start()
    {
        user_textbox.text = "Testing text change";
    }

    public void updateText(bool fromUser, string dialogue){
        if(!fromUser){ // AI Response
            GPT3_textBox.text = dialogue;
        }
        else if (fromUser){ //user message
            user_textbox.text = dialogue;
        }
    }
}
