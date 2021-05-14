using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMapTile : Tile
{
    public int roomNumber;

    public enum RoomFeature
    {
        Entrance,
        Exit,
        LockedExit,
        Fountain,
        Boss,
        Key,
        Shop,
        EndGame,
        None
    }
    public RoomFeature roomFeature;

    public RoomMapTile(bool occupied, int x, int y) : base(occupied, x, y)
    {
        this.occupied = occupied;
        this.x = x;
        this.y = y;
        roomNumber = 0;
        facing = Facing.none;
        roomFeature = RoomFeature.None;

        north = null;
        south = null;
        west = null;
        east = null;
    }

    public RoomMapTile(Tile t)
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

        roomNumber = 0;
        roomFeature = RoomFeature.None;
    }
}
