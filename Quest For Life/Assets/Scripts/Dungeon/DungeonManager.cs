using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField]
    GameObject DungeonGeneratorObj;

    [HideInInspector]
    public Tile[,] map;

    [SerializeField]
    public Dictionary<Tile, GameObject> FreeTiles;

    [HideInInspector]
    public GameManager manager;

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

    int times;

    void Awake()
    {
        custom = new DungeonGenPreset(MAX_LEAF_SIZE, MIN_LEAF_SIZE, mapWidth, mapLength);
    }

    // Start is called before the first frame update
    void Start()
    {
        FreeTiles = new Dictionary<Tile, GameObject>();

        times = 0;

        //for (int i = 0; i < 99; i++)
        //{
        //    StartCoroutine(StartFloorGenerationDelay(times, 0, i));
        //    times++;
        //}

        StartFloorGeneration();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddFreeTile(int x, int y, GameObject obj)
    {
        FreeTiles.Add(map[x, y], obj);
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

        int iterations = 0;
        bool finished = false;
        while (!finished)
        {
            if (!DG.Initiate(custom, true, true, 99, 5)) { Debug.Log("Floor had only 1 room"); }
            else finished = true;
            iterations++;
            if (iterations > 20) {
                finished = true; Debug.Log("Failed to create a floor after 20 attempts, giving up");
                Destroy(DG.gameObject);
                //DG.Initiate(Easy, true, true, 99, 5);
            }
        }
    }

    void StartFloorGeneration()
    {

        GameObject DGobj = Instantiate(DungeonGeneratorObj, Vector3.zero, Quaternion.identity);
        DungeonGenerator DG = DGobj.GetComponent<DungeonGenerator>();
        DG.manager = this;

        GameObject o = DG.gameObject;

        bool fountain = Random.value > 0.5f;
        bool shop = Random.value > 0.5f;

        int iterations = 0;
        bool finished = false;
        while (!finished)
        {
            if (!DG.Initiate(custom, true, true, 99, 5)) { Debug.Log("Floor had only 1 room"); }
            else finished = true;
            iterations++;
            if (iterations > 20)
            {
                finished = true; Debug.Log("Failed to create a floor after 20 attempts, giving up");
                Destroy(DG.gameObject);
                //DG.Initiate(Easy, true, true, 99, 5);
            }
        }
    }


    public void StartFloor(DungeonGenerator DG, Vector2 spawn)
    {
        Destroy(DG);
        //StartCoroutine(DestroyFloor(1, DG.gameObject));

        Vector3 spawnPos = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos.y = 1f;

        manager.player.gameObject.transform.position = spawnPos;
        manager.player.currentTile = map[(int)spawn.x, (int)spawn.y];

    }

    IEnumerator DestroyFloor(int time, GameObject o)
    {
        yield return new WaitForSeconds(time);

        Destroy(o);
    }

}
