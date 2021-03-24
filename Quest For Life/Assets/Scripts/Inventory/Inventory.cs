using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    InventorySlot[] slots;


    public Inventory(int capacity)
    {
        slots = new InventorySlot[capacity];
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot(i);
            //slots[i].GetID(i);
        }

    }

    public bool TryToAddToInventory(Item i, int Quantity) //Major Function that tries to add item to inventory
    {
        if (i is HealItem) //If the Item is an Heal Item
        {
            InventorySlot s = areThereAvailableSlotsForConsumable((HealItem)i, Quantity); //Check if there are slots where the total quantity can be put in one go
            if (s != null) {
                s.AddQuantity(Quantity); //Put in slot in one go
                return true;
            }
            else
            {
               if (TryToDistributeItemStack((HealItem) i, Quantity)) //Try to distribute the quantity between various slots that have that item
                {
                    return true;
                } 
               else
                {
                    if (areThereEmptySlots()) //if all else fails, try to add item to a brand new slot(if there is one open)
                    {
                        AddNewItem(i, Quantity);
                        return true;
                    }
                }
            }
        }
        else if (i is EquipableItem) //Else, if the item is an Equipable item
        {
            if (areThereEmptySlots()) //Equipable items cannot stack, so it must go to a new Slot
            {
                AddNewItem(i, Quantity);
                return true;
            }
        }

        return false; //if everything fails, returns that the Inventory
    }

    InventorySlot areThereAvailableSlotsForConsumable(HealItem i, int Quantity) //Checks if there is a slot where an healItem can be placed in One go
    {
        foreach (var s in slots)
        {
            if (!s.isEmpty() && s.getItem().ID == i.ID)
            {
                if (s.IsFull()) continue;

                if (s.LeftQuantity() >= Quantity)
                {
                    return s;
                }
            }
        }
        return null;
    }

    bool TryToDistributeItemStack(HealItem i, int Quantity) //tries to destribute quantity between slots with the same item that aren't already full
    {
        List<InventorySlot> availableSlots = new List<InventorySlot>();
        int totalAmmountAvailable = 0; //Available quantity combined from all the slots with this item

        foreach (var s in slots)
        {
            if (!s.isEmpty() && s.getItem().ID == i.ID)
            {
                if (s.IsFull()) continue;

                availableSlots.Add(s);
                totalAmmountAvailable += s.LeftQuantity();
            }
        }

        int leftQuantity = Quantity;
        if (totalAmmountAvailable >= leftQuantity)
        {
            foreach (var s in availableSlots)
            {
                if (leftQuantity > 0)
                {
                    if (leftQuantity > s.LeftQuantity())
                    {
                        int quantityPut = s.LeftQuantity();
                        s.AddQuantity(quantityPut);
                        leftQuantity -= quantityPut;
                        continue; 
                    }
                    else if (leftQuantity <= s.LeftQuantity())
                    {
                        s.AddQuantity(leftQuantity);
                        return true;
                    }
                }                
            }
        }
        return false;
    }

    bool AddNewItem(Item i, int Quantity)
    {
        bool success = false;

        foreach (var s in slots)
        {
            if (s.isEmpty())
            {
                ItemStack IS = new ItemStack(i, Quantity, s);
                s.PlaceItem(IS);
                return true;
            }
        }

        return success;
    }


    public InventorySlot getSlot(int i)
    {
        return slots[i];
    }

    bool isFull()
    {
        int fullSlots = 0;
        foreach (var s in slots)
        {
            if (!s.isEmpty()) fullSlots++;
        }

        return fullSlots == slots.Length;
    }

    bool areThereEmptySlots()
    {
        foreach (var s in slots)
        {
            if (s.isEmpty()) return true;
        }

        return false;
    }



    
}
