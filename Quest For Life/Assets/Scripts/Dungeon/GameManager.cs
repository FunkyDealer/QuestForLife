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
        dungeonManager.manager = this;
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

            
        }

        if (startingNewGame) player.Spawn(WorldPosition, MapPosition, map, this, dungeonManager);
        else player.Move(WorldPosition, MapPosition, map);

    }





}
