using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject dungeonManagerObj;
    DungeonManager dungeonManager;

    [SerializeField]
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        GameObject o = Instantiate(dungeonManagerObj, Vector3.zero, Quaternion.identity);
        DungeonManager dm = o.GetComponent<DungeonManager>();        
        dungeonManager = dm;
        dungeonManager.manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        



    }
}
