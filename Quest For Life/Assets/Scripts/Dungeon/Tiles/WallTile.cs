using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : Tile
{
    public enum WallFeature
    {
        ShopEntrance,
        Chest,
        ShopExit,
        None
    }
    public WallFeature wallFeature;

    public bool Room;

    public enum WallType
    {
        Wall,
        Corner,
        DoubleSided
    }
    public WallType wallType;

    public WallTile(bool occupied, int x, int y, bool room) : base(occupied, x, y)
    {
        this.occupied = occupied;
        this.x = x;
        this.y = y;
        wallFeature = WallFeature.None;
        facing = Facing.none;
        this.Room = room;

        north = null;
        south = null;
        west = null;
        east = null;
    }

    public WallTile(Tile t, bool room)
    {
        this.occupied = t.occupied;
        this.x = t.x;
        this.y = t.y;
        wallFeature = WallFeature.None;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;
        this.Room = room;
    }


}
