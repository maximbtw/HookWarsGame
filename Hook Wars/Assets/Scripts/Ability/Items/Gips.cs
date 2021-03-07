using UnityEngine;

public class Gips : Item
{
    public float TimeDuration;
    [Range(1f, 50)]
    public float Speed;
    [Range(25, 175)]
    public float Length;

    public  GameObject GipsPrefab;
    private GameObject gipsPrefab;


    protected override void StartAbility()
    {
        if (!AbilityStartCheck()) return;
        // -- Select Gips

        TargetPlayer.RemoveTarget();
        SetSelected();
        Length *= GameAssets.Player.PlayerStat.PrecentAbilityLength;
    }

    protected override void SelectedAbility()
    {
        if (TargetPlayer.Target != null && TargetPlayer.Target.Side != GameAssets.Player.Side)
        {
            Throw(TargetPlayer.Target);
            SetCooldown();
        }
    }

    private void Throw(Player target)
    {
        // -- Throw Gips

        gipsPrefab = Instantiate(GipsPrefab);
        var gipsMove = gipsPrefab.AddComponent<GipsMove>();
        gipsMove.SetStartProperty(TimeDuration, Speed, Length);
        gipsMove.Throw(target.gameObject);
    }
}
