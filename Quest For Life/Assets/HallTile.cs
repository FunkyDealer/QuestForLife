using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallTile : PhysicalTile
{
    public MapManager m;

    public int HallNumber;

    [SerializeField]
    int WindChance = 10;

    [HideInInspector]
    public bool wind;

    [SerializeField]
    GameObject WindSourcePreFab;

    void Awake()
    {
        wind = false;
    }

    public void Init(MapManager m, int HallNumber, Tile t)
    {
        this.m = m;
        this.HallNumber = HallNumber;
        this.tile = t;
    }

    // Start is called before the first frame update
    void Start()
    {
        PropBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PropBuilder()
    {
        if (WindChance >= Global.Range(0, 101, m) && !checkNearby(checkForWind))
        {
            wind = true;
            Instantiate(WindSourcePreFab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        } 


    }

    bool checkNearby(System.Func<HallTile,bool> condition)
    {
        Vector2 startPos = new Vector2(tile.x - 5, tile.y - 5);

        for (int x = (int)startPos.x; x < tile.x + 5; x++)
        {
            for (int y = (int)startPos.y; y < tile.y + 5; y++)
            {
                Tile e;
                try
                {
                    e = m.map[x, y];
                } catch
                {
                    continue;
                }

                if (e is HallMapTile)
                {
                    HallTile h = m.FreeTiles[e].GetComponent<HallTile>();
                    if (condition(h)) return true;
                }
            }
        }

        return false;
    }

    bool checkForWind(HallTile e)
    {
        return e.wind;
    }
}
