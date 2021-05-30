using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MapGenerator
{
    //These are the tiles that are placed on the map
    #region Placed Tiles
    [SerializeField]
    GameObject WallTileObj; //Tiles for Walls
    [SerializeField]
    GameObject HallWallTileObj;
    [SerializeField]
    GameObject RoomTileObj; //Tile for Room
    [SerializeField]
    GameObject HallTileObj; //Tile for Hall
    [SerializeField]
    GameObject EntranceTileObj; //Tile for Entrance
    [SerializeField]
    GameObject ExitTileObj; //Tile for Exit
    [SerializeField]
    GameObject ShopTileObj; //Tile for Shop
    [SerializeField]
    GameObject ChestTileObj; //Tile for Chest
    [SerializeField]
    GameObject fountainTileObj; //Tile for fountain
    [SerializeField]
    GameObject lockedStairsObj; //Tile for Locked Stairs
    [SerializeField]
    List<GameObject> keysObj;
    #endregion
    
       
    List<Room> rooms;
    List<Hall> halls;

    int keyNumber;
    bool fountain;
    bool shop;
    int chestNumber;

    [HideInInspector]
    int MAX_LEAF_SIZE = 20;
    [HideInInspector]
    int MIN_LEAF_SIZE = 6;

    int floorNumber;

    void Awake()
    {
       
    }



    // Start is called before the first frame update
    void Start()
    {
       

        
    }

    public bool Initiate(DungeonGenPreset preset, bool fountain, bool shop, int chestNumber, int keys, int floorNumber, SaveData data)
    {
        this.mapWidth = preset.WIDTH;
        this.mapLength = preset.LENGTH;
        this.MAX_LEAF_SIZE = preset.MAX_LEAF_SIZE;
        this.MIN_LEAF_SIZE = preset.MIN_LEAF_SIZE;
        this.floorNumber = floorNumber;
        this.fountain = fountain;
        this.shop = shop;
        this.chestNumber = chestNumber;
        this.keyNumber = keys;
        if (keyNumber > 4) keyNumber = 4;

        spawn = new Vector2(5, 10);

        rooms = new List<Room>();
        halls = new List<Hall>();

        //CreateBigEmptyRoom();
        if (!CreateNewMap()) { return false; }
        else
        {
            fillTileConnections();


            manager.map = this.map;

            DrawFloor();
            DrawMap(data);
            DrawRoof();

            DungeonManager DG = (DungeonManager)manager;

            if (data == null) DG.StartMap(this, spawn, data);
            else DG.gameManager.StartLoadedMap(this, data);

            return true;
        }
    }

    /// <summary>
    /// This is where maps are created in bidimentional array form
    /// </summary>
    #region Map Creation

    void CreateBigEmptyRoom() //Creates a big Empty room (Test Purposes)
    {
        map = new Tile[mapWidth, mapLength];
        //Generate map Walls
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                if (x == 0 || x == mapWidth - 1) map[x, y] = new Tile(true, x, y);
                else if (y == 0 || y == mapWidth - 1) map[x, y] = new Tile(true, x, y);
                else map[x, y] = new Tile(false, x, y);
            }
        }
        if (true)
        {
            //Create Entrance
            int x = 5, y = 10;

            x = Global.Range(1, mapWidth, manager);
            y = Global.Range(1, mapLength, manager);

            spawn = new Vector2(x, y);
        }
    }

    bool CreateNewMap() //Creates a new randomly generated map
    {
        map = new Tile[mapWidth, mapLength];

        createBlank();
        if (!createDungeon()) return false;
        else
        {
            createFeatures();
            createChests();
            if (keyNumber > 0) createKeys();
            return true;
        }
    }

    void createBlank() //Creates a blank map where every tile is occupied
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                map[x, y] = new FillMapTile(true, x, y);
                map[x, y].floor = floorNumber;                
            }
        }
    }

    void fillTileConnections() //Fillsin tile connections
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                map[x, y].createConnections(map, mapWidth, mapLength);

            }
        }
    }

    bool createDungeon() //creates rooms and halls
    {
        List<Partition> _leafs = new List<Partition>();

        Partition l; // helper Leaf

        // first, create a Leaf to be the 'root' of all Leafs.
        Partition root = new Partition(0, 0, mapWidth, mapLength, MIN_LEAF_SIZE, manager);
        _leafs.Add(root);

        bool did_split = true;

        // we loop through every partition in our list over and over again, until no more partitions can be split.
        while (did_split)
        {
            did_split = false;

            for (int i = 0; i < _leafs.Count; i++)
            {
                l = _leafs[i];
                if (l.leftChild == null && l.rightChild == null) // if this partition is not already split...
                {
                    // if this partion is too big, or 75% chance...
                    if (l.width > MAX_LEAF_SIZE || l.height > MAX_LEAF_SIZE || Global.Value(manager) > 0.25)
                    {
                        if (l.Split()) // split the partition!
                        {
                            // if we did split, push the child partition to the List so we can loop into them next
                            _leafs.Add(l.leftChild);
                            _leafs.Add(l.rightChild);
                            did_split = true;
                        }
                    }
                }
            }
        }

        //Debug.Log($"there are {_leafs.Count} partitions");
        root.createRooms();

        int numberOfRooms = 0;
        int numberOfHalls = 0;
        foreach (Partition a in _leafs)
        {
            if (a.room != null)
            {
                numberOfRooms++;

                //Debug.Log($"Creating room {numberOfRooms}: X: {a.room.x} Y: {a.room.y} Width: {a.room.width} Height: {a.room.height}");

                for (int x = a.room.x - 1; x < a.room.x + a.room.width + 2; x++)
                {
                    for (int y = a.room.y - 1; y < a.room.y + a.room.height + 2; y++)
                    {
                        if (x >= a.room.x && x <= a.room.x + a.room.width && y >= a.room.y && y <= a.room.y + a.room.height)
                        {
                            RoomMapTile r = new RoomMapTile(map[x, y]);
                            r.roomNumber = numberOfRooms;
                            map[x, y] = r;
                            map[x, y].occupied = false;
                            
                        }
                        else if (map[x, y].occupied)
                        {
                            map[x, y] = new WallTile(map[x, y], true);
                        }
                    }
                }
                a.room.number = numberOfRooms;
                rooms.Add(a.room);
            }

            if (a.halls.Count > 0)
            {
                foreach (Hall h in a.halls)
                {
                    numberOfHalls++;
                    for (int x = h.x; x < h.x + h.width + 2; x++)
                    {
                        for (int y = h.y; y < h.y + h.height + 2; y++)
                        {
                            if (x > h.x && x <= h.x + h.width && y > h.y && y <= h.y + h.height)
                            {
                                map[x, y].occupied = false;
                                map[x, y] = new HallMapTile(map[x, y], numberOfHalls);
                            }
                            else
                            {
                                if (map[x, y] is FillMapTile && map[x, y].occupied)
                                {                                    
                                    map[x, y] = new WallTile(map[x, y], false);                                    
                                }
                            }
                        }
                    }
                    halls.Add(h);

                }
            }
        }
        if (numberOfRooms < 2) return false;

        //Debug.Log($"there are {numberOfRooms} rooms");
        // Debug.Log($"there are {numberOfHalls} halls");

        //foreach (var h in halls)
        //{
        //    Debug.Log($"Created hall: L: {h.leftRoom.number}, R: {h.rightRoom.number}  --- X: {h.x} Y: {h.y} W: {h.width} L: {h.height} ");
        //}

        return true;
    }

    void createFeatures() //Places down features in the map 
    {
        Room currentRoom = rooms[0];

        int entranceX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager); //Entrance's random X
        int entranceY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager); //Entrance's random Y

        if (map[entranceX, entranceY] is RoomMapTile) //create Entrance
        {
            RoomMapTile E = (RoomMapTile)map[entranceX, entranceY];

            E.roomFeature = RoomMapTile.RoomFeature.Entrance;
            E.occupied = false;
            // Debug.Log($"Entrance: {map[entranceX, entranceY].roomNumber} at X: {map[entranceX, entranceY].x} Y: {map[entranceX, entranceY].y}");
            spawn = new Vector2(entranceX, entranceY);

            map[entranceX, entranceY] = E; //Set Entrance
        }
        else
        {
            Debug.Log($"Entrance Error: map[{entranceX},{entranceY}] was not a RoomMapTile and instead a {map[entranceX, entranceY].GetType().ToString()}");
        }

        currentRoom = rooms[rooms.Count - 1];

        int exitX = Global.Range(currentRoom.x + 1, currentRoom.x + currentRoom.width - 1, manager); //exit's random X
        int exitY = Global.Range(currentRoom.y + 1, currentRoom.y + currentRoom.height - 1, manager); //exit's random Y

        if (map[exitX, exitY] is RoomMapTile) //create Exit
        {
            RoomMapTile exit = (RoomMapTile)map[exitX, exitY];

            if (keyNumber == 0) exit.roomFeature = RoomMapTile.RoomFeature.Exit;
            else exit.roomFeature = RoomMapTile.RoomFeature.LockedExit;
            exit.occupied = true;

            map[exitX, exitY] = exit;
        }
        else
        {                       
            Debug.Log($"Exit Error: map[{exitX},{exitY}] was not a RoomMapTile and instead a {map[exitX, exitY].GetType().ToString()}");
        }
        //  Debug.Log($"Exit is in room {map[exitX, exitY].roomNumber} at X: {map[exitX, exitY].x} Y: {map[exitX, exitY].y}");

        //Debug.Log($"Entrance: r:{map[entranceX, entranceY].roomNumber} X:{map[entranceX, entranceY].x} Y:{map[entranceX, entranceY].y}; Exit: r:{map[exitX, exitY].roomNumber} X:{map[exitX, exitY].x} Y:{map[exitX, exitY].y}");

        if (shop) //create the Shop
        {
            bool placed = false;
            //base
            int fX = currentRoom.x - 1;
            int fY = currentRoom.y;

            while (!placed)
            {
                int roomNumber = Global.Range(0, rooms.Count - 2, manager);
                currentRoom = rooms[roomNumber];               
               
                Tile.Facing facing = (Tile.Facing)Global.Range(0, 4, manager); //Select in which side of the squared room the shop will be
                switch (facing)
                {
                    case Tile.Facing.north: //Down
                        fX = currentRoom.x + 1 + currentRoom.width;
                        fY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager);
                        break;
                    case Tile.Facing.east: //Left
                        fX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager);
                        fY = currentRoom.y - 1;
                        break;
                    case Tile.Facing.west: //Right
                        fX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager);
                        fY = currentRoom.y + 1 + currentRoom.height;
                        break;
                    case Tile.Facing.south: //Up
                        fX = currentRoom.x - 1;
                        fY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager);
                        break;

                    default:
                        Debug.Log("SHOP FACING ERROR");
                        break;
                }

                if (map[fX, fY] is WallTile)
                {
                    WallTile wall = (WallTile)map[fX, fY];

                    if (wall.occupied && wall.wallFeature == WallTile.WallFeature.None)
                    { //if the picked place is a wall(occupied) and has no feature, place the Shop
                        wall.wallFeature = WallTile.WallFeature.ShopEntrance;
                        wall.facing = facing;
                        currentRoom.shop = true;
                        placed = true;
                        map[fX, fY] = wall;
                    }
                }
                else
                {
                    Debug.Log($"Shop Entrance Error: map[{fX},{fY}] was not a WallTile and instead a {map[fX, fY].GetType().ToString()}");
                }
            }

            //Debug.Log($"Shop: r:{map[fX, fY].roomNumber} X: {map[fX, fY].x} Y: {map[fX, fY].y}");
        }

        if (fountain) //Creates Foutain
        {
            int roomNumber = Global.Range(1, rooms.Count - 1, manager);
            currentRoom = rooms[roomNumber];
            bool placed = false;
            int iterations = 0;

            int fX = currentRoom.x;
            int fY = currentRoom.y;

            while (!placed)
            {
                fX = Global.Range(currentRoom.x + 1, currentRoom.x + currentRoom.width - 1, manager);
                fY = Global.Range(currentRoom.y + 1, currentRoom.y + currentRoom.height - 1, manager);

                if (map[fX, fY] is RoomMapTile) //create Fountain
                {
                    RoomMapTile f = (RoomMapTile)map[fX, fY];

                    if (!f.occupied && f.roomFeature == RoomMapTile.RoomFeature.None)
                    {
                        f.roomFeature = RoomMapTile.RoomFeature.Fountain;
                        f.occupied = true;
                        placed = true;
                        map[fX, fY] = f;
                    }
                    iterations++;
                    if (iterations > 9) placed = true; //if it fails placing the fountain 9 times, it gives up
                }
                else
                {
                    Debug.Log($"Fountain Error: map[{fX},{fY}] was not a RoomMapTile and instead a {map[fX, fY].GetType().ToString()}");
                }
            } 


           // Debug.Log($"fountain is in room {map[fX, fY].roomNumber} at X: {map[fX, fY].x} Y: {map[fX, fY].y}");
        }
    }

    void createChests() //Place down chests on walls
    {
        if (chestNumber >= rooms.Count - 2) chestNumber = rooms.Count - 2;
        if (chestNumber <= 0) chestNumber = 0;
        if (!shop && rooms.Count > 2) chestNumber++;

        for (int i = 0; i < chestNumber; i++)
        {
            Room currentRoom = rooms[rooms.Count - 1];

            bool FreeRoom = false;
            int roomNumber = 0;
            while (!FreeRoom)
            {
                roomNumber = Global.Range(1, rooms.Count, manager);
                currentRoom = rooms[roomNumber];
                if (!currentRoom.shop && !currentRoom.chest) FreeRoom = true;
            }

            bool placed = false;
            while (!placed)
            {
                int fX = currentRoom.x - 1;//base
                int fY = currentRoom.y;
                Tile.Facing facing = (Tile.Facing)Global.Range(0, 4, manager); //Select in which side of the squared room the Chest will be
                switch (facing)
                {
                    case Tile.Facing.north: //Down
                        fX = currentRoom.x + 1 + currentRoom.width;
                        fY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager);
                        break;
                    case Tile.Facing.east: //Left
                        fX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager);
                        fY = currentRoom.y - 1;
                        break;
                    case Tile.Facing.west: //Right
                        fX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager);
                        fY = currentRoom.y + 1 + currentRoom.height;
                        break;
                    case Tile.Facing.south: //Up
                        fX = currentRoom.x - 1;
                        fY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager);
                        break;
                    default:
                        Debug.Log("CHEST FACING ERROR");
                        break;
                }

                if (map[fX, fY] is WallTile) //if the chosen tile is a Wall
                {
                    WallTile w = (WallTile)map[fX, fY];

                    if (w.occupied && w.wallFeature == WallTile.WallFeature.None) //if the picked place is a wall(occupied) and has no feature, place the chest
                    {
                        w.wallFeature = WallTile.WallFeature.Chest;
                        w.facing = facing;
                        currentRoom.chest = true;
                        placed = true;
                        map[fX, fY] = w;
                    }
                }
                else if (map[fX, fY] is HallMapTile)
                {
                    //The randomizer might accidently try to place the chest in a hall, in this case, it will try again
                }
                else
                {
                    Debug.Log($"Chest Placing Error: map[{fX},{fY}] was a {map[fX, fY].GetType().ToString()} and neither a Wall or a Hall");                    
                }
            }
        }

        //Debug.Log($"{chestNumber} chest successfully placed");
    }

    void createKeys()
    {
        //Debug.Log("creating keys");
        if (keyNumber >= rooms.Count - 2) keyNumber = rooms.Count - 2;
        if (keyNumber <= 0) keyNumber = 0;

        for (int i = 0; i < keyNumber; i++)
        {
            Room currentRoom = rooms[rooms.Count-1];

            bool FreeRoom = false;
            int roomNumber = 0;
            while (!FreeRoom)
            {
                roomNumber = Global.Range(1, rooms.Count-1, manager);
                currentRoom = rooms[roomNumber];
                if (!currentRoom.key) FreeRoom = true;
            }

            bool placed = false;
            while (!placed)
            {
                int keyX = Global.Range(currentRoom.x, currentRoom.x + currentRoom.width, manager);
                int keyY = Global.Range(currentRoom.y, currentRoom.y + currentRoom.height, manager);

                if (map[keyX, keyY] is RoomMapTile) //create Key
                {
                    RoomMapTile k = (RoomMapTile)map[keyX, keyY];

                    if (!k.occupied && k.roomFeature == RoomMapTile.RoomFeature.None) //if the picked place is unnocupied and has no feature, place the key
                    {
                        k.roomFeature = RoomMapTile.RoomFeature.Key;
                        k.roomNumber = roomNumber;
                        currentRoom.key = true;
                        placed = true;
                        map[keyX, keyY] = k;
                    }
                }
                else
                {
                    Debug.Log($"Key Placing Error: map[{keyX},{keyY}] was a {map[keyX, keyY].GetType().ToString()} and not a RoomMapTile");
                }
            }
        }

        //Debug.Log($"{keyNumber} keys successfully placed, { chestNumber} chests successfully placed");
    }
    #endregion
    
    /// <summary>
    /// This is where the Floor is Drawed, Objects are spawned in 3d based on the map created
    /// </summary>
    #region Map Drawing
    void DrawFloor() //Draw Floor
    {        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 0f, y * 4 + 2);
                if (!(map[x, y] is FillMapTile)) InstantiateObj(FloorTileObj, position);            

            }
        }
    }

    void DrawMap(SaveData data = null) //Draws the Map
    {
        int placedKey = 0;
        int placedChests = 0;
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 2.5f, y * 4 + 2);

                if (map[x, y].occupied == true) //Occupied Tiles
                {
                    switch (map[x,y])
                    {
                        case RoomMapTile r:
                            switch (r.roomFeature)
                            {
                                case RoomMapTile.RoomFeature.Entrance:
                                    break;
                                case RoomMapTile.RoomFeature.Exit:
                                    GameObject e = InstantiateObj(ExitTileObj, position);
                                    DungeonManager dm = (DungeonManager)manager;
                                    dm.exit = e;
                                    break;
                                case RoomMapTile.RoomFeature.LockedExit:
                                    createLockedExit(position, r);                                   
                                    break;
                                case RoomMapTile.RoomFeature.Fountain:
                                    position = new Vector3(x * 4 + 2, 0, y * 4 + 2);
                                    InstantiateObj(fountainTileObj, position);
                                    break;
                                case RoomMapTile.RoomFeature.Boss:
                                    break;
                                case RoomMapTile.RoomFeature.Key:
                                    break;
                                case RoomMapTile.RoomFeature.Shop:
                                    break;
                                case RoomMapTile.RoomFeature.None:
                                    break;
                                default:
                                    Debug.Log($"Error drawing a tile in a room's occupied Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case WallTile w:
                            switch (w.wallFeature)
                            {
                                case WallTile.WallFeature.ShopEntrance:
                                    position = new Vector3(x * 4 + 2, 0, y * 4 + 2);
                                    GameObject s = InstantiateObj(ShopTileObj, position);
                                    rotateObj(s, map[x, y].facing);
                                    SetShopEntrance(s, map[x, y],data);
                                    break;
                                case WallTile.WallFeature.Chest:

                                    createChest(position, x, y, data);

                                    break;
                                case WallTile.WallFeature.ShopExit:
                                    break;
                                case WallTile.WallFeature.None:
                                    if (w.Room) InstantiateObj(WallTileObj, position);
                                    else InstantiateObj(HallWallTileObj, position);
                                    break;
                                default:
                                    Debug.Log($"Error drawing a tile in a Wall Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case FillMapTile f:
                           // Instantiate(WallTileObj, position, Quaternion.identity);
                           // InstantiateObj(WallTileObj, position);
                            break;
                        default:
                            Debug.Log($"Error drawing a Occupied tile at X: {x} Y: {y}");
                            break;
                    }
                }
                else //Unnocupied Tiles
                {
                    switch (map[x,y])
                    {
                        case RoomMapTile r:

                            switch (r.roomFeature)
                            {
                                case RoomMapTile.RoomFeature.Entrance:
                                    InstantiateFreeObj(EntranceTileObj, position, x, y);
                                    break;
                                case RoomMapTile.RoomFeature.Exit:
                                    break;
                                case RoomMapTile.RoomFeature.LockedExit:
                                    break;
                                case RoomMapTile.RoomFeature.Fountain:
                                    break;
                                case RoomMapTile.RoomFeature.Boss:
                                    break;
                                case RoomMapTile.RoomFeature.Key:
                                    GameObject g = InstantiateFreeObj(RoomTileObj, position, x, y);

                                    RoomTile RT = g.GetComponent<RoomTile>();
                                    RT.Init(manager, r.roomNumber, r);

                                    if (data != null)
                                    {
                                        bool hasKey = false;                                        
                                        foreach (var K in data.playerData.keys)
                                        {
                                            if (placedKey == K) hasKey = true;
                                        }
                                        if (hasKey == false)
                                        {
                                            position = new Vector3(x * 4 + 2, 0, y * 4 + 2);
                                            InstantiateObj(keysObj[placedKey], position);
                                        }
                                    }
                                    else
                                    {
                                        position = new Vector3(x * 4 + 2, 0, y * 4 + 2);
                                        InstantiateObj(keysObj[placedKey], position);
                                    }
                                    placedKey++;
                                    break;
                                case RoomMapTile.RoomFeature.Shop:
                                    break;
                                case RoomMapTile.RoomFeature.None:
                                    GameObject o = InstantiateFreeObj(RoomTileObj, position, x, y);
                                    RoomTile rt = o.GetComponent<RoomTile>();
                                    rt.Init(manager, r.roomNumber, r);
                                    break;
                                default:
                                    Debug.Log($"Error Drawing a room tile in a room at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case HallMapTile h:
                            GameObject H = InstantiateFreeObj(HallTileObj, position, x, y);
                            HallTile HT = H.GetComponent<HallTile>();
                            HT.Init(manager, h.hallNumber, h);
                            break;
                        default:
                            Debug.Log($"Error drawing a Free tile at X: {x} Y: {y}");
                            break;
                    }
                    
                    //manager.AddFreeTile(x, y, o);
                }

            }
        }
    }

    private void createLockedExit(Vector3 position, RoomMapTile t)
    {
        if (keyNumber == 0)
        {
            GameObject i = InstantiateObj(ExitTileObj, position);
            DungeonManager d = (DungeonManager)manager;
            d.exit = i;
            t.roomFeature = RoomMapTile.RoomFeature.Exit;
        }
        else
        {
            GameObject o = InstantiateObj(lockedStairsObj, position);
            LockedStairs l = o.GetComponent<LockedStairs>();
            l.numKeys = keyNumber;
            DungeonManager DM = (DungeonManager)manager;
            DM.exit = o;
        }
    }

    private void SetShopEntrance(GameObject ShopEntranceObj, Tile tile, SaveData data)
    {
        ShopEntrance e = ShopEntranceObj.GetComponent<ShopEntrance>();
        e.shopManager = manager.gameManager.shopManager;
        e.tile = tile;
        if (data != null) e.data = data.shopData;
        else e.data = null;

        DungeonManager d = (DungeonManager)manager;
        d.currentShop = e;
    }

    void DrawRoof()
    {
        //Draw Roof
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 5f, y * 4 + 2);
                if (!(map[x,y] is FillMapTile))
                {
                   InstantiateObj(RoofTileObj, position);
                    //Instantiate(FloorTileObj, position,FloorTileObj.transform.rotation);
                }
            }
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }


    void createChest(Vector3 position, int x, int y, SaveData data)
    {
        GameObject o = InstantiateObj(ChestTileObj, position);
        rotateObj(o, map[x, y].facing);
        DungeonManager DM = (DungeonManager)manager;
        DM.Chests.Add(map[x, y], o);
        Chest c = o.GetComponent<Chest>();
        c.id = chestNumber;
        chestNumber++;

        if (data != null)
        {
            foreach (var C in data.mapData.openChests)
            {
                if (c.id == C) c.isOpen = true;
            }
        }
    }

}

