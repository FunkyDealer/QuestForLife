using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// manager for the Inventory Interface, controls the Inventory Window
/// 
/// </summary>
public class InventoryIFManager : MonoBehaviour
{    
    [SerializeField]
    HudManager HudManager;

    [SerializeField]
    List<InventorySlotIF> interfaceSlots;

   // Inventory inventory;
    public Inventory Inventory => HudManager.player.Inventory;

    [HideInInspector]
    public List<GameObject> menus;

    //[SerializeField]
    //List<>

    int? SlotBeingSwitched = null;

    [SerializeField]
    Text goldAmmountDisplay;

    void Awake()
    {
        menus = new List<GameObject>();

        Player.onGoldChange += updateGoldAmmount;
    }

    // Start is called before the first frame update
    void Start()
    {

        // inventory = navigationInterface.player.Inventory;
        if (goldAmmountDisplay != null)
        goldAmmountDisplay.text = HudManager.player.currentGold.ToString();

    }

    void OnDestroy()
    {
        Player.onGoldChange -= updateGoldAmmount;
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    void OnDisable()
    {
        CloseAllMenus();
    }

    public void CloseAllMenus()
    {
        foreach (var m in menus) Destroy(m);

        menus.Clear();
    }

    public bool EquipItem(int OriginSlot)
    {
        EquipableItem e = (EquipableItem)HudManager.player.Inventory.getSlot(OriginSlot).getItem();

        if (TryToEquip(e))
        {
            //Success
            HudManager.player.Inventory.getSlot(OriginSlot).Discard();
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

    public bool canConsumeItem(int slot)
    {
        return Inventory.canConsumeItem(slot);

        return false;
    }

    public void BattleUseItem(int slot)
    {
        HudManager.UseItemAction(slot);       
    }



    public bool UnequipItem(int ID)
    {
        GearSlot slot = GetGearSlot(ID);

        if (slot.unequip(HudManager.player.Inventory)) return true;

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
                return HudManager.player.HatSlot;
            case 2:
                return HudManager.player.BodySlot;
            case 3:
                return HudManager.player.RingSlot1;
            case 4:
                return HudManager.player.RingSlot2;
            case 5:
                return HudManager.player.BeltSlot;
            default:
                return HudManager.player.WeaponSlot;
        }
    }

    public bool TryToEquip(EquipableItem i)
    {
        switch (i.GearType)
        {
            case Global.GearType.HAT:
                return HudManager.player.HatSlot.AttemptToPlaceItem(i);
            case Global.GearType.RING:
                bool Success = HudManager.player.RingSlot1.AttemptToPlaceItem(i);
                if (!Success) return HudManager.player.RingSlot2.AttemptToPlaceItem(i);
                else return true;
            case Global.GearType.BODYCLOTHING:
                return HudManager.player.BodySlot.AttemptToPlaceItem(i);
            case Global.GearType.BELT:
                return HudManager.player.BeltSlot.AttemptToPlaceItem(i);              
            default:
                return HudManager.player.WeaponSlot.AttemptToPlaceItem(i);
        }
    }

    void updateGoldAmmount(int newAmmount)
    {
        if (goldAmmountDisplay != null)
        goldAmmountDisplay.text = newAmmount.ToString() + "g";
    }

}
