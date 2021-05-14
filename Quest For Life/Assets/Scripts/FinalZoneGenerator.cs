using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneGenerator : MapGenerator
{
    //These are the tiles that are placed on the map
    #region Placed Tiles
    [SerializeField]
    GameObject WallTileObj; //Tiles for Walls
    [SerializeField]
    GameObject RoomTileObj; //Tile for Room
    [SerializeField]
    GameObject EntranceTileObj; //Tile for Entrance
    [SerializeField]
    GameObject endTileObj;
    #endregion

    public bool Initiate(int width, int length, MapManager manager, SaveData data)
    {
        this.mapWidth = width;
        this.mapLength = length;
        this.manager = manager;

        spawn = new Vector2(0, 5);

        GenerateFinalRoom();

        manager.map = this.map;

        DrawFloor();
        drawFinalRoom();
        DrawRoof();

       // manager.StartMap(this, spawn);
        DungeonManager DM = (DungeonManager)manager;
        if (data == null) DM.StartFinalRoom(this, spawn);
        else DM.gameManager.StartLoadedMap(this, data);


        return true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFinalRoom()
    {
        map = new Tile[mapWidth, mapLength];
        //Generate map Walls
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                //Create Walls
                if (x == 0 || x == mapWidth - 1)
                {
                    map[x, y] = new WallTile(true, x, y, true);
                }
                else if (y == 0 || y == mapLength - 1)
                {
                    map[x, y] = new WallTile(true, x, y, true);
                }
                else
                {
                    map[x, y] = new RoomMapTile(false, x, y);
                }
            }
        }
        if (true)
        {
            //Create Entrance
            int x = 1, y = mapLength / 2;

            RoomMapTile e = (RoomMapTile)map[x, y];
            e.roomFeature = RoomMapTile.RoomFeature.Entrance;
            e.occupied = false;
            map[x, y] = e;

            //  map[x, y].feature = Tile.Feature.ShopExit;
            //  map[x + 1, y].feature = Tile.Feature.Entrance;

            spawn = new Vector2(x, y);

            x = mapWidth - 4;
            y = mapLength / 2;

            RoomMapTile s = (RoomMapTile)map[x, y];
            s.roomFeature = RoomMapTile.RoomFeature.EndGame;
            s.occupied = true;
            s.facing = Tile.Facing.south;
            map[x, y] = s;

        }
    }

    void DrawFloor() //Draw Floor
    {
        Vector3 objPos = this.gameObject.transform.position;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 0f, objPos.z + (y * 4 + 2));
                if (!(map[x, y] is FillMapTile)) InstantiateObj(FloorTileObj, position);

            }
        }
    }

    void DrawRoof()
    {
        Vector3 objPos = this.gameObject.transform.position;
        //Draw Roof
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 5f, objPos.z + (y * 4 + 2));
                if (!(map[x, y] is FillMapTile))
                {
                    InstantiateObj(RoofTileObj, position);
                    //Instantiate(FloorTileObj, position,FloorTileObj.transform.rotation);
                }
            }
        }
    }


    void drawFinalRoom()
    {
        Vector3 objPos = this.gameObject.transform.position;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 2.5f, objPos.z + (y * 4 + 2));
                if (map[x, y].occupied == true) //Occupied Tiles
                {
                    switch (map[x, y])
                    {
                        case RoomMapTile r:
                            switch (r.roomFeature)
                            {
                                case RoomMapTile.RoomFeature.Fountain:
                                   //position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 0, objPos.z + (y * 4 + 2));
                                    //InstantiateObj(fountainTileObj, position);
                                    break;
                                case RoomMapTile.RoomFeature.EndGame:
                                    GameObject s = InstantiateObj(endTileObj, position);
                                    rotateObj(s, map[x, y].facing);
                                    break;
                                default:
                                    Debug.Log($"SHOP: Error drawing a tile in a room's occupied Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case WallTile w:
                            switch (w.wallFeature)
                            {
                                case WallTile.WallFeature.None:
                                    InstantiateObj(WallTileObj, position);
                                    break;
                                default:
                                    Debug.Log($"SHOP: Error drawing a tile in a Wall Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case FillMapTile f:
                            break;
                        default:
                            Debug.Log($"SHOP: Error drawing a Occupied tile at X: {x} Y: {y}");
                            break;
                    }
                }
                else //Unnocupied Tiles
                {
                    switch (map[x, y])
                    {
                        case RoomMapTile r:

                            switch (r.roomFeature)
                            {
                                case RoomMapTile.RoomFeature.Entrance:
                                    InstantiateFreeObj(EntranceTileObj, position, x, y);
                                    break;
                                case RoomMapTile.RoomFeature.None:
                                    InstantiateFreeObj(RoomTileObj, position, x, y);
                                    break;
                                default:
                                    Debug.Log($"SHOP: Error Drawing a room tile in a room at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        default:
                            Debug.Log($"SHOP: Error drawing a Free tile at X: {x} Y: {y}");
                            break;
                    }
                }
            }
        }
    }



}
