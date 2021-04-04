using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Controller for the Menu that appears when clicking the individual Inventory Slot
/// 
/// </summary>
public class SlotManagementMenu : ItemManagementMenu
{

    [SerializeField]
    Text UseButtonText;

    bool Consumable = false;

    // Start is called before the first frame update
    void Start()
    {
        if (inventorySlotIF.getItem() is HealItem)
        {
            Consumable = true;
            UseButtonText.text = "Use";
        }


    }

    void OnDestroy()
    {
       // inventoryIFManager.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useButton()
    {
        if (Consumable)
        {
            UseItem();
        }
        else
        {
            EquipItem();
        }

    }

    void UseItem()
    {
        if (inventoryIFManager.ConsumeItem(inventorySlotIF.ID))
        {
            CloseMenu(); //success, close menu
        }
        else
        {
            //Warn the player that the action couldn't be completed


        }


    }

    void EquipItem()
    {
        if (inventoryIFManager.EquipItem(inventorySlotIF.ID))
        {
            CloseMenu();
        }
        else
        {
            //Warn the player that the action couldn't be completed


        }


    }

    public void MoveStack()
    {
        inventoryIFManager.InitiateItemSwitch(inventorySlotIF.ID);

        CloseMenu();
    }

    public void DiscardStack()
    {
        inventorySlotIF.DiscardItem();
        CloseMenu();
    }

    void CloseMenu()
    {
        inventoryIFManager.CloseAllMenus();
    }

}
