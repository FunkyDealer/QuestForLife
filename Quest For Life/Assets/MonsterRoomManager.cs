using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoomManager : MonoBehaviour
{
    [SerializeField]
    GameObject spawner;

    GameObject currentMonster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnMonster(GameObject o)
    {
        GameObject m = Instantiate(o, spawner.transform.position, Quaternion.identity);
        currentMonster = m;
    }

    public void RemoveMonster()
    {
        Destroy(currentMonster);
    }
}
