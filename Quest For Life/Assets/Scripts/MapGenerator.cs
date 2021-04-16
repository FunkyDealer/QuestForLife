using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    protected int mapWidth;
    protected int mapLength;

    [SerializeField]
    protected GameObject FloorTileObj; //Tiles for floor
    [SerializeField]
    protected GameObject RoofTileObj; //Tiles for Roof

    [HideInInspector]
    public Tile[,] map;

    [SerializeField]
    protected bool parent = true;

    protected Vector2 spawn;

    [HideInInspector]
    public MapManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void rotateObj(GameObject o, Tile.Facing facing)
    {
        switch (facing)
        {
            case Tile.Facing.north:
                o.transform.Rotate(0, -90, 0);
                break;
            case Tile.Facing.west:
                o.transform.Rotate(0, 180, 0);
                break;
            case Tile.Facing.east:
                o.transform.Rotate(0, 0, 0);
                break;
            case Tile.Facing.south:
                o.transform.Rotate(0, 90, 0);
                break;
            case Tile.Facing.none:
                o.transform.Rotate(0, 0, 0);
                break;
        }
    }

    protected GameObject InstantiateObj(GameObject o, Vector3 pos)
    {
        if (parent)
        {
            return Instantiate(o, pos, o.transform.rotation, this.gameObject.transform);
        }
        else
        {
            return Instantiate(o, pos, o.transform.rotation);
        }

    }

    protected GameObject InstantiateFreeObj(GameObject o, Vector3 pos, int x, int y)
    {
        if (parent)
        {
            GameObject O = Instantiate(o, pos, o.transform.rotation, this.gameObject.transform);
            manager.AddFreeTile(x, y, O);
            return O;
        }
        else
        {
            GameObject O = Instantiate(o, pos, o.transform.rotation);
            manager.AddFreeTile(x, y, O);
            return O;
        }
    }    
}
