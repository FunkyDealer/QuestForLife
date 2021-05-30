using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public MusicPlayerController musicPlayer;

    void Awake()
    {
        startingNewGame = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveData data = DataGameMail.inst.getData();

        GameObject o = Instantiate(dungeonManagerObj, Vector3.zero, Quaternion.identity);
        DungeonManager dm = o.GetComponent<DungeonManager>();        
        dungeonManager = dm;
        dungeonManager.Init(this, FinalZoneObj);

        if (data == null) dungeonManager.CreateNewFloor();
        else dungeonManager.LoadGame(data);

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
        player.Move(shopManager.SpawnPos3d, shopManager.Spawn2d, shopManager.map, shopManager);
        player.TurnPlayer(shopManager.EntranceDir);
        player.location = Player.Location.SHOP;

    }

    public void MovePlayerToDungeon()
    {
        Tile forwarTile = dungeonManager.currentShop.tile.getForwardTile();
        Vector3 pos3d = dungeonManager.FreeTiles[forwarTile].transform.position;
        pos3d.y = 1;
        Vector2 pos2d = new Vector2(forwarTile.x, forwarTile.y);

        player.Move(pos3d, pos2d, dungeonManager.map, dungeonManager);
        player.TurnPlayer(dungeonManager.currentShop.tile.Direction());
        player.location = Player.Location.DUNGEON;
    }

    internal void EndGame()
    {
        GameObject o = Instantiate(DataBase.inst.ScreenChanger, Vector3.zero, Quaternion.identity);
        FadeToBlackScreenChange f = o.GetComponent<FadeToBlackScreenChange>();
        f.Init(GoToEndGame, false);

    }

    void GoToEndGame()
    {
        Debug.Log("Ending Game");
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single);
    }

    public void LoadPlayer(SaveData data, Vector3 worldPos, Tile[,] map)
    {
        GameObject o = Instantiate(playerObj, worldPos, Quaternion.identity);

        player = o.GetComponent<Player>();
        player.compass = compassController;

        Vector2 MapPosition = new Vector2(data.playerData.currentTile[0], data.playerData.currentTile[1]);

        player.Spawn(worldPos, MapPosition, map, this, dungeonManager, data);

        player.TurnPlayer((Global.FacingDirection)data.playerData.Orientation);
    }



    public void StartLoadedMap(MapGenerator DG, SaveData data)
    {
        Destroy(DG);
        Vector3 spawnPos = Vector3.zero;
        Tile[,] Map = dungeonManager.map;
        if (data.playerData.inShop)
        {
            spawnPos = shopManager.FreeTiles[shopManager.map[(int)data.playerData.currentTile[0], (int)data.playerData.currentTile[1]]].transform.position;
            Map = shopManager.map;
        }
        else
        {
            spawnPos = dungeonManager.FreeTiles[dungeonManager.map[(int)data.playerData.currentTile[0], (int)data.playerData.currentTile[1]]].transform.position;
        }

        spawnPos.y = 1f;

        LoadPlayer(data, spawnPos, Map);
    }
}
