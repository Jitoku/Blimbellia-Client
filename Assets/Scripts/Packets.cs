using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packets : MonoBehaviour
{
    public enum ClientGet : ushort
    {
        getWelcome = 1,
        getWorldData,
    }
    public enum ClientSend : ushort
    {
        SendName = 1,
        joinWorld, 
    }
}
