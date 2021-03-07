using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Player : MonoBehaviour
{
    public int PlayerId => PhotonView.OwnerActorNr;

    public Side Side;
    [NonSerialized] public List<ItemManager.ItemID> Items;
    public PlayerStat PlayerStat { get; private set; }
    public PhotonView PhotonView { get; private set; }
    public PlayerController PlayerController { get; private set; }

    private PlayerController.Controll startControll;

    void Start()
    {
        PlayerStat       = GetComponent<PlayerStat>();
        PhotonView       = GetComponent<PhotonView>();
        PlayerController = GetComponent<PlayerController>();
        Items = new List<ItemManager.ItemID>();
        transform.SetParent(GamePrefabs.instance.PlayersPlace.transform, false);

        gameObject.name = PhotonView.Owner.NickName 
            + (PhotonView.Owner.Equals(PhotonNetwork.LocalPlayer) ? " (me)" : string.Empty);
    }

    private void Update()
    {
        if (PlayerController == null)
            PlayerController = GetComponent<PlayerController>();
        if (PlayerStat == null)
            PlayerStat = GetComponent<PlayerStat>();
    }

    public void Spawn()
    {
        transform.position = GameAssets.TeamManager.GetPosition(this);
        if (PhotonView.IsMine)
        {
            GameEventManager.EventInvoke("Hide", PlayerId, true);
        }
    }

    public void Killed(int gold)
    {
        PlayerStat.KDAStat.Kill++;
        PlayerStat.Gold += gold;
        PlayerStat.Streak++;
    }

    public void Death(int gold, float time)
    {
        PlayerStat.KDAStat.Death++;
        PlayerStat.Streak = 0;
        PlayerStat.Gold -= gold;
        if (PhotonView.IsMine)
        {
            GameEventManager.EventInvoke("Hide", PlayerId, false);
            RespawnManager.AddRespawn(this, time);
        }
    }

    public void WriteOffMoney(int gold)
    {
        if(PlayerStat.Gold < gold)
        {
            Debug.LogError($"{gold} - у игрока нет столько денег");
            gold = PlayerStat.Gold;
        }
        PlayerStat.Gold -= gold;
    }

    public void ChangeSpeed(float speed)
    {
        if (PlayerStat.Speed + speed < 0)
        {
            PlayerStat.Speed = 0;
            return;
        }
        PlayerStat.Speed += speed;
    }

    public void Immobilize(float disabledTime) => PlayerController.Immobilize(disabledTime);

    public void TakeControll(bool isRemoveBodyType)
    {
        startControll = PlayerController.Controller;
        PlayerController.Controller = PlayerController.Controll.TakeControll;
        if(isRemoveBodyType)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public void ReturnControll(bool isRemoveBodyType)
    {
        PlayerController.Controller = startControll;
        if (isRemoveBodyType)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
