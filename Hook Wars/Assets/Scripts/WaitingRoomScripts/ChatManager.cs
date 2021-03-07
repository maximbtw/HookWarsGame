using ExitGames.Client.Photon;
using UnityEngine.UI;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    [SerializeField] private InputField inputField = null;

    private ChatClient chatClient;
    private ChatWriter chatWriter;

    void Start()
    {
        chatWriter = GetComponent<ChatWriter>();

        chatClient = new ChatClient(this);
        chatClient.ChatRegion = "EU";
        chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            PhotonNetwork.AppVersion,
            new AuthenticationValues(PhotonNetwork.NickName));
    }

    void Update()
    {
        chatClient.Service();    
    }

    public void SendMessage()
    {
        string text = inputField.text;
        if (string.IsNullOrEmpty(text)) return;

        inputField.text = string.Empty;
        inputField.placeholder.GetComponent<Text>().text = "Введите текст...";

        chatClient.PublishMessage("main", "0" + PhotonNetwork.NickName + ":  " + text);
    }

    public void SenServerMessage(ChatWriter.MessageType type, string message)
    {
        chatClient.SendPrivateMessage(PhotonNetwork.NickName, (int)type + message);
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { "main" });
        chatClient.PublishMessage("main", "1" + PhotonNetwork.NickName + " присоединился");
    }

    public void OnDisconnected()
    {
        chatClient.PublishMessage("main", "1" + PhotonNetwork.NickName + " вышел");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for(int i = 0; i < senders.Length; i++)
        {
            if (messages[i].ToString()[0] == '1')
            {
                chatWriter.SendMessage(ChatWriter.MessageType.GlobalServer, messages[i].ToString());
            }
            else
            {
                chatWriter.SendMessage(ChatWriter.MessageType.Global, messages[i].ToString());
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (message.ToString()[0] == '1')
        {
            chatWriter.SendMessage(ChatWriter.MessageType.GlobalServer, message.ToString());
        }
        else if (message.ToString()[0] == '2')
        {
            chatWriter.SendMessage(ChatWriter.MessageType.PrivateServer, message.ToString());
        }
        else
        {
            //TODO -- send private message to player other player   
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }
}
