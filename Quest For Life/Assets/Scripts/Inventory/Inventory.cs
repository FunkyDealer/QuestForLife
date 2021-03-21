using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    InventorySlot[] Slots;

    public Inventory(int capacity)
    {
        Slots = new InventorySlot[capacity];

    }

    public void AddNewItem(Item i, int Quantity)
    {
        int n = 0;
        foreach (var s in Slots)
        {
            if (s.isEmpty())
            {
                ItemStack IS = new ItemStack(i, Quantity, s);
                s.PlaceItem(IS);
            }
        }
    }


    public InventorySlot getSlot(int i)
    {
        return Slots[i];
    }


    public bool isFull()
    {
        int fullSlots = 0;
        foreach (var s in Slots)
        {
            if (!s.isEmpty()) fullSlots++;
        }

        return fullSlots == Slots.Length;
    }



    
}
