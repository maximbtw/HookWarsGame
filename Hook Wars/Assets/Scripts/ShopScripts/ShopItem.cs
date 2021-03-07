using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public ItemManager.ItemID ItemID;
    public int Price { get; protected set; }

    protected virtual void Start()
    {
        gameObject.AddComponent<DisplayShopItem>();
        Price = ItemManager.GetPrice(ItemID);
        gameObject.GetComponent<Image>().sprite 
            = ItemManager.GetItem(ItemID)?.gameObject.GetComponent<Image>().sprite;

        GetComponentInChildren<Text>().text = Price.ToString();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (EnoughMoney && !Selected)
        {
            ShopManager.RemoveOtherSelectedButton(this);
            Selected = true;
        }
    }

    public void RemoveSelect() { Selected = false; }
    public bool Selected { get; protected set; }
    public virtual bool EnoughMoney => GameAssets.Player.PlayerStat.Gold >= Price;
}
