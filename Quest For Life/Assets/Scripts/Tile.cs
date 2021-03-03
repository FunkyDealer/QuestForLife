using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public enum Flag
    {
        spawn,
        stairs,
        none          
    }

    public bool Occupied;
    Flag flag;


    public Tile(bool occupied)
    {
        this.Occupied = occupied;
        flag = Flag.none;
    }


    public void getFlag(Flag f)
    {
        this.flag = f;
    }
}
