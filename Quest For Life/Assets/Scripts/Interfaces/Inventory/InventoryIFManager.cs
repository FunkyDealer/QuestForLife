using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// manager for the Inventory Interface, controls the Inventory Window
/// 
/// </summary>
public class InventoryIFManager : MonoBehaviour
{
    
    [SerializeField]
    NavigationInterfaceManager navigationInterface;

    [SerializeField]
    List<InventorySlotIF> interfaceSlots;

   // Inventory inventory;
    public Inventory Inventory => navigationInterface.player.Inventory;

    [HideInInspector]
    public List<GameObject> menus;

    int? SlotBeingSwitched = null;

    void Awake()
    {
        menus = new List<GameObject>();
        

    }

    // Start is called before the first frame update
    void Start()
    {

      // inventory = navigationInterface.player.Inventory;


    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public void CloseAllMenus()
    {
        foreach (var m in menus) Destroy(m);

        menus.Clear();
    }


    public void InitiateItemSwitch(int OriginSlot)
    {
        SlotBeingSwitched = OriginSlot;

        foreach (var s in interfaceSlots)
        {
            s.StartItemSwitch();
        }
    }

    public void StopItemSwitch()
    {
        if (SlotBeingSwitched != null)
        {
            SlotBeingSwitched = null;

            foreach (var s in interfaceSlots)
            {
                s.EndItemSwitch();
            }
        }
    }

    public void PerformItemSwitch(int targetSlot)
    {
        if (SlotBeingSwitched == targetSlot) return; //if the Origin and Target are the same, stop and return
        
        bool success = Inventory.tryToSwitchToSlot((int)SlotBeingSwitched, targetSlot); //Try to make the switch

        if (success) StopItemSwitch();


        
    }

}
