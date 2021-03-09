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

    void Awake()
    {
        player = playerObj.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(dungeonManagerObj, Vector3.zero, Quaternion.identity);
        DungeonManager dm = o.GetComponent<DungeonManager>();        
        dungeonManager = dm;
        dungeonManager.manager = this;
        player.dungeonManager = dungeonManager;
    }

    // Update is called once per frame
    void Update()
    {
        



    }
}
