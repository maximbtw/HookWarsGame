public class Boots : Item
{
    public float Speed;

    protected override void AddItem()
    {
        base.AddItem();
        GameAssets.Player.ChangeSpeed(Speed);
    }

    public override void RemoveItem()
    {
        base.RemoveItem();
        GameAssets.Player.ChangeSpeed(-Speed);
    }
}
