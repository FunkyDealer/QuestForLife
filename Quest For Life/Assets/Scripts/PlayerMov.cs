using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    Player player;

    Vector2 dir;
    Vector3 nextPosition;
    Quaternion nextRotation;
    float nextRotationDir;

    [SerializeField]
    float inputDelay = 0.1f;    
    [SerializeField]
    float velocity = 50f;
    [SerializeField]
    float turningVelocity = 50f;
    float inputTimer;
    bool inputDelayOn = false;

    DungeonManager dungeonManager;

    enum MovementState
    {
        WAITINGINPUT,
        MOVING,
        TURNING,
        FROZEN,
        IN_BATTLE,
        COOLDOWN
    }
    MovementState movementState;

    bool inBattle;

    void Awake()
    {
        inputTimer = 0;
        player = GetComponent<Player>();
        movementState = MovementState.WAITINGINPUT;
        inBattle = false;
      
    }

    void Start()
    {
        dungeonManager = player.dungeonManager;
    }

    void Update()
    {
        switch (movementState)
        {
            case MovementState.WAITINGINPUT:
                input();
                break;
            case MovementState.MOVING:

                break;
            case MovementState.TURNING:
                transform.localRotation = Quaternion.Lerp(transform.localRotation, nextRotation, turningVelocity * Time.deltaTime);
                break;
            case MovementState.FROZEN:
                break;
            case MovementState.COOLDOWN:
                if (inputTimer < inputDelay) inputTimer += Time.deltaTime;
                else
                {
                    inputTimer = 0;
                    movementState = MovementState.WAITINGINPUT;
                }
                break;
            case MovementState.IN_BATTLE:

                //if (Input.GetKeyDown(KeyCode.Escape))
                //{
                //    inBattle = false;
                //    movementState = MovementState.COOLDOWN;
                //    player.EndBattle();
                //}

                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        switch (movementState)
        {
            case MovementState.WAITINGINPUT:
                break;
            case MovementState.MOVING:

                //Direction to move
                Vector3 dir = nextPosition - transform.position;
                dir.Normalize();

                transform.position += dir * velocity * Time.deltaTime; //Moving

                float mag = Vector3.Distance(transform.position, nextPosition);
                if (mag < 0.1f)
                {
                    this.transform.position = nextPosition;

                    inBattle = dungeonManager.CheckForEncounter();
                    if (inBattle) movementState = MovementState.IN_BATTLE;
                    else movementState = MovementState.COOLDOWN;
                }

                break;
            case MovementState.TURNING:               
                Vector3 targetForward = nextRotation * Vector3.forward;
                
                if (Vector3.Dot(transform.forward, targetForward) > 0.99995)
                {
                    transform.localRotation = nextRotation;
                    movementState = MovementState.COOLDOWN;
                }                

                break;
            case MovementState.FROZEN:
                break;
            case MovementState.IN_BATTLE:
                break;
            default:
                break;
        }
    }

    void input()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (vertical != 0)
        {
            if (vertical > 0) vertical = 1;
            else vertical = -1;

            CheckNewPos(vertical);

        }
        if (horizontal != 0)
        {
            if (horizontal > 0) horizontal = 1;
            else horizontal = -1;
            RotatePlayer(horizontal);
        }
    }

    void RotatePlayer(float dir)
    {
        switch (player.direction)
        {
            case Global.FacingDirection.NORTH:
                if (dir > 0) player.direction = Global.FacingDirection.EAST;
                else player.direction = Global.FacingDirection.WEST;
                break;
            case Global.FacingDirection.EAST:
                if (dir > 0) player.direction = Global.FacingDirection.SOUTH;
                else player.direction = Global.FacingDirection.NORTH;
                break;
            case Global.FacingDirection.WEST:
                if (dir > 0) player.direction = Global.FacingDirection.NORTH;
                else player.direction = Global.FacingDirection.SOUTH;
                break;
            case Global.FacingDirection.SOUTH:
                if (dir > 0) player.direction = Global.FacingDirection.WEST;
                else player.direction = Global.FacingDirection.EAST;
                break;
        }

        //nextRotationDir = dir;
        Quaternion r90 = Quaternion.AngleAxis(90 * dir, Vector3.up);
        nextRotation = transform.localRotation * r90;
        movementState = MovementState.TURNING;
    }

    void CheckNewPos(float i)
    {
        Vector2 newPos = new Vector2(player.currentTile.x, player.currentTile.y);

        switch (player.direction)
        {
            case Global.FacingDirection.NORTH:
                newPos = new Vector2(player.currentTile.x - i, player.currentTile.y);
                break;
            case Global.FacingDirection.EAST:
                newPos = new Vector2(player.currentTile.x, player.currentTile.y + i);
                break;
            case Global.FacingDirection.WEST:
                newPos = new Vector2(player.currentTile.x, player.currentTile.y - i);
                break;
            case Global.FacingDirection.SOUTH:
                newPos = new Vector2(player.currentTile.x + i, player.currentTile.y);
                break;
        }

        switch (player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type)
        {
            case Tile.Type.filling:

                break;
            case Tile.Type.hall:
                CalculateNextPosition(newPos);
                break;
            case Tile.Type.none:

                break;
            case Tile.Type.room:
                switch (player.dungeonManager.map[(int)newPos.x, (int)newPos.y].feature)
                {
                    case Tile.Feature.Entrance:
                        CalculateNextPosition(newPos);
                        break;
                    case Tile.Feature.Exit:
                        MoveToNextFloor(newPos);
                        Debug.Log(player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type);
                        break;
                    case Tile.Feature.LockedExit:
                        MoveToNextFloor(newPos);
                        Debug.Log(player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type);
                        break;
                    case Tile.Feature.Shop:
                        break;
                    case Tile.Feature.Fountain:
                        break;
                    case Tile.Feature.Chest:
                        break;
                    case Tile.Feature.Boss:
                        break;
                    case Tile.Feature.Key:
                        break;
                    case Tile.Feature.None:
                        CalculateNextPosition(newPos);                        
                        break;
                    default:
                        break;
                }
                
                break;
            default:
                Debug.Log(player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type);
                break;
        }
    }

    void CalculateNextPosition(Vector2 newPos)
    {
        Vector3 targetPos = player.dungeonManager.FreeTiles[player.dungeonManager.map[(int)newPos.x, (int)newPos.y]].transform.position;

        targetPos = new Vector3(targetPos.x, player.gameObject.transform.position.y, targetPos.z);

        //this.transform.position = targetPos;
        player.currentTile = player.dungeonManager.map[(int)newPos.x, (int)newPos.y];
        nextPosition = targetPos;
        movementState = MovementState.MOVING;
    }

    void MoveToNextFloor(Vector2 newPos)
    {
        player.EnterExit();
    }


    public void EndBattle()
    {
        inBattle = false;
        movementState = MovementState.COOLDOWN;
    }
}
