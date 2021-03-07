public class Item : Ability
{
    public ItemManager.ItemID ItemID;
    public bool Passiv;

    private State state;

    protected override void Start()
    {
        base.Start();
        if (Passiv)
        {
            AbilityState = AbilityState.CantChoose;
        }
        AddItem();
        var sellItem = gameObject.AddComponent<SellItem>();
        sellItem.ItemID = ItemID;
    }

    protected override void Update()
    {
        if (Passiv) return;
        base.Update();
    }

    protected virtual void StartItemAbility() { }

    protected override void StartAbility()
    {
        if (state == State.Game)
        {
            StartItemAbility();
        }
    }

    protected virtual void AddItem() 
    {
        Shop.Close += SwichState;
        Shop.Open += SwichState;
    }

    public virtual void RemoveItem() 
    {
        Shop.Close -= SwichState;
        Shop.Open -= SwichState;
    }

    private void SwichState()
    {
        state = (State)(((int)state + 1 ) % 2);
        GetComponent<SellItem>()?.SwichDisplay();
    }

    public enum State
    {
        Game, Shop,
    }
}
