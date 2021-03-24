using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// Inventory Slot Data type, contains a Item stack and is contained in the Inventory
/// 
/// </summary>
public class InventorySlot
{
    ItemStack currentStack;
    int id;

    public delegate void NewItem(int id);
    public static event NewItem onSlotNewItem;

    public InventorySlot(int id)
    {
        this.currentStack = null;
        this.id = id;
    }

    public void PlaceItem(ItemStack i) //Place a Stack in the this Slot
    {
        this.currentStack = i;
        onSlotNewItem(id);
    }

    public void AddQuantity(int Quantity)
    {
        currentStack.quantity += Quantity;
        if (currentStack.quantity > 99) currentStack.quantity = 99;
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

        onSlotNewItem(i.id);

        this.currentStack = null;
    }

    public void Discard()
    {

    }


    public Item getItem()
    {
        if (currentStack.item != null) return currentStack.item;
        else return null;
    }

    public void GetID(int i)
    {
        this.id = i;
    }

    public bool IsFull()
    {
        if (isEmpty()) return false;
        else return currentStack.quantity == 99;
    }

    public int LeftQuantity()
    {
        return 99 - currentStack.quantity;
    }

}
