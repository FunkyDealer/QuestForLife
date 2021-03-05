using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject FloorTileObj; //Tiles for floor
    [SerializeField]
    GameObject RoofTileObj; //Tiles for Roof

    //These are the tiles that are placed on the map
    #region Placed Tiles
    [SerializeField]
    GameObject WallTileObj; //Tiles for Walls
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
    #endregion


    [SerializeField]
    int mapWidth = 20;
    [SerializeField]
    int mapLength = 40;

    [HideInInspector]
    public Tile[,] map;
    
    [HideInInspector]
    public DungeonManager manager;

    public bool Finished;

    Vector2 spawn;

    List<Room> rooms;
    List<Hall> halls;

    void Awake()
    {
        Finished = false;
        spawn = new Vector2(5, 10);
    }

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        halls = new List<Hall>();        

        //CreateBigEmptyRoom();
        CreateNewMap();

        manager.map = this.map;

        DrawFloor();
        DrawMap();
        //DrawRoof();

        Finished = true;
        manager.StartFloor(this,spawn);
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

            x = Random.Range(1, mapWidth);
            y = Random.Range(1, mapLength);

            map[x, y].feature = Tile.Feature.Entrance;
            spawn = new Vector2(x, y);
        }
    }

    void CreateNewMap() //Creates a new randomly generated map
    {
        map = new Tile[mapWidth, mapLength];

        bool fountain = Random.value > 0.5f;
        bool shop = Random.value > 0.5f;

        createBlank();
        createDungeon();
        createFeatures(true, true);
        createChests(99, true);

    }

    void createBlank() //Creates a blank map where every tile is occupied
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                map[x, y] = new Tile(true, x, y);

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

    void createDungeon() //creates rooms and halls
    {
        const int MAX_LEAF_SIZE = 20;

        List<Partition> _leafs = new List<Partition>();

        Partition l; // helper Leaf

        // first, create a Leaf to be the 'root' of all Leafs.
        Partition root = new Partition(0, 0, mapWidth, mapLength);
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
                    if (l.width > MAX_LEAF_SIZE || l.height > MAX_LEAF_SIZE || Random.value > 0.25)
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

        Debug.Log($"there are {_leafs.Count} partitions");
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
                            map[x, y].occupied = false;
                            map[x, y].type = Tile.Type.room;
                        }
                        else if (map[x, y].occupied)
                        {
                            map[x, y].type = Tile.Type.wall;
                        }
                        map[x, y].roomNumber = numberOfRooms;
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
                                map[x, y].type = Tile.Type.hall;
                            }
                            else
                            {
                                if (map[x, y].type == Tile.Type.filling && map[x, y].occupied)
                                {
                                    map[x, y].type = Tile.Type.wall;
                                }
                            }
                        }
                    }
                    halls.Add(h);

                }
            }
        }

        Debug.Log($"there are {numberOfRooms} rooms");
        Debug.Log($"there are {numberOfHalls} halls");

        //foreach (var h in halls)
        //{
        //    Debug.Log($"Created hall: L: {h.leftRoom.number}, R: {h.rightRoom.number}  --- X: {h.x} Y: {h.y} W: {h.width} L: {h.height} ");
        //}
    }

    void createFeatures(bool shop, bool fountain) //Places down features in the map 
    {
        Room currentRoom = rooms[0];

        int entranceX = Random.Range(currentRoom.x, currentRoom.x + currentRoom.width);
        int entranceY = Random.Range(currentRoom.y, currentRoom.y + currentRoom.height);

        map[entranceX, entranceY].feature = Tile.Feature.Entrance;
        map[entranceX, entranceY].occupied = false;
        Debug.Log($"Entrance is in room {map[entranceX, entranceY].roomNumber} at X: {map[entranceX, entranceY].x} Y: {map[entranceX, entranceY].y}");
        spawn = new Vector2(entranceX, entranceY);

        currentRoom = rooms[rooms.Count - 1];

        int exitX = Random.Range(currentRoom.x + 1, currentRoom.x + currentRoom.width - 1);
        int exitY = Random.Range(currentRoom.y + 1, currentRoom.y + currentRoom.height - 1);

        map[exitX, exitY].feature = Tile.Feature.Exit;
        map[exitX, exitY].occupied = true;
        Debug.Log($"Exit is in room {map[exitX, exitY].roomNumber} at X: {map[exitX, exitY].x} Y: {map[exitX, exitY].y}");

        if (shop) //create the Shop
        {
            bool placed = false;
            while (!placed)
            {
                int roomNumber = Random.Range(0, rooms.Count - 2);
                currentRoom = rooms[roomNumber];

                int fX = currentRoom.x - 1;//base
                int fY = currentRoom.y;
                Tile.Facing facing = (Tile.Facing)Random.Range(0, 4); //Select in which side of the squared room the shop will be
                switch (facing)
                {
                    case Tile.Facing.north: //Down
                        fX = currentRoom.x + 1 + currentRoom.width;
                        fY = Random.Range(currentRoom.y, currentRoom.y + currentRoom.height);
                        break;
                    case Tile.Facing.west: //Left
                        fX = Random.Range(currentRoom.x, currentRoom.x + currentRoom.width);
                        fY = currentRoom.y - 1;
                        break;
                    case Tile.Facing.east: //Right
                        fX = Random.Range(currentRoom.x, currentRoom.x + currentRoom.width);
                        fY = currentRoom.y + 1 + currentRoom.height;
                        break;
                    case Tile.Facing.south: //Up
                        fX = currentRoom.x - 1;
                        fY = Random.Range(currentRoom.y, currentRoom.y + currentRoom.height);
                        break;
                    default:
                        Debug.Log("SHOP FACING ERROR");
                        break;
                }

                if (map[fX, fY].occupied && map[fX, fY].feature == Tile.Feature.none)
                { //if the picked place is a wall(occupied) and has no feature, place the Shop
                    map[fX, fY].feature = Tile.Feature.Shop;
                    map[fX, fY].facing = facing;
                    map[fX, fY].roomNumber = roomNumber;
                    currentRoom.shop = true;
                    placed = true;
                }
            }
        }

        if (fountain) //Creates Foutain
        {
            int roomNumber = Random.Range(1, rooms.Count - 1);
            currentRoom = rooms[roomNumber];
            bool placed = false;
            int iterations = 0;
            while (!placed)
            {
                int fX = Random.Range(currentRoom.x + 1, currentRoom.x + currentRoom.width - 1);
                int fY = Random.Range(currentRoom.y + 1, currentRoom.y + currentRoom.height - 1);

                if (!map[fX,fY].occupied && map[fX, fY].feature == Tile.Feature.none)
                {
                    map[fX, fY].feature = Tile.Feature.fountain;
                    map[fX, fY].occupied = true;
                    placed = true;
                }
                iterations++;
                if (iterations > 9) placed = true; //if it fails placing the fountain 9 times, it gives up
            }
        }
    }

    void createChests(int number, bool shop) //Place down chests on walls
    {
        if (number >= rooms.Count - 2) number = rooms.Count - 2;
        if (number <= 0) number = 0;
        if (!shop && rooms.Count > 2) number++;

        for (int i = 0; i < number; i++)
        {
            Room currentRoom = rooms[rooms.Count - 1];

            bool FreeRoom = false;
            int roomNumber = 0;
            while (!FreeRoom)
            {
                roomNumber = Random.Range(1, rooms.Count);
                currentRoom = rooms[roomNumber];
                if (!currentRoom.shop && !currentRoom.chest) FreeRoom = true;
            }

            bool placed = false;
            while (!placed)
            {
                int fX = currentRoom.x - 1;//base
                int fY = currentRoom.y;
                Tile.Facing facing = (Tile.Facing)Random.Range(0, 4); //Select in which side of the squared room the Chest will be
                switch (facing)
                {
                    case Tile.Facing.north: //Down
                        fX = currentRoom.x + 1 + currentRoom.width;
                        fY = Random.Range(currentRoom.y, currentRoom.y + currentRoom.height);
                        break;
                    case Tile.Facing.east: //Left
                        fX = Random.Range(currentRoom.x, currentRoom.x + currentRoom.width);
                        fY = currentRoom.y - 1;
                        break;
                    case Tile.Facing.west: //Right
                        fX = Random.Range(currentRoom.x, currentRoom.x + currentRoom.width);
                        fY = currentRoom.y + 1 + currentRoom.height;
                        break;
                    case Tile.Facing.south: //Up
                        fX = currentRoom.x - 1;
                        fY = Random.Range(currentRoom.y, currentRoom.y + currentRoom.height);
                        break;
                    default:
                        Debug.Log("CHEST FACING ERROR");
                        break;
                }

                if (map[fX, fY].occupied && map[fX, fY].feature == Tile.Feature.none) //if the picked place is a wall(occupied) and has no feature, place the chest
                {
                    map[fX, fY].feature = Tile.Feature.chest;
                    map[fX, fY].facing = facing;
                    map[fX, fY].roomNumber = roomNumber;
                    currentRoom.chest = true;
                    placed = true;
                }
            }
        }
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
                Vector3 position = new Vector3(x * 4 + 2, -0.1f, y * 4 + 2);
                Instantiate(FloorTileObj, position, Quaternion.identity);
            }
        }
    }

    void DrawMap() //Draws the Map
    {
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 2.5f, y * 4 + 2);

                if (map[x, y].occupied == true) //Occupied Tiles
                {
                    switch (map[x,y].type)
                    {
                        case Tile.Type.room:
                            switch (map[x,y].feature)
                            {
                                case Tile.Feature.Exit:
                                    Instantiate(ExitTileObj, position, Quaternion.identity);
                                    break;
                                case Tile.Feature.fountain:
                                    Instantiate(fountainTileObj, position, Quaternion.identity);
                                    break;
                                default:
                                    Debug.Log($"Error drawing a tile in a room's occupied Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case Tile.Type.wall:
                            switch (map[x,y].feature)
                            {
                                case Tile.Feature.Shop:
                                    Instantiate(ShopTileObj, position, Quaternion.identity);
                                    break;
                                case Tile.Feature.chest:
                                    Instantiate(ChestTileObj, position, Quaternion.identity);
                                    break;
                                case Tile.Feature.none:
                                    Instantiate(WallTileObj, position, Quaternion.identity);
                                    break;
                                default:
                                    Debug.Log($"Error drawing a tile in a Wall Tile at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case Tile.Type.filling:
                            Instantiate(WallTileObj, position, Quaternion.identity);
                            break;
                        default:
                            Debug.Log($"Error drawing a Occupied tile at X: {x} Y: {y}");
                            break;
                    }
                }
                else //Unnocupied Tiles
                {
                    switch (map[x,y].type)
                    {
                        case Tile.Type.room:
                            switch (map[x,y].feature)
                            {                               
                                case Tile.Feature.Entrance:
                                    GameObject e = Instantiate(EntranceTileObj, position, Quaternion.identity);
                                    manager.AddFreeTile(x, y, e);
                                    
                                    break;
                                case Tile.Feature.none:
                                    GameObject r = Instantiate(RoomTileObj, position, Quaternion.identity);
                                    manager.AddFreeTile(x, y, r);
                                    break;
                                default:
                                    Debug.Log($"Error Drawing a room tile in a room at X: {x} Y: {y}");
                                    break;
                            }
                            break;
                        case Tile.Type.hall:
                            switch (map[x,y].feature)
                            {
                                case Tile.Feature.none:
                                    GameObject h = Instantiate(HallTileObj, position, Quaternion.identity);
                                    manager.AddFreeTile(x, y, h);
                                    break;
                                default:
                                    Debug.Log($"Error Drawing a hall tile in a room at X: {x} Y: {y}");
                                    break;
                            }
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

    void DrawRoof()
    {

        //Draw Roof
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                Vector3 position = new Vector3(x * 4 + 2, 5.1f, y * 4 + 2);
                Instantiate(RoofTileObj, position, Quaternion.identity);
            }
        }
    }


    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}

