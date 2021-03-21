using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    ItemStack currentStack;

    public InventorySlot()
    {
        this.currentStack = null;
    }

    public void PlaceItem(ItemStack i) //Place a Stack in the this Slot
    {
        this.currentStack = i;
    }

    public ItemStack TakeItem() //Take the Stack From the this Slot
    {
        if (currentStack == null) return null;
        else
        {
            ItemStack i = currentStack;
            this.currentStack = null;
            return i;
        }
    }

    public bool isEmpty() //Check if the Slot is Empty
    {
        return currentStack == null;
    }

    public int CurrentQuantity() //Check the quantity of items in this Slot
    {
        if (currentStack == null) return 0;
        else return currentStack.quantity;
    }

    public bool MoreThanOne() //check if there is more than one Item in the Slot's Stack
    {
        return currentStack.quantity > 1;
    }

    public void ChangeStackTo(InventorySlot i) //Change the stack to another Slot
    {
        ItemStack s = currentStack;
        s.currentSlot = i;
        i.currentStack = s;
        this.currentStack = null;
    }


}
