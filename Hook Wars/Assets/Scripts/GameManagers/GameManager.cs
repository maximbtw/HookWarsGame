using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Canvas     canvas      = null;
    [SerializeField] private GameObject waitingRoom = null;
    [SerializeField] private GameObject gameRoom    = null;
    [SerializeField] public  GameObject debugLog    = null;

    void Start()
    {
        GameAssets.Canvas = canvas;
        GameAssets.TeamManager = FindObjectOfType<TeamManager>();
        waitingRoom.SetActive(true);
        gameRoom.SetActive(false);
        debugLog.SetActive(false);
        waitingRoom.GetComponent<WaitingRoomController>().AddPlayersToList();
        WaitingRoomServer.LoadGameEvent += TurnOnGameScene;
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        var playerPrefab = PhotonNetwork.Instantiate(
                           GamePrefabs.instance.PlayerPrefab.name, 
                           Vector3.zero, 
                           Quaternion.identity);

        var controller = playerPrefab.GetComponent<PlayerController>();
        controller.Controller = PlayerController.Controll.User;
        controller.Joystick = GamePrefabs.instance.Joystick;
    }

    private void TurnOnGameScene()
    {
        waitingRoom.SetActive(false);
        gameRoom.SetActive(true);
    }

    public void Leave() 
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        waitingRoom.GetComponent<WaitingRoomController>().AddPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        waitingRoom.GetComponent<WaitingRoomController>().DisconnectPlayer(otherPlayer);
    }
}
