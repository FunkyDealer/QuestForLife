using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillMapTile : Tile
{

    public FillMapTile(bool occupied, int x, int y) : base(occupied, x, y)
    {
        this.occupied = occupied;
        this.x = x;
        this.y = y;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;
    }

    public FillMapTile(Tile t)
    {
        this.floor = t.floor;
        this.occupied = t.occupied;
        this.x = t.x;
        this.y = t.y;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;
    }



}
