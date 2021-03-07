using Photon.Pun;
using UnityEngine;

public class Hook : Ability
{
    [Range(1f, 100)]
    public float Speed;
    [Range(25, 175)]
    public float Length;

    public GameObject HookPrefab;
    private GameObject hookPrefab;
    public GameObject ChainPrefab;

    protected override void StartAbility()
    {
        if (!AbilityStartCheck()) return;

        SetSelected();
        Speed *= GameAssets.Player.PlayerStat.PrecentAbilitySpeed;
        Length *= GameAssets.Player.PlayerStat.PrecentAbilityLength;
        used = true;
    }

    protected override void SelectedAbility()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Throw(Input.mousePosition);
            SetCooldown();
        }

        //TODO -- Mobile
        //if (Input.touchCount == 0) return;

        //var touch = (Input.touchCount == 1) ? Input.GetTouch(0) : Input.GetTouch(1);
        //if (touch.phase == TouchPhase.Began)
        //{
        //    Throw(touch.position);
        //    SetCooldown();
        //}
    }

    protected override void UsedAbility()
    {
        if (hookPrefab == null) used = false;
    }

    private void Throw(Vector2 touchPosition)
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touchPosition);

        hookPrefab = PhotonNetwork.Instantiate(HookPrefab.name, transform.position,Quaternion.identity);
        var hookMove = hookPrefab.GetComponent<HookMove>();
        hookMove.SetStartProperty(Speed, Length, ChainPrefab);
        hookMove.ThrowHook((Vector3)touchPos);
    }
}
