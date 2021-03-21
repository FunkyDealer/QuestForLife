using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    public int HealAmmount;
    public Global.HealType HealType;

    public HealItem(int heal, int id)
    {
        HealAmmount = heal;
        ID = id;
    }

}
