using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// Inventory Slot Data type, contains a Item stack and is contained in the Inventory
/// 
/// </summary>
public class InventorySlot : Slot
{
    ItemStack currentStack;

    public delegate void NewItem(int id);
    public static event NewItem onSlotNewItem;

    public delegate void ClearItem(int id);
    public static event ClearItem onSlotClearItem;

    public delegate void UpdateItem(int id, int newQuantity);
    public static event UpdateItem onSlotUpdateItem;

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
        onSlotUpdateItem(id, currentStack.quantity);

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

        onSlotClearItem(id);
        onSlotNewItem(i.id);

        this.currentStack = null;
    }

    public int PlaceQuantityTo(InventorySlot target)
    {
        int spaceAvailable = target.LeftQuantity();

        if (currentStack.quantity <= target.LeftQuantity())
        {
            int quantityPut = currentStack.quantity;
            target.AddQuantity(currentStack.quantity);

            Discard();

            return quantityPut;
        }
        else
        {
            int quantityThatCanBePut = target.LeftQuantity();
            target.AddQuantity(quantityThatCanBePut);
            currentStack.quantity -= quantityThatCanBePut;

            onSlotUpdateItem(id, CurrentQuantity());

            return quantityThatCanBePut;
        }
        return 0;
    }
     

    public Item getItem() //get the Item type in this slot
    {
        if (currentStack != null) return currentStack.item;
        else return null;
    }

    public ItemStack GetStack()
    {
        if (currentStack != null) return currentStack;
        else return null;
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
       

    public void ConsumeOne() //Consume 1 of the item
    {
        currentStack.quantity -= 1;
        if (currentStack.quantity <= 0)
        {
            Discard();
        }
        else
        {
            onSlotUpdateItem(id, currentStack.quantity);
        }
    }

    public void Discard() //discard item, removing it
    {
        this.currentStack = null;
        onSlotClearItem(id);
    }

    public void Discard(int Quantity)
    {
        this.currentStack.quantity -= Quantity;
        if (currentStack.quantity <= 0)
        {
            Discard();
        } else
        {
            onSlotUpdateItem(id, currentStack.quantity);
        }
    }

}
