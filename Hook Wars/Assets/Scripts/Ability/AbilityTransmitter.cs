using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class AbilityTransmitter : MonoBehaviour, IOnEventCallback
{
    public void SendEventToPlayers(byte eventCode, object eventContent, ReceiverGroup receivers = ReceiverGroup.Others)
    {
        var options = new RaiseEventOptions() { Receivers = receivers };
        var sendOptions = new SendOptions() { Reliability = true };

        PhotonNetwork.RaiseEvent(eventCode, eventContent, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 101:
                GameAssets.TeamManager.GetPlayer((int)photonEvent.CustomData).gameObject.SetActive(false);
                break;
            case 102:
                GameAssets.TeamManager.GetPlayer((int)photonEvent.CustomData).gameObject.SetActive(true);
                break;
            case 103:
                break;
            case 104:
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
