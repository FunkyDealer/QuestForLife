﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MapManager
{
    //Dungeon Generation start
    [SerializeField]
    GameObject DungeonGeneratorObj;
    [SerializeField]
    GameObject BattleManagerObj;

    [HideInInspector]
    public Dictionary<Tile, GameObject> Chests;

    [SerializeField]
    int MAX_LEAF_SIZE = 20;
    [SerializeField]
    int MIN_LEAF_SIZE = 6;
    [SerializeField]
    int mapWidth = 20;
    [SerializeField]
    int mapLength = 40;

    DungeonGenPreset Easy = new DungeonGenPreset(30, 9, 20, 20);
    DungeonGenPreset Memium = new DungeonGenPreset(30, 9, 30, 40);
    DungeonGenPreset Hard = new DungeonGenPreset(40, 9, 50, 50);
    DungeonGenPreset VeryHard1 = new DungeonGenPreset(10, 9, 50, 60);
    DungeonGenPreset custom;
    //Dungeon Generation end

    [HideInInspector]
    GameObject currentFloorObject;

    int currentFloor;
    public int Floor => currentFloor;

    const int DEFAULT_ENCOUNTER_THRESHOLD = 10;
    int currentThresHold = 10;
    int stepCounter;

    public ShopEntrance currentShop;

    GameObject FinalZoneObj;
    [SerializeField]
    GameObject finalZoneGeneratorPrefab;

    [HideInInspector]
    public GameObject exit;

    public void Init(GameManager gm, GameObject finalZoneObj)
    {
        this.gameManager = gm;
        this.FinalZoneObj = finalZoneObj;
        
    }

    void Awake()
    {
        currentShop = null;
        currentFloor = 0;
        stepCounter = 0;

        custom = new DungeonGenPreset(MAX_LEAF_SIZE, MIN_LEAF_SIZE, mapWidth, mapLength);

    }

    // Start is called before the first frame update
    void Start()
    {      
 
    }

    void resetFloor()
    {
        if (FreeTiles == null) FreeTiles = new Dictionary<Tile, GameObject>();
        FreeTiles.Clear();
        if (Chests == null) Chests = new Dictionary<Tile, GameObject>();
        Chests.Clear();

        currentShop = null;
        exit = null;

        if (currentFloorObject != null)
        {
            DestroyCurrentFloor();
        }
    }

    public void CreateNewFloor()
    {
        resetFloor();

        currentFloor++;

        StartFloorGeneration(false);
    }

    public void LoadGame(SaveData data)
    {
        resetFloor();
        currentFloor = data.mapData.floor;
        mapSeeds = data.mapData.seeds;

        if (data.playerData.inFinalZone) CreateFinalZone(data);
        else StartFloorGeneration(true, data);
    }

    public void CreateFinalZone(SaveData data = null)
    {
        if (data == null)
        {
            resetFloor();

            currentFloor++;
        }
        else
        {
            currentFloor = data.mapData.floor;
        }

            currentFloorObject = Instantiate(finalZoneGeneratorPrefab, Vector3.zero, Quaternion.identity);
            FinalZoneGenerator e = currentFloorObject.GetComponent<FinalZoneGenerator>();

            e.Initiate(30, 7, this, data);
    }

    // Update is called once per frame
    void Update()
    {


    }

    void StartFloorGeneration(bool loading, SaveData data = null)
    {
        currentFloorObject = Instantiate(DungeonGeneratorObj, Vector3.zero, Quaternion.identity);
        DungeonGenerator DG = currentFloorObject.GetComponent<DungeonGenerator>();
        DG.manager = this;     
        
        currentSeed = 0;

        if (!loading) mapSeeds = generateNewSeed();

        int iterations = 0;
        bool finished = false;
        while (!finished)
        {

            bool fountain = Global.Value(this) > 0.5f;
            bool shop = Global.Value(this) > 0.5f;

            if (!DG.Initiate(custom, true, true, 99, 5, currentFloor, data)) { Debug.Log("Floor had only 1 room"); }
            else finished = true;
            iterations++;
            if (iterations > 20)
            {
                finished = true;
                Debug.Log("Failed to create a floor after 20 attempts, giving up");
                Destroy(DG.gameObject);
                //DG.Initiate(Easy, true, true, 99, 5);
            }
        }       
    }

    public override void StartMap(MapGenerator DG, Vector2 spawn, SaveData data)
    {
        Destroy(DG);


        Vector3 spawnPos = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos.y = 1f;


        gameManager.SpawnPlayer(spawnPos, spawn, map);
    }



    public void StartFinalRoom(MapGenerator DG, Vector2 spawn)
    {
        Destroy(DG);

        Vector3 spawnPos = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos.y = 1f;


        gameManager.SpawnPlayer(spawnPos, spawn, map);
        gameManager.player.MovementManager.leaveDungeon();
        gameManager.player.TurnPlayer(Global.FacingDirection.SOUTH);
    }

    void DestroyCurrentFloor()
    {
        Destroy(currentFloorObject);
    }    

    public bool CheckForEncounter()
    {
        bool enemyEncounter = false;
        stepCounter++;
        
        if (stepCounter > 10)
        {
            enemyEncounter = Random.Range(0, 100) < currentThresHold;

            if (enemyEncounter)
            {
                currentThresHold = DEFAULT_ENCOUNTER_THRESHOLD;
                InitiateBattle();
                stepCounter = 0;
            }
            else
            {
                currentThresHold += 2;
            }
        }

        return enemyEncounter;
    }

    void InitiateBattle()
    {
        //START ENCOUNTER
        //Debug.Log("Encounter Starting");        

        MonsterGenerator mg = gameObject.AddComponent(typeof(MonsterGenerator)) as MonsterGenerator;
        mg.Initiate(this, gameManager.player, currentFloor);

        BattleManager bm = Instantiate(BattleManagerObj, Vector3.zero, Quaternion.identity).GetComponent<BattleManager>();

        bm.StartBattle(gameManager.player, gameManager.player.StartBattle());
    }


    int[] generateNewSeed()
    {
        int[] mapSeeds = new int[254];

        for (int i = 0; i < 254; i++)
        {
            int[] seed = new int[9];
            for (int z = 0; z < 9; z++)
            {
                seed[z] = Random.Range(0, 10);              

            }
            mapSeeds[i] = seed.Select((t, r) => t * System.Convert.ToInt32(Mathf.Pow(10, seed.Length - r - 1))).Sum();
        }

        return mapSeeds;
    }

}
