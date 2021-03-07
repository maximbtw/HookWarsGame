using UnityEngine;
using UnityEngine.UI;

public class ChatWriter : MonoBehaviour
{
    [SerializeField] private RectTransform content = null;
    [SerializeField] private Text      contentItem = null;

    public void SendMessage(MessageType type, string text)
    {
        text = text.Substring(1);

        switch (type)
        {
            case MessageType.Global:
                SendFormatMessage(text, new Color(0.3187611f, 0.6981132f, 0.5157324f));
                break;
            case MessageType.GlobalServer:
                SendFormatMessage("сервер:  " + text, new Color(0.7450981f, 0.7098039f, 0.772549f));
                break;
            case MessageType.PrivateServer:
                SendFormatMessage("(private) сервер:  " + text, new Color(0.9137255f, 0.6509804f, 0.3333333f));
                break;
        }
    }

    private void SendFormatMessage(string text, Color color)
    {
        Text newMessage = Instantiate(contentItem);

        newMessage.text = text;
        newMessage.color = color;

        newMessage.transform.SetParent(content, false);
    }

    public enum MessageType
    {
        Global,
        GlobalServer,
        PrivateServer,       
    }
}
