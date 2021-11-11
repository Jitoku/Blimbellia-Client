using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Transports.RudpTransport;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        ConnectDialog = 1, //Default State
        JoinWorldDialog,
    }

    public GameObject[] UIDialogs;

    UIState curState = UIState.ConnectDialog;

    public GameObject joinButton;

    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }


    public GameObject nameInputField;
    public GameObject worldInputField;

    private void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect()
    {
        if (curState != UIState.ConnectDialog)
            return;
        string t = nameInputField.GetComponent<TMP_InputField>().text;
        if (t.Equals(""))
            return;

        joinButton.GetComponent<Button>().interactable = false;
        LocalClient.Singleton.Connect(t);
    }

    public void FailedToConnect()
    {
        joinButton.GetComponent<Button>().interactable = true;
    }

    public void OnConnect()
    {
        ChangeUIDialog(UIState.JoinWorldDialog);
    }

    public void JoinWorld()
    {
        string t = worldInputField.GetComponent<TMP_InputField>().text;
        Regex regex = new Regex(Rules.pattern);

        // Compare a string against the regular expression
        if (t.Equals("") || Regex.IsMatch(t, Rules.pattern))
        {
            Debug.Log("Invalid World Name!");
            return;
        }
        Message message = Message.Create(MessageSendMode.reliable, (ushort)Packets.ClientSend.joinWorld);
        message.Add(t);
        LocalClient.Singleton.GetClient().Send(message);
    }

    public void ChangeUIDialog(UIState state)
    {
        if (curState == state)
        {
            Debug.Log("Same State. Returning...");
            return;
        }
        UIDialogs[(int)curState - 1].SetActive(false);
        curState = state;
        UIDialogs[(int)curState - 1].SetActive(true);
    }

    public void OnJoinWorld()
    {

    }
}
