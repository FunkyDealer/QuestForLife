using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public int consumableNumber;
    public int equipableNumber;
    public int[] consumables;
    public int[] equipables;

    public ShopData(Shop shop)
    {
        consumableNumber = 0;

        List<int> consumablesList = new List<int>();

        foreach (var s in shop.ConsumableInventory)
        {
            if (s != null)
            {
                consumablesList.Add(s.ID);
                consumableNumber++;
            }
        }
        
        consumables = new int[consumableNumber];
        if (consumableNumber > 0) for (int i = 0; i < consumablesList.Count; i++) consumables[i] = consumablesList[i];

        equipableNumber = 0;
        List<int> equipablesList = new List<int>();

        foreach (var s in shop.EquipableInventory)
        {
            if (s != null)
            {
                equipablesList.Add(s.ID);
                equipableNumber++;
            }
        }

        equipables = new int[equipableNumber];
        if (equipableNumber > 0) for (int i = 0; i < equipablesList.Count; i++) equipables[i] = equipablesList[i];
    }


}
