using UnityEngine;

public static class ItemManager 
{
    public enum ItemID
    {
        None,
        Boots,
        Gips
    }

    public static int GetPrice(ItemID id)
    {
        foreach (var item in GameAssets.instance.PurchaseItemArray)
            if (item.ItemID == id)
                return item.Price;
        Debug.Log("Item " + id + " not foand!");
        return -1;
    }

    public static Item GetItem(ItemID id)
    {
        foreach (var item in GameAssets.instance.PurchaseItemArray)
            if (item.ItemID == id)
                return item.Item;
        Debug.Log("Item " + id + " not foand!");
        return null;
    }
}
