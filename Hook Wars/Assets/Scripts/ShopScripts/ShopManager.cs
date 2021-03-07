using UnityEngine;

public class ShopManager : MonoBehaviour
{
    protected ShopItem currentSelectedItem;

    public delegate void ShopItemEvent(ShopItem item);
    private static event ShopItemEvent SelectButton;
    public static event ShopItemEvent Purchase;

    void Start()
    {
        Shop.Close += RemoveSelectedButton;
        SelectButton += Select;
    }

    private void Select(ShopItem item)
    {
        RemoveSelectedButton();
        currentSelectedItem = item;
    }

    protected void RemoveSelectedButton()
    {
        currentSelectedItem?.RemoveSelect();
        currentSelectedItem = null;
    }

    public static void RemoveOtherSelectedButton(ShopItem item)
    {
        SelectButton?.Invoke(item);
    }

    public void PurchaseItem()
    {
        if (currentSelectedItem == null)
        {
            Debug.Log("Item not selected");
            return;
        }

        if (GameAssets.Player.Items.Contains(currentSelectedItem.ItemID))
        {
            Debug.Log("Такой предмет уже куплен");
            return;
        }

        Purchase?.Invoke(currentSelectedItem);
        GameAssets.Player.WriteOffMoney(currentSelectedItem.Price);
        RemoveSelectedButton();
    }
}
