using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    InventorySlot[] slots;
    public InventorySlot[] Slots => slots;
    Player player;

    public Inventory(int capacity, Player p)
    {
        this.player = p;
        slots = new InventorySlot[capacity];
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new InventorySlot(i);
            //slots[i].GetID(i);
        }
    }

    public bool tryToSwitchToSlot(int Origin, int target) //Major function that Tries to Move Item Quantities to Another slot
    {
        if (slots[Origin].getItem() is HealItem) //if the Item is an Consumable
        {
            if (slots[target].isEmpty()) //If the Target is completely Empty, Place All of Origin Slot on the new Slot
            {
                slots[Origin].ChangeStackTo(slots[target]);

                return true;
            }
            else
            {
                if (slots[target].getItem().ID == slots[Origin].getItem().ID && !slots[target].IsFull()) //Else if the target is not empty, but not full either
                {
                    int quantityPut = slots[Origin].PlaceQuantityTo(slots[target]); //Try to place as much quantity as it can on the target

                    return true;  //quantity Put is the ammount that was put, if all quantity was put, the origin slots will now be empty
                }
            }
        }
        else if (slots[Origin].getItem() is EquipableItem) //if the origin's item is a Equipable item (That can't stack)
        {
            if (slots[target].isEmpty()) //it can only be switched to slots that are empty
            {                
                slots[Origin].ChangeStackTo(slots[target]);
                return true;
            }
        }    
        return false; //if everything fails, Send that the switch was a fail
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

    public bool ConsumeItem(int slot) //tried to consume item
    {
        HealItem i = (HealItem)slots[slot].getItem();

        switch (i.HealType)
        {
            case Global.HealType.HEALTH:
                if (player.currentHealth < player.maxHealth) //if current health is not maxed
                {
                    player.HealHealth(i.HealAmmount);
                    slots[slot].ConsumeOne();
                    return true; //success
                }
                break;
            case Global.HealType.MANA:
                if (player.currentMana < player.maxMana) //if current mana is not maxed
                {
                    player.RestoreMana(i.HealAmmount);
                    slots[slot].ConsumeOne();
                    return true; //success
                }
                break;
            case Global.HealType.BOTH:
                if (player.currentMana < player.maxMana || player.currentHealth < player.maxHealth) //if current mana or health are not maxed
                {
                    player.RestoreHealthAndMana(i.HealAmmount);
                    slots[slot].ConsumeOne();
                    return true; //success
                }
                break;
            default:
                break;
        }


        return false; //Failed at consuming the item
    }

    public bool canConsumeItem(int slot) //returns wether or not item can be consumed
    {
        HealItem i = (HealItem)slots[slot].getItem();

        switch (i.HealType)
        {
            case Global.HealType.HEALTH:
                if (player.currentHealth < player.maxHealth) //if current health is not maxed
                {
                    return true; //success
                }
                break;
            case Global.HealType.MANA:
                if (player.currentMana < player.maxMana) //if current mana is not maxed
                {
                    return true; //success
                }
                break;
            case Global.HealType.BOTH:
                if (player.currentMana < player.maxMana || player.currentHealth < player.maxHealth) //if current mana or health are not maxed
                {
                    return true; //success
                }
                break;
            default:
                break;
        }
        return false; //cannot consume Item
    }

}
