using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ChatLog : EventListenerBase
{
    [SerializeField] private GameObject chatFrame;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private List<string> chatLogEntries = new List<string>();

    public string Filter(string s)
    {
        //TODO: filter the chat for profanity etc then return chat message

        return s;
    }

    public override void EventMethod()
    {
        //message is a blank string on the eventlistenerbase class to be populated by
        //a tcp signal from the event holding class
        //not sure why I need a listener observer for chat when it could be handled by one object
        chatLogEntries.Add(Filter(message));
        GameObject nextText = Instantiate(textPrefab, chatFrame.transform);
        nextText.GetComponent<TMP_Text>().text = message;
    }

    
    
}
