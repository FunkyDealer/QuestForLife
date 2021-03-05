using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Player player;

    Vector2 dir;
    Vector2 position;

    DungeonManager dungeon;
    Tile[,] map;
    void Start()
    {
        //dungeon = FindObjectOfType<DungeonManager>();
        //map = dungeon.map;
        position = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MovePlayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MovePlayer(-1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //RODAR CAM
            Quaternion r90 = Quaternion.AngleAxis(-90, Vector3.up);
            this.transform.localRotation *= r90;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //RODAR CAM
            Quaternion r90 = Quaternion.AngleAxis(90, Vector3.up);
            this.transform.localRotation *= r90;
        }
    }

    void MovePlayer(int i)
    {
        this.transform.position += i*this.transform.forward;
    }
}
