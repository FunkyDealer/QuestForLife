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

    public void SetMonster(GameObject monster)
    {
        this.currentMonster = monster;

        SpawnCurentMonster();
    }


    public void SpawnCurentMonster()
    {
        GameObject m = Instantiate(currentMonster, spawner.transform.position, spawner.transform.rotation, spawner.transform);
        currentMonster = m;
        Enemy e = m.GetComponent<Enemy>();
        e.SetMonsterAnimator(spawner.GetComponent<Animator>());
    }


    public void RemoveMonster()
    {
        Destroy(currentMonster);
    }

    public Entity GetMonster()
    {
        return currentMonster.GetComponent<Entity>();
    }

    public void StartBattle()
    {

    }

    public void EndBattle()
    {
        MonsterDeath();
    }

    public void MonsterDeath()
    {
        RemoveMonster();
        currentMonster = null;
       
    }


    public void PlayerDeath()
    {
        
    }
}
