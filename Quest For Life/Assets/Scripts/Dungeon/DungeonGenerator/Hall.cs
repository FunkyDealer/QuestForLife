using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall
{
    public int x, y, width, height; //Position and dimension of the room
    public int left, right;
    public int top, bottom;
    public Room leftRoom;
    public Room rightRoom;

    public Hall(int x, int y, int width, int height, Room l, Room r)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        left = x;
        right = x + width;
        top = y;
        bottom = y + height;
        this.leftRoom = l;
        this.rightRoom = r;

    }


}
