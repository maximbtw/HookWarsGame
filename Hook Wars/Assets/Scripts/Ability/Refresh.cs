using UnityEngine;

public class Refresh : Ability
{
    protected override void StartAbility()
    {
        if (!AbilityStartCheck()) return;
        SetCooldown();
        // Sound Refresh
        GameAssets.Player.WriteOffMoney(Price);
        RefreshAll();
    }
    private void RefreshAll()
    {
        foreach (var ability in GameAssets.Canvas.GetComponentsInChildren<Ability>())
        {
            ability.RefreshCooldown();
        }
    }
}
