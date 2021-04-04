using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItemManagementMenu : ItemManagementMenu
{
    

    public void useButton()
    {
        if (inventoryIFManager.canConsumeItem(inventorySlotIF.ID))
        {
            inventoryIFManager.BattleUseItem(inventorySlotIF.ID); //send item to be used
            CloseMenu(); //success, close menu
        }
        else
        {
            //Warn the player that the action couldn't be completed


        }
    }    

    public void CancelButton()
    {
        CloseMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CloseMenu()
    {
        inventoryIFManager.CloseAllMenus();
    }

    
}
