using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack 
{
    public Item item;
    public int quantity;
    public InventorySlot currentSlot;


    public ItemStack(Item i, int Quantity, InventorySlot slot)
    {
        this.item = i;
        this.quantity = Quantity;
        this.currentSlot = slot;
    }


}
