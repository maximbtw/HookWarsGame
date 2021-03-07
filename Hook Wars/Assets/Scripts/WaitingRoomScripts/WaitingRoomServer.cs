using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using System;

public class WaitingRoomServer : MonoBehaviour, IOnEventCallback
{
    public static event Action LoadGameEvent;

    private WaitingRoomController roomController;

    void Start() => roomController = GetComponent<WaitingRoomController>();

    public void SendEventToPlayers(byte eventCode, object eventContent, ReceiverGroup receivers = ReceiverGroup.All)
    {
        var options = new RaiseEventOptions() { Receivers = receivers };
        var sendOptions = new SendOptions() { Reliability = true };

        PhotonNetwork.RaiseEvent(eventCode, eventContent, options, sendOptions);
    }

    private void StartGame(int[] sides)
    {
        FindObjectOfType<GameManager>().debugLog.SetActive(true);
        GameAssets.TeamManager.AddPlayers();
        GameAssets.TeamManager.SetSidesToPlayers(sides);
        GameAssets.TeamManager.CreateTeam();
        GameAssets.TeamManager.SetMainPlayer(PhotonNetwork.LocalPlayer.UserId);
        GameAssets.Player.Spawn();
        LoadGameEvent?.Invoke();
        Debug.Log("Game start!");
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code > 45) return;
        int[] sides;
        if (photonEvent.CustomData is int[])
        {
            sides = (int[])photonEvent.CustomData;
            if (sides == null || sides.Length != PlayerList.Players.Count) return;
        }
        else return;

        switch (photonEvent.Code)
        {
            case 42:
                StartGame(sides);
                break;
            case 43:
                roomController.SetSidesToPlayers(sides);
                break;
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
