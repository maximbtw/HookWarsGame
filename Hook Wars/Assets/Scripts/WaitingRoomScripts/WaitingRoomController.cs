using UnityEngine;
using Photon.Pun;
using System.Linq;

public class WaitingRoomController : MonoBehaviour
{
    private WaitingRoomServer server;
    private PlayerList  playerList;
    private ChatManager chat;

    void Awake()
    {
        server = GetComponent<WaitingRoomServer>();
        chat = GetComponentInChildren<ChatManager>();
        playerList = GetComponentInChildren<PlayerList>();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (playerList.GetSides().Any(side => (Side)side == Side.None))
            {
                chat.SenServerMessage(ChatWriter.MessageType.PrivateServer, 
                    "Не все игроки распределены по командам");
            }
            else
            {
                server.SendEventToPlayers(42, playerList.GetSides());
            }
        }
        else
        {
            chat.SenServerMessage(ChatWriter.MessageType.PrivateServer, "Вы не хост");
        }
    }

    public void AddPlayer(Photon.Realtime.Player newPlayer)
    {
        playerList.AddPlayer(newPlayer);
        server.SendEventToPlayers(43, playerList.GetSides(), Photon.Realtime.ReceiverGroup.Others);
    }
    public void AddPlayersToList()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerList.AddPlayer(player);
        }
        playerList.UpdatePlayers();
    }

    public void DisconnectPlayer(Photon.Realtime.Player otherPlayer)
    {
        playerList.RemovePlayer(otherPlayer);
        chat.SenServerMessage(ChatWriter.MessageType.GlobalServer, otherPlayer.NickName + " вышел");
    }

    public void ShufleTeam()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            server.SendEventToPlayers(43, playerList.GetShufleSide());
        }
        else
        {
            chat.SenServerMessage(ChatWriter.MessageType.PrivateServer, "Вы не хост");
        }
    }

    public void SetSidesToPlayers(int[] sides)
    {
        playerList.SetSides(sides);
    }

    public Side GetSidePlayer(Photon.Realtime.Player player)
    {
        return playerList.GetSidePlayer(player);
    }
}
