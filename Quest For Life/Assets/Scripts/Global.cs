using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global 
{
    public enum Type
    {
        FIRE,
        WATER,
        THUNDER,
        LIGHT,
        DARKNESS,
        NONE
    }

    public enum FacingDirection
    {
        NORTH,
        EAST,
        WEST,
        SOUTH
    }

    public enum AttackEffect
    {
        HEAL,
        DEATH,
        NONE
    }

   public enum HealType
    {
        HEALTH,
        MANA
    }

    public enum GearType
    {
        HAT,
        RING,
        BODYCLOTHING,
        BELT       
    }


}
