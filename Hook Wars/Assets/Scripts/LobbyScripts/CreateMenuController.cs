using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;
using TMPro;

public class CreateMenuController : MonoBehaviour
{
    [SerializeField] private CheckBox   checkBoxPrivateRoom = null;
    [SerializeField] private InputField inputFieldName      = null;
    [SerializeField] private InputField inputFieldPassword  = null;
    [SerializeField] private TMP_Text   textCountPlayers    = null;
    [SerializeField] private Animator   buttonSwichCountPlayersAnimator = null;

    private int countPlayersInOneTeam;

    public static event Action onCreateRoom;

    void Start()
    {
        countPlayersInOneTeam = 3;
        UpdateText();
    }

    public void ClickSwichCountPlayers()
    {
        buttonSwichCountPlayersAnimator.Play("Click Animation");
        countPlayersInOneTeam++;
        if (countPlayersInOneTeam > 10) countPlayersInOneTeam = 1;
        UpdateText();
    }

    private void UpdateText() => textCountPlayers.text = countPlayersInOneTeam + " x " + countPlayersInOneTeam;

    public void CreateRoom()
    {
        if (checkBoxPrivateRoom.isSelected)
        {
            CreatePrivateRoom();
        }
        else
        {
            CreatePublicRoom();
        }
    }

    private void CreatePrivateRoom()
    {
        if (string.IsNullOrEmpty(inputFieldName.text)
        || string.IsNullOrEmpty(inputFieldPassword.text))
        {
            LobbyManager.FailMessage.Log("введите название комнаты и пароль");
            return;
        }
        if (!CheckingRoomName()) return;

        RoomSettings.CountPlayer = (byte)(countPlayersInOneTeam * 2);
        RoomSettings.Name = inputFieldName.text;
        RoomSettings.Password = inputFieldPassword.text;

        onCreateRoom?.Invoke();
        FindObjectOfType<LobbyManager>().LoadStarting();
    }

    private void CreatePublicRoom()
    {
        if (string.IsNullOrEmpty(inputFieldName.text))
        {
            LobbyManager.FailMessage.Log("дайте название комнате");
            return;
        }
        if (!CheckingRoomName()) return;

        RoomSettings.CountPlayer = (byte)(countPlayersInOneTeam * 2);
        RoomSettings.Name = inputFieldName.text;
        RoomSettings.Password = string.Empty;

        onCreateRoom?.Invoke();
        FindObjectOfType<LobbyManager>().LoadStarting();
    }

    private bool CheckingRoomName()
    {
        if (LobbyManager.RoomList.Any(room => room.Name.Equals(inputFieldName.text)))
        {
            LobbyManager.FailMessage.Log("комната с таким именем уже существует");
            inputFieldName.text = string.Empty;
            return false;
        }
        return true;
    }
}
