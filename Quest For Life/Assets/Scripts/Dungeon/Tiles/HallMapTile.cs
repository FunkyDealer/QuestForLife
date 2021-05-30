using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallMapTile : Tile
{


    public int hallNumber;

    public HallMapTile(bool occupied, int x, int y, int hallNumber) : base(occupied, x, y)
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

        this.hallNumber = hallNumber;
    }

    public HallMapTile(Tile t, int hallNumber)
    {
        this.floor = t.floor;
        this.occupied = t.occupied;
        this.x = t.x;
        this.y = t.y;
        facing = t.facing;

        north = t.north;
        south = t.south;
        west = t.west;
        east = t.east;

        this.hallNumber = hallNumber;
    }



}
