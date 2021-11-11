using RiptideNetworking;
using RiptideNetworking.Transports.RudpTransport;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalClient : MonoBehaviour
{
    public static string ip = "127.0.0.1";
    public static string port = "7777";

    public static string tempName;

    private static LocalClient _singleton;
    public static LocalClient Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(LocalClient)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }

    private Client client;

    public Client GetClient() { return client; }

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RiptideLogger.Initialize(Debug.Log, false);

        client = new Client(new RudpClient());
        client.Connected += DidConnect;
        client.ConnectionFailed += FailedToConnect;
        client.ClientDisconnected += PlayerLeft;
        client.Disconnected += DidDisconnect;
        GameObject obj = new GameObject();

    }

    public void Connect(string t)
    {
        client.Connect($"{ip}:{port}");
        tempName = t;
    }

    private void FixedUpdate()
    {
        client.Tick();
    }

    private void DidConnect(object sender, EventArgs e)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)Packets.ClientSend.SendName);
        message.Add(tempName);
        GetClient().Send(message);
        tempName = "";
        UIManager.Singleton.OnConnect();
    }

     private void FailedToConnect(object sender, EventArgs e)
     {
        RiptideLogger.Log("Connection Failed. Try Again.");
        UIManager.Singleton.FailedToConnect();
     }

     private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
     {

     }

     private void DidDisconnect(object sender, EventArgs e)
     {

     }

     private void OnApplicationQuit()
     {
        client.Disconnect();

        client.Connected -= DidConnect;
        client.ConnectionFailed -= FailedToConnect;
        client.ClientDisconnected -= PlayerLeft;
        client.Disconnected -= DidDisconnect;
     }

}
