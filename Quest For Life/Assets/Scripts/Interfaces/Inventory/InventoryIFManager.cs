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

    //[SerializeField]
    //List<>

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

    public bool EquipItem(int OriginSlot)
    {
        EquipableItem e = (EquipableItem)navigationInterface.player.Inventory.getSlot(OriginSlot).getItem();

        if (TryToEquip(e))
        {
            //Success
            navigationInterface.player.Inventory.getSlot(OriginSlot).Discard();
            return true;
        }       


        return false;
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

    public bool ConsumeItem(int slot)
    {
        return Inventory.ConsumeItem(slot);

        return false;
    }

    public bool UnequipItem(int ID)
    {
        GearSlot slot = GetGearSlot(ID);

        if (slot.unequip(navigationInterface.player.Inventory)) return true;

        return false;
    }

    public bool hasGear(int ID)
    {
        GearSlot slot = GetGearSlot(ID);

        return !slot.isEmpty();
    }

     public GearSlot GetGearSlot(int id)
    {
        switch (id)
        {
            case 1:
                return navigationInterface.player.HatSlot;
            case 2:
                return navigationInterface.player.BodySlot;
            case 3:
                return navigationInterface.player.RingSlot1;
            case 4:
                return navigationInterface.player.RingSlot2;
            case 5:
                return navigationInterface.player.BeltSlot;
            default:
                return navigationInterface.player.WeaponSlot;
        }
    }

    public bool TryToEquip(EquipableItem i)
    {
        switch (i.GearType)
        {
            case Global.GearType.HAT:
                return navigationInterface.player.HatSlot.AttemptToPlaceItem(i);
            case Global.GearType.RING:
                bool Success = navigationInterface.player.RingSlot1.AttemptToPlaceItem(i);
                if (!Success) return navigationInterface.player.RingSlot2.AttemptToPlaceItem(i);
                else return true;
            case Global.GearType.BODYCLOTHING:
                return navigationInterface.player.BodySlot.AttemptToPlaceItem(i);
            case Global.GearType.BELT:
                return navigationInterface.player.BeltSlot.AttemptToPlaceItem(i);              
            default:
                return navigationInterface.player.WeaponSlot.AttemptToPlaceItem(i);
        }
    }

    
}
