using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemDisplayController : ShopItemDisplayController
{
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init(EquipableItem item, int slot, ShopManager m, ShopBuyMenuController buyMenu)
    {
        base.Init(item, slot, m, buyMenu);
    }

    public override void Init(HealItem item, int slot, ShopManager m, ShopBuyMenuController buyMenu)
    {
        base.Init(item, slot, m, buyMenu);
    }    

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
       // Debug.Log("Mouse is no longer on GameObject.");
    }


    public void BuyItem()
    {
        if (MenuController.player.currentGold >= item.Cost)
        {
            MenuController.confirmation();
            GameObject o = Instantiate(confirmPrompt);
            GenericConfirmInterface g = o.GetComponent<GenericConfirmInterface>();
            g.Init(confirmBuyAction, MenuController.manager.unLockInterface, $"Are you sure you want to buy {item.Name} for {item.Cost} gold?");
        }
    }

    void confirmBuyAction()
    {
        if (shopManager.BuyItem(item, slot, MenuController.player))
        {
            ShopBuyMenuController s = (ShopBuyMenuController)MenuController;
            s.BuyItem(this.gameObject); //sucefully buy
        }
        else
        { //warn player that buying Failed

        }
    }
}
