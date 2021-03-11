using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenPreset
{
    public int MAX_LEAF_SIZE;
    public int MIN_LEAF_SIZE;
    public int WIDTH;
    public int LENGTH;

    public DungeonGenPreset(int MAX_LEAF_SIZE, int MIN_LEAF_SIZE, int WIDTH, int LENGTH)
    {
        this.MAX_LEAF_SIZE = MAX_LEAF_SIZE;
        this.MIN_LEAF_SIZE = MIN_LEAF_SIZE;
        this.WIDTH = WIDTH;
        this.LENGTH = LENGTH;
    }
}
