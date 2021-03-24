using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item
{
    public Global.GearType GearType;
  //  public int DefenceBonus;
  //  public int ManaBonus;
    public List<int> Boosts;

    public EquipableItem() 
    {
        Boosts = new List<int>();
    }
}
