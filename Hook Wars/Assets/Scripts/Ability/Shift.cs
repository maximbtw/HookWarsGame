using UnityEngine;
using Photon.Pun;

public class Shift : Ability
{
    [Range(0,10)]
    public float TimeDuration;
    private double startTime;

    protected override void StartAbility()
    {
        if (!AbilityStartCheck()) return;
        SetCooldown();
        GameEventManager.EventInvoke("Hide", GameAssets.Player.PlayerId, false);
        startTime = PhotonNetwork.Time;
        used = true;
    }

    protected override void UsedAbility()
    {
        if (PhotonNetwork.Time - startTime >= TimeDuration)
        {
            SetState();
            GameEventManager.EventInvoke("Hide", GameAssets.Player.PlayerId, true);
            used = false;
        }
    }
}
