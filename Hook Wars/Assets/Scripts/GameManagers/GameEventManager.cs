using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;


public class GameEventManager : MonoBehaviour, IOnEventCallback
{
    public static List<GameEvent> Events = new List<GameEvent>()
    {
        new GameEvent("Was Murder",168, WasMurder, true),
        new GameEvent("Death",169, Death),
        new GameEvent("Kill", 170, Kill),
        new GameEvent("Hide", 171, HidePlayer),
        new GameEvent("Take Controll", 172, TakeControll),
        new GameEvent("Return Controll", 173, ReturnControll),
        new GameEvent("Immobilize", 174, Immobilize),
    };

    private static void Kill(params object[] eventParams)
    {
        var playerId = (int)eventParams[0];
        var gold     = (int)eventParams[1];

        var player = GameAssets.TeamManager.GetPlayer(playerId);
        player.Killed(gold);
    }

    private static void Death(params object[] eventParams)
    {
        var playerId = (int)eventParams[0];
        var gold = (int)eventParams[1];

        var player  = GameAssets.TeamManager.GetPlayer(playerId);
        player.Death(gold, 5f);
    }

    private static void WasMurder(params object[] eventParams)
    {
        var killerId = (int)eventParams[0];
        var killedId = (int)eventParams[1];

        var killer = GameAssets.TeamManager.GetPlayer(killerId);
        var killed = GameAssets.TeamManager.GetPlayer(killedId);

        var gold = 100 + (killed.PlayerStat.Streak - killer.PlayerStat.Streak) * 5;
        if (gold < 50) gold = 50;

        EventInvoke("Kill", killerId, gold);

        if (gold > killed.PlayerStat.Gold) gold = killed.PlayerStat.Gold;

        EventInvoke("Death", killedId, gold);
    }

    private static void HidePlayer(params object[] eventParams) 
    {
        var playerId = (int)eventParams[0];
        var hide     = (bool)eventParams[1];

        GameAssets.TeamManager.GetPlayer(playerId).gameObject.SetActive(hide);
    }

    private static void TakeControll(params object[] eventParams)
    {
        var playerId = (int)eventParams[0];
        var isRemoveBodyType = (bool)eventParams[1];

        GameAssets.TeamManager.GetPlayer(playerId).TakeControll(isRemoveBodyType);
    }

    private static void ReturnControll(params object[] eventParams)
    {
        var playerId = (int)eventParams[0];
        var isRemoveBodyType = (bool)eventParams[1];

        GameAssets.TeamManager.GetPlayer(playerId).ReturnControll(isRemoveBodyType);
    }

    private static void Immobilize(params object[] eventParams)
    {
        var playerId = (int)eventParams[0];
        var time =   (float)eventParams[1];

        GameAssets.TeamManager.GetPlayer(playerId).Immobilize(time);
    }

    public static void EventInvoke(string nameEvent, params object[] eventParams)
    {
        GameEvent gameEvent = Events.FirstOrDefault(x => x.Name == nameEvent);

        if (gameEvent.isLocalEvent)
        {
            gameEvent.ActionEvent.Invoke(eventParams);
            return;
        }

        var options = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        var sendOptions = new SendOptions() { Reliability = true };

        PhotonNetwork.RaiseEvent(gameEvent.Id, eventParams, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code < 150 || photonEvent.Code > 200) return;

        Events.FirstOrDefault(x => x.Id == photonEvent.Code)
              .ActionEvent
              .Invoke((object[])photonEvent.CustomData);
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public class GameEvent
    {
        public readonly bool isLocalEvent;
        public string Name { get; private set; }
        public byte Id { get; private set; }
        public Action<object[]> ActionEvent { get; private set; }

        public GameEvent(string name, byte id, Action<object[]> actionEvent, bool local = false)
        {
            Name = name;
            Id = id;
            ActionEvent = actionEvent;
            isLocalEvent = local;
        }
    }
}
