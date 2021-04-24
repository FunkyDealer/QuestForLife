using System.Collections;
using System.Collections.Generic;
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

    public LockedExit lockedExit;

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
    DungeonGenPreset custom;
    //Dungeon Generation end

    public List<GameObject> monsterPrefabs;

    int times;

    [SerializeField]
    bool genDemo = false;

    [HideInInspector]
    GameObject currentFloorObject;

    int currentFloor;

    const int DEFAULT_ENCOUNTER_THRESHOLD = 10;
    int currentThresHold = 10;
    int stepCounter;

    public Tile currentShop;

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

        times = 0;
        if (genDemo)
        {
            for (int i = 0; i < 99; i++)
            {
                StartCoroutine(StartFloorGenerationDelay(times, 0, i));
                times++;
            }
        }
    }

    public void CreateNewFloor()
    {
        if (FreeTiles == null) FreeTiles = new Dictionary<Tile, GameObject>();
        FreeTiles.Clear();
        if (Chests == null) Chests = new Dictionary<Tile, GameObject>();
        Chests.Clear();

        if (currentFloorObject != null)
        {
            DestroyCurrentFloor();
        }

        currentFloor++;

        StartFloorGeneration();


    }

    // Update is called once per frame
    void Update()
    {

    }



    IEnumerator StartFloorGenerationDelay(float time, int x, int y)
    {
        yield return new WaitForSeconds(time);

        GameObject DGobj = Instantiate(DungeonGeneratorObj, Vector3.zero, Quaternion.identity);
        DungeonGenerator DG = DGobj.GetComponent<DungeonGenerator>();
        DG.manager = this;

        GameObject o = DG.gameObject;
        o.transform.position += new Vector3(0, 10 * y, 0);

        bool fountain = Random.value > 0.5f;
        bool shop = Random.value > 0.5f;

        int nKeys = Random.Range(1, 4);
        Debug.Log("Numero de chaves " + nKeys);

        int iterations = 0;
        bool finished = false;
        while (!finished)
        {
            if (!DG.Initiate(custom, true, true, 99, nKeys, currentFloor)) { Debug.Log("Floor had only 1 room"); }
            else finished = true;
            iterations++;
            if (iterations > 20)
            {
                finished = true; Debug.Log("Failed to create a floor after 20 attempts, giving up");
                Destroy(DG.gameObject);
                //DG.Initiate(Easy, true, true, 99, 5);
            }
        }
        lockedExit.numKeys = nKeys;
    }

    void StartFloorGeneration()
    {
        currentFloorObject = Instantiate(DungeonGeneratorObj, Vector3.zero, Quaternion.identity);
        DungeonGenerator DG = currentFloorObject.GetComponent<DungeonGenerator>();
        DG.manager = this;

        bool fountain = Random.value > 0.5f;
        bool shop = Random.value > 0.5f;

        int nKeys = Random.Range(1, 4);
        Debug.Log("Numero de chaves " + nKeys);

        int iterations = 0;
        bool finished = false;
        while (!finished)
        {
            if (!DG.Initiate(custom, true, true, 99, nKeys, currentFloor)) { Debug.Log("Floor had only 1 room"); }
            else finished = true;
            iterations++;
            if (iterations > 20)
            {
                finished = true; Debug.Log("Failed to create a floor after 20 attempts, giving up");
                Destroy(DG.gameObject);
                //DG.Initiate(Easy, true, true, 99, 5);
            }
        }
        lockedExit.numKeys = nKeys;
    }

    public override void StartMap(MapGenerator DG, Vector2 spawn)
    {
        if (!genDemo) Destroy(DG);
        else StartCoroutine(DestroyFloor(1, DG.gameObject));

        Vector3 spawnPos = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos.y = 1f;


        gameManager.SpawnPlayer(spawnPos, spawn, map);
    }

    IEnumerator DestroyFloor(int time, GameObject o)
    {
        yield return new WaitForSeconds(time);

        Destroy(o);
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




}
