using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ChatSharp;

public class IRCClient : MonoBehaviour {
    private IrcClient Client;
    private EventManager EventManager;

    public string Username = "";
    public string Password = "";
    public string Roomname = "";

    public void Start() {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Roomname))
        {
            Debug.LogWarning("IRC client requires configuration!", this);
        }

        EventManager = new EventManager();

        Debug.Log("[IRC] Starting...");
        Client = new IrcClient("chat.eu.freenode.net", new IrcUser(Username, Username, Password));
        Client.ConnectionComplete += Client_ConnectionComplete;
        Client.ChannelMessageRecieved += Client_ChannelMessageRecieved;

        Debug.Log("[IRC] Connecting...");
        Client.ConnectAsync();
    }

    public static string DebugMessageEvent(ChatSharp.Events.PrivateMessageEventArgs e)
    {
        string parameters = "";
        for (int i = 0; i < e.IrcMessage.Parameters.Length; i++) {
            if (i != 0) {
                parameters += ", ";
            }
            parameters += e.IrcMessage.Parameters[i];
        }

        string result = string.Format(
          @"IrcMessage: 
            Command: {0}
            Parameters: [{1}]
            Prefix: {2}
            Raw message: {3}

            PrivateMessage:
            IsChannelMessage: {4}
            Message: {5}
            Source: {6}
            User: {7}",
          e.IrcMessage.Command, parameters, e.IrcMessage.Prefix, e.IrcMessage.RawMessage,
          e.PrivateMessage.IsChannelMessage, e.PrivateMessage.Message, e.PrivateMessage.Source, e.PrivateMessage.User
        );

        return result;
    }

    void Client_ChannelMessageRecieved(object sender, ChatSharp.Events.PrivateMessageEventArgs e)
    {
        Debug.Log("[IRC] Message!");
        if (e.PrivateMessage.Message.Contains("team")) {
            if (e.PrivateMessage.Message.Contains("blue")) {
                EventManager.QueueEvent(() =>
                {
                    Minions.SpawnAllLanes(true, 2);
                    Client.SendMessage("Sending reinforcements to team BLUE!", "#vimaster_game");
                });
            }

            if (e.PrivateMessage.Message.Contains("red"))
            {
                EventManager.QueueEvent(() =>
                {
                    Minions.SpawnAllLanes(false, 2);
                    Client.SendMessage("Sending reinforcements to team RED!", "#vimaster_game");
                });
            }
        }
    }

    void OnDestroy() {
        if (Client != null)
        {
            Client.Quit("Game shut down.");
        }
    }

    void Client_ConnectionComplete(object sender, System.EventArgs e)
    {
        Debug.Log("[IRC] Connected!");
        Client.JoinChannel("#" + Roomname.ToLower());
    }

    void Update()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Roomname))
        {
        }

        EventManager.Update();
    }
}
