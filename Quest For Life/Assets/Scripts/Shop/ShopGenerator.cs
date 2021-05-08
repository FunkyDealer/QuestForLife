using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGenerator : MapGenerator
{   

    //These are the tiles that are placed on the Shop
    #region Placed Tiles
    [SerializeField]
    GameObject WallTileObj; //Tiles for Walls
    [SerializeField]
    GameObject RoomTileObj; //Tile for Room
    [SerializeField]
    GameObject EntranceTileObj; //Tile for Entrance
    [SerializeField]
    GameObject ShopTileObj; //Tile for Shop
    [SerializeField]
    GameObject ExitTileObj; //Tile for Shop
    [SerializeField]
    GameObject fountainTileObj; //Tile for fountain
    #endregion

    Global.FacingDirection entranceDir;

    public bool Initiate(int width, int length)
    {
        this.mapWidth = width;
        this.mapLength = length;
        
        spawn = new Vector2(0, 5);        
 
        GenerateShop();

        manager.map = this.map;

        DrawFloor();
        drawShop();
        DrawRoof();

        ShopManager s = (ShopManager)manager;
        s.StartShop(this, spawn, Global.FacingDirection.SOUTH);

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


    void GenerateShop()
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
            int x = 0, y = mapLength / 2;

            WallTile e = (WallTile)map[x, y];
            e.wallFeature = WallTile.WallFeature.ShopExit;
            e.facing = Tile.Facing.south;
            e.occupied = true;
            map[x, y] = e;

            RoomMapTile eE = (RoomMapTile)map[x + 1, y];
            eE.roomFeature = RoomMapTile.RoomFeature.Entrance;
            eE.occupied = false;
            map[x + 1, y] = eE;

           //  map[x, y].feature = Tile.Feature.ShopExit;
           //  map[x + 1, y].feature = Tile.Feature.Entrance;

            spawn = new Vector2(x + 1, y);

            x = mapWidth - 2;
            y = 1;

            RoomMapTile s = (RoomMapTile)map[x, y];
            s.roomFeature = RoomMapTile.RoomFeature.Shop;
            s.occupied = true;
            s.facing = Tile.Facing.east;
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
                if (!(map[x,y] is FillMapTile)) InstantiateObj(FloorTileObj, position);

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

    void drawShop()
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
                                    position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 0, objPos.z + (y * 4 + 2));
                                    InstantiateObj(fountainTileObj, position);
                                    break;
                                case RoomMapTile.RoomFeature.Shop:
                                    GameObject s = InstantiateObj(ShopTileObj, position);
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
                                case WallTile.WallFeature.ShopExit:
                                    position = new Vector3(objPos.x + (x * 4 + 2), objPos.y + 0, objPos.z + (y * 4 + 2));
                                    GameObject s = InstantiateObj(ExitTileObj, position);
                                    rotateObj(s, map[x, y].facing);
                                    SetUpShopExit(s, map[x, y]);
                                    break;
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

    private void SetUpShopExit(GameObject shopExitObj, Tile tile)
    {
        ShopExit e = shopExitObj.GetComponent<ShopExit>();
        ShopManager m = (ShopManager)manager;
        e.shopManager = m;
        e.tile = tile;
        m.shopExit = e;
    }
}
