using UnityEngine;

public class SellItemManager : ShopManager
{
    protected new ShopItem currentSelectedItem;
    public static event ShopItemEvent Sell;

    public void SellItem()
    {
        if (currentSelectedItem == null)
        {
            Debug.Log("Item not selected");
            return;
        }

        Sell?.Invoke(currentSelectedItem);
        GameAssets.Player.WriteOffMoney(-currentSelectedItem.Price);
        RemoveSelectedButton();
    }
}

