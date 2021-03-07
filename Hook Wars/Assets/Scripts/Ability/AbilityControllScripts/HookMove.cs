using System.Collections.Generic;
using Photon.Pun;
using System.Linq;
using UnityEngine;

public class HookMove : MonoBehaviour
{
    private StateHook state = StateHook.None;
    private GameObject chainPrefab;
    private float speed;
    private float length;
    private Vector3 direction;
    private Vector3 startPosition;
    private Player hookedPlayer;
    private Rigidbody2D hookBody;

    private GameObject target;

    private List<ChainMove> chains;
    private ChainMove firstCahin => chains.First();
    private ChainMove lastCahin => chains.Last();

    private void Awake()=> hookBody = gameObject.GetComponent<Rigidbody2D>();

    public void SetStartProperty(float speed, float length, GameObject chainPrefab)
    {
        this.speed = speed;
        this.length = length;
        this.chainPrefab = chainPrefab;
        chains = new List<ChainMove>();
    }

    public void ThrowHook(Vector3 touchPos)
    {
        startPosition = transform.position = GameAssets.Player.transform.position;
        direction = (touchPos - startPosition).normalized;
        var angle = new Vector3(0, 0, MathAsset.GetAngleFromDir(direction) + 180);
        transform.eulerAngles = GameAssets.Player.transform.eulerAngles = angle;
        GameAssets.Player.TakeControll(false);
        state = StateHook.FlyForward;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        hookBody.MovePosition(transform.position + speed * direction * Time.fixedDeltaTime);
        switch (state)
        {
            case StateHook.FlyForward:
                UpdateFlyForward();
                break;
            case StateHook.FlyBack:
                UpdateFlyBack();
                break;
        }
    }

    private float lengthFromStart => (transform.position - startPosition).magnitude;
    private void UpdateFlyForward()
    {
        if (lengthFromStart >= length)
        {
            DeployHook();
            return;
        }

        if (chains.Count == 0)
        {
            if (lengthFromStart > 5)
            {
                AddChain();
            }
            return;
        }
        if((lastCahin.transform.position - startPosition).magnitude > 3)
        {
            AddChain();
        }
    }

    private void UpdateFlyBack()
    {
        direction = (target.transform.position - gameObject.transform.position).normalized;

        if ((lastCahin.transform.position - GameAssets.Player.transform.position).magnitude < 5)
        {
            PhotonNetwork.Destroy(lastCahin.gameObject);
            chains.Remove(lastCahin);

            if (chains.Count == 0)
            {
                PhotonNetwork.Destroy(gameObject);
                StopHooking();
                return;
            }
            lastCahin.SwichTarget(GameAssets.Player.gameObject);
        }

        if (hookedPlayer != null)
        {
            MathAsset.MoveObject(hookedPlayer.gameObject, gameObject, speed * 1.75f);
        }
    }

    private void StopHooking()
    {
        if (hookedPlayer == null) return;

        GameEventManager.EventInvoke("Return Controll", hookedPlayer.PlayerId, true);

        if (GameAssets.Player.Side != hookedPlayer.Side)
        {
            GameEventManager.EventInvoke("Was Murder", GameAssets.Player.PlayerId, hookedPlayer.PlayerId);
        }
    }

    private void AddChain()
    {
        var chain = PhotonNetwork.Instantiate(chainPrefab.name, startPosition, Quaternion.identity);
        var move = chain.AddComponent<ChainMove>();
        move.SwichTarget((chains.Count == 0) ? gameObject : lastCahin.gameObject);
        move.SetSpeed(speed);
        chains.Add(move);
    }

    private void DeployHook()
    {
        speed *= 1.75f;
        target = firstCahin.gameObject;

        for (int i = 0; i < chains.Count; i++)
        {
            var chainTarget = (i == chains.Count - 1) 
                            ? GameAssets.Player.gameObject 
                            : chains[i + 1].gameObject;
            chains[i].SwichTarget(chainTarget);
            chains[i].SetSpeed(speed);
        }

        state = StateHook.FlyBack;
        GameAssets.Player.ReturnControll(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null && player != GameAssets.Player && state == StateHook.FlyForward)
        {
            GameEventManager.EventInvoke("Take Controll", player.PlayerId, true);
            hookedPlayer = player;
            DeployHook();
        }
    }

    enum StateHook
    {
        None,
        FlyForward,
        FlyBack,
    }
}
