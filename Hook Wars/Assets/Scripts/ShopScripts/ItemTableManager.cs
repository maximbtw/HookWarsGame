using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class ItemTableManager : ShopManager
{
    private Slot[] slots;

    private void Start()
    {
        slots = GetComponentsInChildren<Image>().Select(x => new Slot(x.transform)).ToArray();
        ShopManager.Purchase += AddItem;
        SellItemManager.Sell += RemoveItem;
    }

    public void AddItem(ShopItem shopItem)
    {
        var itemID = shopItem.ItemID;

        foreach(var slot in slots)
        {
            if (!slot.Equipped)
            {
                var item = Instantiate(ItemManager.GetItem(itemID).gameObject);
                item.transform.SetParent(transform);
                slot.Add(item.GetComponent<Item>());
                GameAssets.Player.Items.Add(itemID);
                return;
            }
        }
        Debug.Log("All slots are equipped");
    }

    public void RemoveItem(ShopItem shopItem)
    {
        var itemID = shopItem.ItemID;

        foreach (var slot in slots)
        {
            if (slot.Equipped && slot.Item.ItemID == itemID)
            {
                Destroy(slot.Item.gameObject);
                slot.Remove();
                GameAssets.Player.Items.Remove(itemID);
                break;
            }
        }
    }

    class Slot
    {
        public Transform Transform { get; private set; }
        public bool Equipped { get; private set; }
        public Item Item { get; private set; }

        public Slot(Transform transform)
        {
            transform.gameObject.SetActive(false);
            Transform = transform;
            Equipped = false;
        }

        public void Add(Item item)
        {
            Equipped = true;

            item.transform.position = Transform.position;
            item.transform.localScale = Transform.localScale; 
            Item = item;
        }

        public void Remove()
        {
            Equipped = false;
            Item.RemoveItem();
            Item = null;
        }
    }
}
