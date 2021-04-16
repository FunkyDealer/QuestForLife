﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
    EquipableItem[] equipableInventory;
    public EquipableItem[] EquipableInventory => equipableInventory;
    HealItem[] consumableInventory;
    public HealItem[] ConsumableInventory => consumableInventory;


    public Shop(int EquipableNum, int ConsumableNum)
    {
        equipableInventory = new EquipableItem[EquipableNum];
        consumableInventory = new HealItem[ConsumableNum];

    }

    public void RestockInventory(int floor)
    {
        List<HealItem> availableHealItems = new List<HealItem>(); //List of available heal Items

        foreach (var h in DataBase.inst.Consumables.Values) //add from database
        {
            if (h.buyable && floor >= h.MinLevel && floor <= h.MaxLevel) availableHealItems.Add(h);
        }


        List<EquipableItem> availableEquipItem = new List<EquipableItem>(); //list of available equipItems

        foreach (var h in DataBase.inst.Gears.Values) //add from database
        {
            if (h.buyable && floor >= h.MinLevel && floor <= h.MaxLevel) availableEquipItem.Add(h);
        }

        int healItemCount = availableHealItems.Count;
        Debug.Log("Heal Items available for sale: " + healItemCount);
        if (healItemCount > 0)
        {
            for (int i = 0; i < consumableInventory.Length; i++) //fill consumable inventory
            {
                int r = Random.Range(0, healItemCount); //choose a random consumable to put in inventory
                consumableInventory[i] = availableHealItems[r];
            }
        }

        int equipItemCount = availableEquipItem.Count;
        Debug.Log("equipament Items available for sale: " + equipItemCount);
        if (equipItemCount > 0) {

            for (int i = 0; i < equipableInventory.Length; i++) //fill the equipable inventory
            {
                int r = Random.Range(0, equipItemCount); //choose a random equipable
                equipableInventory[i] = availableEquipItem[r];
            }

        }

    }

    public bool buyItem(Item item, int slot, Player player)
    {
        Debug.Log("entered");
        if (item is HealItem)
        {          
            if (player.Inventory.TryToAddToInventory(item, 1)) //try to add to inventory
            {

                consumableInventory[slot] = null;
                player.spendGold(item.Cost);
                return true;
            }
        }
        else if (item is EquipableItem)
        {
            if (player.Inventory.TryToAddToInventory(item, 1)) //try to add to inventory
            {
                equipableInventory[slot] = null;
                player.spendGold(item.Cost);
                return true;
            }
        }



        return false;
    }


}
