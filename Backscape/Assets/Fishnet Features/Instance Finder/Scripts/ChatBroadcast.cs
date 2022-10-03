using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;

public class ChatBroadcast : NetworkBehaviour
{
    [SerializeField] private Transform chatHolder;
    [SerializeField] private GameObject msgElement;
    [SerializeField] private TMP_InputField playerMessage;
    private string playerName;

    public struct Message : IBroadcast 
    {
        public string userName;
        public string message;
    }
    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageRecieved);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(OnClientMessageRecieved);
    }
    private void Awake()
    {
        playerMessage.onSubmit.AddListener(MessageSubmit);
        PlayerStats.OnPlayerNameSet += PlayerStats_OnPlayerNameSet;
    }

    private void PlayerStats_OnPlayerNameSet(string obj)
    {
        playerName = obj;
    }


    private void MessageSubmit(string text)
    {
        SendMessage();
    }

    private void SendMessage() 
    {
        Message msg = new Message()
        {
            userName = playerName,
            message = playerMessage.text

        };
        if (InstanceFinder.IsServer)
        {
            InstanceFinder.ServerManager.Broadcast(msg);
        }
        else 
        {
            InstanceFinder.ClientManager.Broadcast(msg);
        }
    }
    private void OnMessageRecieved(Message msg) 
    {
        GameObject finalMessage = Instantiate(msgElement, chatHolder);
        finalMessage.GetComponent<TextMeshProUGUI>().text = msg.userName + ": " + msg.message; 
    
    }

    private void OnClientMessageRecieved(NetworkConnection networkConnection, Message msg)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
}
