using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject FloorTileObj;
    [SerializeField]
    GameObject WallTileObj;
    [SerializeField]
    GameObject RoofTileObj;
    [SerializeField]
    GameObject FreeSpaceObj;

    [SerializeField]
    GameObject playerObj;

    [SerializeField]
    int mapWidth = 50;
    [SerializeField]
    int mapLength = 50;

    [HideInInspector]
    public Tile[,] map;
    
    [HideInInspector]
    public DungeonManager manager;

    public bool Finished;

    Vector2 spawn;

    void Awake()
    {
        Finished = false;
        spawn = new Vector2(5, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
       

        map = new Tile[mapWidth, mapLength];

        //Generate map Walls
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                if (x == 0 || x == mapWidth - 1) map[x, y] = new Tile(true);
                else if (y == 0 || y == mapWidth - 1) map[x, y] = new Tile(true);
                else map[x, y] = new Tile(false);
            }
        }

        CreateSpawn(); //Creates the stairs
        createStairs(); //not done

        manager.map = this.map;

        //Draw Floor
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                    Vector3 position = new Vector3(x * 4 + 2, -0.1f, y * 4 + 2);
                    Instantiate(FloorTileObj, position, Quaternion.identity);
            }
        }

        //Draw Walls
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 2.5f, y * 4 + 2);
                if (map[x, y].Occupied == true) {                    
                    Instantiate(WallTileObj, position, Quaternion.identity);
                }
                else
                {
                    GameObject o = Instantiate(FreeSpaceObj, position, Quaternion.identity);
                    manager.AddFreeTile(x, y, o);
                }
            }
        }

        //Draw Roof
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 5.1f, y * 4 + 2);
                Instantiate(RoofTileObj, position, Quaternion.identity);
            }
        }



        Finished = true;
        manager.StartFloor(this,spawn);
    }


    void CreateSpawn()
    {
        int x = 5, y = 10;

        x = Random.Range(1, mapWidth);
        y = Random.Range(1, mapLength);

        map[x, y].getFlag(Tile.Flag.spawn);
        spawn = new Vector2(x, y);
    }

    void createStairs()
    {

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
