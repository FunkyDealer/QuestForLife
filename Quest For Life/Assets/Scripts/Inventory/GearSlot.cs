using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSlot : Slot
{
    EquipableItem item;

    Global.GearType type;
    Player player;
    
    public delegate void NewItem(int id);
    public static event NewItem onSlotNewItem;

    public delegate void ClearItem(int id);
    public static event ClearItem onSlotClearItem;

    public GearSlot(Global.GearType type, int ID, Player player)
    {
        this.item = null;
        this.type = type;
        this.id = ID;
        this.player = player;
    }

    void placeItem(EquipableItem i) //Place a Stack in the this Slot
    {
        this.item = i;
        player.AddStatsFromItem(item);        
        onSlotNewItem(id);
    }

    public bool isEmpty() //Check if the Slot is Empty
    {
        return item == null;
    }

    public void RemoveItem()
    {
        player.RemoveStatsFromItem(item);
        item = null;        
        onSlotClearItem(id);
    }

    public Global.GearType getType() => type;

    public bool AttemptToPlaceItem(EquipableItem i)
    {
        if (isEmpty() && i.GearType == getType())
        {
            placeItem(i);            
            return true;
        }

        return false;
    }

    bool TryToSwitchItem(InventorySlot i)
    {
        if (i.isEmpty()) {
            ItemStack s = new ItemStack(item, 1, i);
            i.PlaceItem(s);
            RemoveItem();
            return true;
         }

        return false;
    }

    public bool unequip(Inventory i)
    {
        if (i.TryToAddToInventory(item, 1))
        {
            RemoveItem();
            return true;
        }



        return false;
    }

    public Item GetItem() => item;

}
