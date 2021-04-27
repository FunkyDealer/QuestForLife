using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallMapTile : Tile
{


    public int hallNumber;

    public HallMapTile(bool occupied, int x, int y) : base(occupied, x, y)
    {
        this.occupied = occupied;
        this.x = x;
        this.y = y;
        hallNumber = 0;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;

    }

    public HallMapTile(Tile t)
    {
        this.occupied = t.occupied;
        this.x = t.x;
        this.y = t.y;
        facing = t.facing;

        north = t.north;
        south = t.south;
        west = t.west;
        east = t.east;

        hallNumber = 0;
    }



}
