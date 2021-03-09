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



    // Start is called before the first frame update
    void Start()
    {
        FreeTiles = new Dictionary<Tile, GameObject>();

        GameObject DGobj = Instantiate(DungeonGeneratorObj, Vector3.zero, Quaternion.identity);
        DungeonGenerator DG = DGobj.GetComponent<DungeonGenerator>();
        DG.manager = this;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFreeTile(int x, int y, GameObject obj)
    {
        FreeTiles.Add(map[x, y], obj);
    }


    public void StartFloor(DungeonGenerator DG, Vector2 spawn)
    {
        Destroy(DG.gameObject);
        
        Vector3 spawnPos = FreeTiles[map[(int)spawn.x, (int)spawn.y]].transform.position;
        spawnPos.y = 1f;
        
        manager.player.gameObject.transform.position = spawnPos;
        manager.player.currentTile = map[(int)spawn.x,(int)spawn.y];

    }
    
}
