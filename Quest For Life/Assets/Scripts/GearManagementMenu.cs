using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManagementMenu : MonoBehaviour
{
    [HideInInspector]
    public GearSlotIF gearSlot;
    [HideInInspector]
    public InventoryIFManager inventoryIFManager;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unequipButton()
    {
        if (gearSlot.UnequipItem())
        {
            CloseMenu(); //success, close menu
        } else
        {
            //warn player
        }



    }

    void CloseMenu()
    {
        inventoryIFManager.CloseAllMenus();
    }

}
