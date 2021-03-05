using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Player player;

    Vector2 dir;
    [SerializeField]
    float inputDelay = 0.5f;
    float inputTimer;
    bool inputDelayOn = false;

    DungeonManager dungeonManager;

    void Awake()
    {
        inputTimer = 0;
    }

    void Start()
    {
        //dungeon = FindObjectOfType<DungeonManager>();
        //map = dungeon.map;
    }

    void Update()
    {
        input();




    }

    void input()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (vertical != 0 && inputTimer <= 0)
        {
            if (vertical > 0) vertical = 1;
            else vertical = -1;
            MovePlayer(vertical);
            inputTimer = inputDelay;
            
        }
        if (horizontal != 0 && inputTimer <= 0)
        {
            if (horizontal > 0) horizontal = 1;
            else horizontal = -1;
            RotatePlayer(horizontal);
            inputTimer = inputDelay;
        }

        if (inputTimer > 0) inputTimer -= Time.deltaTime;
    }

    void MovePlayer(float i)
    {
        //Debug.Log($"Moving: {i}");
        this.transform.position += i*this.transform.forward;
    }

    void RotatePlayer(float dir)
    {
        Debug.Log($"rotating: {dir}");
        //RODAR CAM
        Quaternion r90 = Quaternion.AngleAxis(90 * dir, Vector3.up);
        this.transform.localRotation *= r90;
    }
}
