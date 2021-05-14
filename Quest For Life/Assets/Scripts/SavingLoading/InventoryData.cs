using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int HatSlot;
    public int BodySlot;
    public int BeltSlot;
    public int RingSlot1;
    public int RingSlot2;
    public int WeaponSlot;

    public int[] inventoryIds;
    public int[] inventoryQt;
    public int[] inventoryType;

    public InventoryData(Player player)
    {
        inventoryIds = new int[10];
        inventoryQt = new int[10];
        inventoryType = new int[10];


        for (int i = 0; i < player.Inventory.Slots.Length; i++)
        {
            if (player.Inventory.Slots[i].GetStack() != null)
            {
                inventoryIds[i] = player.Inventory.Slots[i].getItem().ID;
                inventoryQt[i] = player.Inventory.Slots[i].GetStack().quantity;

                if (player.Inventory.Slots[i].getItem() is HealItem) inventoryType[i] = 0;
                else if (player.Inventory.Slots[i].getItem() is EquipableItem) inventoryType[i] = 1;
            }
            else
            {
                inventoryIds[i] = -1;
                inventoryQt[i] = -1;
                inventoryType[i] = -1;
            }
        }

        if (!player.HatSlot.isEmpty()) HatSlot = player.HatSlot.GetItem().ID;
        else HatSlot = -1;

        if (!player.BodySlot.isEmpty()) BodySlot = player.BodySlot.GetItem().ID;
        else BodySlot = -1;

        if (!player.BeltSlot.isEmpty()) BeltSlot = player.BeltSlot.GetItem().ID;
        else BeltSlot = -1;

        if (!player.RingSlot1.isEmpty()) RingSlot1 = player.RingSlot1.GetItem().ID;
        else RingSlot1 = -1;

        if (!player.RingSlot2.isEmpty()) RingSlot2 = player.RingSlot2.GetItem().ID;
        else RingSlot2 = -1;

        if (!player.WeaponSlot.isEmpty()) WeaponSlot = player.WeaponSlot.GetItem().ID;
        else WeaponSlot = -1;

    }


}
