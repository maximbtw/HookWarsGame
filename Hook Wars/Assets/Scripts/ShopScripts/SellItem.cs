using UnityEngine.EventSystems;

public class SellItem : ShopItem
{
    private DisplayShopItem displayShop;
    private ButtonAbilityDisplay displayGame;

    private bool priceCurrcet;

    protected override void Start()
    {
        Price = ItemManager.GetPrice(ItemID);
        priceCurrcet = false;
        Shop.Close += CurrectPrice;
        displayGame = gameObject.GetComponent<ButtonAbilityDisplay>();
        displayShop = gameObject.AddComponent<DisplayShopItem>();
        displayGame.enabled = !displayGame.enabled;
    }

    private void CurrectPrice()
    {
        if (!priceCurrcet)
        {
            Price /= 2;
            priceCurrcet = true;
        }
    }

    public void SwichDisplay()
    {
        displayShop.enabled = !displayShop.enabled;
        displayGame.enabled = !displayGame.enabled;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (EnoughMoney)
        {
            ItemTableManager.RemoveOtherSelectedButton(this);
            Selected = true;
        }
    }

    public override bool EnoughMoney => true;
}
