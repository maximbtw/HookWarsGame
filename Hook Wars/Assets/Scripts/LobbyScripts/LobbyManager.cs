using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public event Action StartLoad;
    public event Action LoadComplite;
    public event Action ExitToMainMenu;
    public event Action CreateMenuShowing;
    public event Action JoinMenuShowing;
    public MenuState State { get; private set; }

    public static FailMessage    FailMessage;
    public static List<RoomInfo> RoomList;

    public Text TextLog;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(1, 100);
            Log("Игрок получил имя: " + PhotonNetwork.NickName);

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1";
            var setting = new Photon.Realtime.AppSettings
            {
                BestRegionSummaryFromStorage = "EU",
            };

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.ConnectToRegion("EU");
            RoomList = new List<RoomInfo>();
        }

        FailMessage = gameObject.GetComponent<FailMessage>();
    }

    public void LoadStarting()
    {
        State = MenuState.Load;
        StartLoad?.Invoke();
    }

    public void QuickGame()
    {
        PhotonNetwork.JoinRandomRoom();
        LoadStarting();
    }

    public void ToJoinRoomMenu()
    {
        State = MenuState.JoinRoom;
        JoinMenuShowing?.Invoke();
    }

    public void ToCreateRoomMenu()
    {
        State = MenuState.CreateRoom;
        CreateMenuShowing?.Invoke();
    }

    public void ToMainMenu()
    {
        State = MenuState.Common;
        ExitToMainMenu?.Invoke();
    }

    public void ExitTheApplication()
    {
        Application.Quit();
    }

    public void CreateRoom()
    {
        var settings = new RoomOptions()
        {
            MaxPlayers = RoomSettings.CountPlayer,
            IsVisible = true,
            CustomRoomPropertiesForLobby = new string[] { "Password" },
            CustomRoomProperties = new Hashtable
            {
                { "Password", RoomSettings.Password }
            },
        };

        PhotonNetwork.CreateRoom(RoomSettings.Name, settings, null);
    }

    public override void OnConnectedToMaster()
    {
        Log("Connect to master");
        State = MenuState.Common;
        LoadComplite?.Invoke();
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(var room in roomList)
        {
            RoomList.Add(room);
        }
        Debug.Log(RoomList.Count);
        base.OnRoomListUpdate(roomList);
    }

    public override void OnJoinedRoom()
    {
        Log("Присоеденение к комнате " + PhotonNetwork.CurrentRoom.Name);
        Log("пароль комнаты " + PhotonNetwork.CurrentRoom.CustomProperties["Password"]);
        PhotonNetwork.LoadLevel("Game");
    }

    /// <summary>
    /// [Debug log] in the future should be removed
    /// </summary>
    /// <param name="message"></param>
    private void Log(string message)
    {
        Debug.Log(message);
        TextLog.text += "\n";
        TextLog.text += message;
    }

    public enum MenuState
    {
        Load,
        Common,
        CreateRoom,
        JoinRoom,
    }
}
