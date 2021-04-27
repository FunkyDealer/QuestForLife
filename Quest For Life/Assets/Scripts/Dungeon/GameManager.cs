using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject dungeonManagerObj;
    [HideInInspector]
    public DungeonManager dungeonManager;
    
    [SerializeField]
    GameObject playerObj;
    [HideInInspector]
    public Player player;

    [SerializeField]
    public GameObject monsterRoom;

    [HideInInspector]
    public bool startingNewGame;

    [SerializeField]
    public GameObject MonsterCamera;

    [SerializeField]
    CompassController compassController;

    [SerializeField]
    public ShopManager shopManager;

    [SerializeField]
    GameObject FinalZoneObj;

    void Awake()
    {
        startingNewGame = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(dungeonManagerObj, Vector3.zero, Quaternion.identity);
        DungeonManager dm = o.GetComponent<DungeonManager>();        
        dungeonManager = dm;
        dungeonManager.Init(this, FinalZoneObj);

        dungeonManager.CreateNewFloor();
        
    }

    // Update is called once per frame
    void Update()
    {
        



    }

    public void SpawnPlayer(Vector3 WorldPosition, Vector2 MapPosition, Tile[,] map)
    {
        if (player == null)
        {
            GameObject o = Instantiate(playerObj, WorldPosition, Quaternion.identity);

            player = o.GetComponent<Player>();
            player.compass = compassController;           
        }

        if (startingNewGame) player.Spawn(WorldPosition, MapPosition, map, this, dungeonManager);
        else player.Move(WorldPosition, MapPosition, map, dungeonManager);
    }


    public void MovePlayerToShop()
    {
        player.Move(shopManager.SpawnPos3d(), shopManager.Spawn2d(), shopManager.map, shopManager);        
    }

    public void MovePlayerToDungeon()
    {
        Vector3 pos3d = dungeonManager.FreeTiles[dungeonManager.currentShop].transform.position;
        pos3d.y = 1;
        Vector2 pos2d = new Vector2(dungeonManager.currentShop.x, dungeonManager.currentShop.y);

        player.Move(pos3d, pos2d, dungeonManager.map, dungeonManager);
    }


}
