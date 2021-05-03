﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum Facing
    {
        north,
        west,
        east,
        south,
        none
    }

    public Facing facing;

    public bool occupied;

    public int x;
    public int y;
    public Tile north;
    public Tile south;
    public Tile west;
    public Tile east;

    public int floor;

    public bool explored;

    public Tile()
    {
        this.occupied = false;
        this.x = 0;
        this.y = 0;
        facing = Facing.none;

        north = null;
        south = null;
        west = null;
        east = null;
    }

    public Tile(bool occupied, int x, int y)
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

    public Tile(Tile t)
    {
        this.occupied = t.occupied;
        this.x = t.x;
        this.y = t.y;
        facing = t.facing;

        north = t.north;
        south = t.south;
        west = t.west;
        east = t.east;
    }

    public void createConnections(Tile[,] map, int width, int length)
    {
        if (x > 0) north = map[x - 1, y];
        if (x < length) south = map[x + 1, y];
        if (y > 0) west = map[x - 1, y];
        if (y < width) east = map[x + 1, y];
    }

    public Global.FacingDirection OppositeDirection()
    {
        switch (facing)
        {
            case Facing.north:
                return Global.FacingDirection.SOUTH;
            case Facing.east:
                return Global.FacingDirection.WEST;
            case Facing.west:
                return Global.FacingDirection.EAST;
            case Facing.south:
                return Global.FacingDirection.NORTH;
        }
        return Global.FacingDirection.NORTH;
    }
}