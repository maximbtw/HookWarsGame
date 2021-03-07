using UnityEngine;
using TMPro;
using Photon.Pun;

public class VisuasiableLobby : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu       = null;
    [SerializeField] private GameObject menuCreateRoom = null;
    [SerializeField] private GameObject menuJoinRoom   = null;
    [SerializeField] private TMP_Text   loadingLog     = null;

    private string loadingText = "Загрузка";
    private string customText  = string.Empty;
    private double startTime;


    private LobbyManager lobby;

    void Start()
    {
        lobby = gameObject.GetComponent<LobbyManager>();
        SetStartTime();

        mainMenu.SetActive(false);
        menuCreateRoom.SetActive(false);
        menuJoinRoom.SetActive(false);
        loadingLog.gameObject.SetActive(true);

        lobby.LoadComplite    += HideLoadText;
        lobby.LoadComplite    += ShowMainMenu;

        lobby.StartLoad       += HideMainMenu;
        lobby.StartLoad       += HideCreteRoomMenu;
        lobby.StartLoad       += HideJoinRoomMenu;
        lobby.StartLoad       += CloseLobby;

        lobby.CreateMenuShowing += HideMainMenu;
        lobby.JoinMenuShowing   += HideMainMenu;

        lobby.ExitToMainMenu  += ShowMainMenu;
        lobby.ExitToMainMenu  += HideJoinRoomMenu;
        lobby.ExitToMainMenu  += HideCreteRoomMenu;

        CreateMenuController.onCreateRoom += lobby.CreateRoom;
        mainMenu.GetComponent<AnimatorController>().AnimationComplite += CloseLobby;
    }

    void Update()
    {
        if (lobby.State == LobbyManager.MenuState.Load)
        {
            UpdateLodadingText();
        }
    }

    private void UpdateLodadingText()
    {
        if (PhotonNetwork.Time - startTime >= 1)
        {
            if (customText.Length == 3)
            {
                customText = string.Empty;
            }
            else
            {
                customText += ".";
            }
            startTime = PhotonNetwork.Time;
        }
        loadingLog.text = loadingText + customText;
    }

    private void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        mainMenu.GetComponent<Animator>().Play("LobbyMenuOpen");
    }

    private void HideMainMenu()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.GetComponent<Animator>().Play("LobbyMenuClose");
        }
    }

    //Этот метод подписан на конец анимации скрытия основного меню
    private void CloseLobby()
    {
        mainMenu.SetActive(false);
        switch (lobby.State)
        {
            case LobbyManager.MenuState.Load:
                SetStartTime();
                ShowLoadText();
                break;
            case LobbyManager.MenuState.CreateRoom:
                ShowCreteRoomMenu();
                break;
            case LobbyManager.MenuState.JoinRoom:
                ShowJoinRoomMenu();
                break;
        }
    }

    private void ShowJoinRoomMenu() => menuJoinRoom.SetActive(true);

    private void HideJoinRoomMenu() => menuJoinRoom.gameObject.SetActive(false);

    private void ShowCreteRoomMenu() => menuCreateRoom.SetActive(true);

    private void HideCreteRoomMenu() => menuCreateRoom.gameObject.SetActive(false);

    private void ShowLoadText() => loadingLog.gameObject.SetActive(true);

    private void HideLoadText() => loadingLog.gameObject.SetActive(false);

    private void SetStartTime() => startTime = PhotonNetwork.Time;
}
