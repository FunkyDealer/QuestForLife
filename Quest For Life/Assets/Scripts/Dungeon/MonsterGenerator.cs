using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{  
    DungeonManager dungeonManager;
    Player player;
    int currentFloor;

    void Awake()
    {
    }

    void Start()
    {
    }

    public void Initiate(DungeonManager dungeonManager, Player player, int floor)
    {
        this.dungeonManager = dungeonManager;
        this.player = player;
        this.currentFloor = floor;

        generateMonster();
    }

    void generateMonster()
    {
        List<int> floorMonsters = generateFloorMonsterList();
        Debug.Log($"floor monsters list generated - size: {floorMonsters.Count}");

        Global.DungeonMonsterInfo monster = DataBase.inst.Monsters[floorMonsters[Random.Range(0, floorMonsters.Count)]];

        Debug.Log($"Monster type Generated: {monster.Name}, Generating Prefab...");

        GameObject o = DataBase.inst.MonsterPrefabs[monster.id];
        o.GetComponent<Enemy>().getStats(monster, currentFloor);
        o.name = $"{monster.Name} - lvl {currentFloor}";       
        MonsterRoomManager.inst.GetMonster(o);  //Send monster Object to Monster Room for Spawning

        Debug.Log("Finished Monster generation.");

        Finish();
    }
    
    List<int> generateFloorMonsterList()
    {        
        List<int> floorMonsters = new List<int>();
        //Debug.Log($"generating Floor List from list with {DataBase.inst.Monsters.Count} entries in level {currentFloor}");
        foreach (var m in DataBase.inst.Monsters)
        {
            //Debug.Log($"checking a {m.Value.Name}: min Floor {m.Value.minFloor} max floor {m.Value.maxFloor}");
            if (m.Value.minFloor <= currentFloor && m.Value.maxFloor >= currentFloor) {
                floorMonsters.Add(m.Key);
                //Debug.Log($"this floor contains a {m.Value.Name}"); 
            }
        }

        return floorMonsters;
    }

    void Finish()
    {
        Destroy(this);
    }





    // Update is called once per frame
    void Update()
    {
        
    }



}
