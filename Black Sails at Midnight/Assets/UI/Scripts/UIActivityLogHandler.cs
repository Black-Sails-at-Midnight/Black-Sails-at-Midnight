using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIActivityLogHandler : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField]
    public TextMeshProUGUI timestampField;
    
    [SerializeField]
    public TextMeshProUGUI messageField;

    [Header("Message History Settings")]
    [SerializeField]
    public int maxRetainedMessages = 50;
    
    [SerializeField]
    public List<Message> messageHistory;

    public struct Message
    {
        public System.DateTime time;
        public string message;
        public bool displayed;
    }

    void Start()
    {
        messageHistory = new();

        timestampField.text = "";
        messageField.text = "";
    }

    public void LogMessage(string _message)
    {
        Message message = new Message {time = System.DateTime.Now, message = _message, displayed = false};
        messageHistory.Add(message);

        if (messageHistory.Count > maxRetainedMessages)
        {
            messageHistory.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (messageHistory.Count == 0)
            return;

        Message lastMessage = messageHistory[messageHistory.Count - 1];
        if (lastMessage.displayed == false)
        {
            timestampField.text = lastMessage.time.ToShortTimeString();
            messageField.text = lastMessage.message;
        }
    }
}
