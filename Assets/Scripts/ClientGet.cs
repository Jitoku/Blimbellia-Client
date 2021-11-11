using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Transports.RudpTransport;

public class ClientGet : MonoBehaviour
{
    [MessageHandler((ushort)Packets.ClientGet.getWelcome)]
    public static void PlayerName(Message message)
    {
        string name = message.GetString();
        RiptideLogger.Log(name);

    }
}
