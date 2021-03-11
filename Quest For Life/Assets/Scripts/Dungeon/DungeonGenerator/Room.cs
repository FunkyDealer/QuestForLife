using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public int x, y, width, height; //Position and dimension of the room
    public int left, right;
    public int top, bottom;
    public int number;
    public bool shop;
    public bool chest;
    public bool key;

    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        left = x;
        right = x + width;
        top = y;
        bottom = y + height;
    }




}
