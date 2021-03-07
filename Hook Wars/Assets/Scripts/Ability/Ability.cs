using UnityEngine.EventSystems;
using UnityEngine;

public class Ability : MonoBehaviour, IPointerClickHandler
{
    [Range(0,1000)]
    public int Price;
    [Range(0, 200)]
    public float Cooldown;

    public float Timer { get; private set; }
    public AbilityState AbilityState { get; protected set; }

    protected AbilityTransmitter Transmitter;

    protected virtual void Start()
    {
        Transmitter = gameObject.AddComponent<AbilityTransmitter>();
        AbilityState = (Price > 0) 
            ? AbilityState.CantChoose 
            : AbilityState.NotActive;
    }

    protected void SetState()
    {
        if (AbilityState == AbilityState.Selected)
        {
            return;
        }
        else if (Timer > 0)
        {
            AbilityState = AbilityState.Cooldown;
        }
        else if (GameAssets.Player.PlayerStat.Gold < Price)
        {
            AbilityState = AbilityState.CantChoose;
        }
        else
        {
            AbilityState = AbilityState.NotActive;
        }
    }

    protected void SetCooldown()
    {
        Cooldown *= GameAssets.Player.PlayerStat.PrecentAbilityCooldown;
        Timer = Cooldown;
        AbilityState = AbilityState.Cooldown;
    }

    protected void SetSelected() => AbilityState = AbilityState.Selected;

    private void UpdateCooldown() => Timer -= Time.deltaTime;

    public void RefreshCooldown() => Timer = 0;

    protected bool used;

    protected virtual void Update()
    {
        SetState();
        switch (AbilityState)
        {
            case AbilityState.Cooldown:
                UpdateCooldown();
                break;
            case AbilityState.CantChoose:
                break;
            case AbilityState.NotActive:
                break;
            case AbilityState.Selected:
                SelectedAbility();
                break;
        }

        if (used) UsedAbility();
    }

    protected bool AbilityStartCheck()
    {
        if (AbilityState == AbilityState.Cooldown)
        {
            Debug.Log("Cooldown: " + Timer);
            return false;
        }
        if (AbilityState == AbilityState.CantChoose)
        {
            Debug.Log("Not enough money: " + (Price - GameAssets.Player.PlayerStat.Gold));
            return false;
        }
        if (used)
        {
            Debug.Log("Способность еще используется");
            return false;
        }
        return true;
    }

    protected virtual void StartAbility() { }

    protected virtual void SelectedAbility() { }

    protected virtual void UsedAbility() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartAbility();
    }
}
