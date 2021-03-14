using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoomManager : MonoBehaviour
{
    [SerializeField]
    GameObject spawner;

    GameObject currentMonster;

    private static MonsterRoomManager _instance;
    public static MonsterRoomManager inst { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        currentMonster = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMonster(GameObject monster)
    {
        this.currentMonster = monster;

        SpawnCurentMonster();
    }


    public void SpawnCurentMonster()
    {
        GameObject m = Instantiate(currentMonster, spawner.transform.position, Quaternion.identity);
        currentMonster = m;
    }


    public void RemoveMonster()
    {
        Destroy(currentMonster);
    }
}
