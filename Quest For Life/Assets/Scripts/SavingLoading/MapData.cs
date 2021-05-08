using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public int floor;
    public int Height;
    public int Width;
    public int[] seeds;
    public int[] openChests;


    public MapData(int floor, int Height, int Width, int[] seeds, int[] openChests)
    {
        this.floor = floor;
        this.Height = Height;
        this.Width = Width;
        this.seeds = seeds;
        this.openChests = openChests;
    }



}
