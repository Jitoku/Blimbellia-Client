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

    [MessageHandler((ushort)Packets.ClientGet.getWorldData)]
    public static void HasJoinedWorld(Message message)
    {
        UIManager.Singleton.OnJoinWorld(message);
    }

    [MessageHandler((ushort)Packets.ClientGet.getChatMessageFromPlayer)]
    public static void GetChatMessage(Message message)
    {
        string[] names;
        if (message.GetBool())
        {
            names = message.GetString().Split(':');
            RiptideLogger.Log($"Player With Local ID '{names[0]}' Has Sent The Message '{names[1]}'");
        }
        else
        {
            RiptideLogger.Log($"You Have Sent The Message '{message.GetString()}'.");
        }
    }
}
