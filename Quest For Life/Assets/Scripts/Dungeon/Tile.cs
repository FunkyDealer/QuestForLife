using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum Type
    {
        room,
        hall,
        wall,
        filling,
        none
    }

    public enum Feature
    {
        Entrance,
        Exit,
        LockedExit,
        Shop,
        Fountain,
        Chest,
        Boss,
        Key,
        None
    }

    public enum Facing
    {
        north,
        west,
        east,
        south,
        none
    }

    public Feature feature;
    public Type type;
    public Facing facing;

    public bool occupied;

    public int roomNumber;
    public int hallNumber;
    public int x;
    public int y;
    public Tile north;
    public Tile south;
    public Tile west;
    public Tile east;

    public Chest chest;

    public Tile(bool occupied, int x, int y)
    {
        this.occupied = occupied;
        this.x = x;
        this.y = y;
        roomNumber = 0;
        hallNumber = 0;
        feature = Feature.None;
        type = Type.filling;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;
        
    }

    public void createConnections(Tile[,] map, int width, int length)
    {
        if (x > 0) north = map[x - 1, y];
        if (x < length) south = map[x + 1, y];
        if (y > 0) west = map[x - 1, y];
        if (y < width) east = map[x + 1, y];

    }
}
