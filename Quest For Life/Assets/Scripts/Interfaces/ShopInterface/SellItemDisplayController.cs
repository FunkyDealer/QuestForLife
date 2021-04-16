using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemDisplayController :ShopItemDisplayController 
{
    InventorySlot InventorySlot;
    [SerializeField]
    Text ammount;

    int cost;

    public void Init(InventorySlot slot, ShopManager m, ShopSellMenuController buyMenu)
    {
        this.InventorySlot = slot;
        this.cost = (int)(InventorySlot.GetStack().item.Cost * 0.4f);
        itemName.text = InventorySlot.GetStack().item.Name;
        itemCost.text = cost.ToString() + "g";
        this.shopManager = m;
        this.MenuController = buyMenu;

        UpdateAmmount();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateAmmount()
    {
        ammount.text = InventorySlot.GetStack().quantity.ToString();

        
    }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }


    public void SellItem()
    {
            MenuController.confirmation();
            GameObject o = Instantiate(confirmPrompt); //Create the confirm prompt
            GenericQuantityConfirmInterface g = o.GetComponent<GenericQuantityConfirmInterface>();
            g.Init(confirmSellAction, MenuController.manager.unLockInterface, $"Are you sure you want to Sell {InventorySlot.GetStack().item.Name} for {cost} gold each?", InventorySlot.GetStack().quantity);
    }

    void confirmSellAction(int quantity)
    {
        ShopSellMenuController s = (ShopSellMenuController)MenuController;
        
        InventorySlot.Discard(quantity);
        s.SellAmmount(quantity, cost); //Gain Money

        if (InventorySlot.isEmpty()) //if inventory slot is now empty
        {            
            s.SellItem(this.gameObject);  //destroy this object
        }
        else
        {
            UpdateAmmount(); //or else simply update the ammount displayed
        }

       
    }
}
