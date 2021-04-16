using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplayController : MonoBehaviour
{
    [SerializeField]
    protected GameObject itemDisplayImg;
    [SerializeField]
    public Text itemName;
    [SerializeField]
    public Text itemCost;

    protected Item item;
    protected int slot;

    [HideInInspector]
    public ShopManager shopManager;
    [HideInInspector]
    public ShopMenuController MenuController;


    [SerializeField]
    protected GameObject confirmPrompt;

    public virtual void Init(HealItem item, int slot, ShopManager m, ShopBuyMenuController buyMenu)
    {
        this.item = item;
        this.slot = slot;
        itemName.text = item.Name;
        itemCost.text = item.Cost.ToString() + "g";
        this.shopManager = m;
        this.MenuController = buyMenu;
    }

    public virtual void Init(EquipableItem item, int slot, ShopManager m, ShopBuyMenuController buyMenu)
    {
        this.item = item;
        this.slot = slot;
        itemName.text = item.Name;
        itemCost.text = item.Cost.ToString() + "g";
        this.shopManager = m;
        this.MenuController = buyMenu;
    }
}
