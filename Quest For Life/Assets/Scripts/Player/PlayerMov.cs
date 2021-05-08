using System;
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
    bool inCutScene;

    MapManager mapManager;

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
    bool inDungeon;

    bool interactCoolDown;

    void Awake()
    {
        //inDungeon = true;
        inDungeon = false;

        interactCoolDown = false;
        inCutScene = false;

        inputTimer = 0;
        player = GetComponent<Player>();
        movementState = MovementState.WAITINGINPUT;
        inBattle = false;      
    }

    void Start()
    {
        mapManager = player.mapManager;
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

                float v = velocity;
                if (inCutScene) v = velocity / 6;
                

                transform.position += dir * v * Time.deltaTime; //Moving

                float mag = Vector3.Distance(transform.position, nextPosition);
                if (mag < 0.1f)
                {
                    this.transform.position = nextPosition;
                    player.AnnounceMove();
                    interactCoolDown = false;

                    if (inDungeon)
                    {
                        DungeonManager m = (DungeonManager)mapManager;
                        inBattle = m.CheckForEncounter();
                    }
                    if (inBattle) movementState = MovementState.IN_BATTLE;
                    else if (inCutScene) movementState = MovementState.FROZEN;
                    else movementState = MovementState.COOLDOWN;
                }
                break;
            case MovementState.TURNING:               
                Vector3 targetForward = nextRotation * Vector3.forward;
                
                if (Vector3.Dot(transform.forward, targetForward) > 0.99995)
                {
                    transform.localRotation = nextRotation;
                    interactCoolDown = false;
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
        if (Input.GetButtonDown("Inventory")) {
            
            player.OpenInventory();
            movementState = MovementState.FROZEN;
            return;
        }
        if (Input.GetButtonDown("CloseMenu"))
        {
            player.OpenPauseMenu();
            movementState = MovementState.FROZEN;
            return;
        }

        if (Input.GetKeyDown("r"))
        {
            OpenShop();
            return;
        }

        if (Input.GetKeyDown("z"))
        {
            MoveToNextFloor();
            movementState = MovementState.COOLDOWN;
            return;
        }

        if (Input.GetKeyDown("l"))
        {
            player.AddInterfaceMessage($"Testing a message \n 2nd line", TextMessage.MessageSpeed.VERYSLOW);
            movementState = MovementState.COOLDOWN;
            return;
        }

        if (Input.GetKeyDown("k"))
        {
           // Debug.Log("Leveling Up manually");
           
            player.LevelUpManually();
            movementState = MovementState.COOLDOWN;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        

        if (vertical != 0)
        {
            if (vertical > 0) vertical = 1;
            else vertical = -1;

            CheckNewPos(vertical);
        }
        else if (horizontal != 0)
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
        player.rotateCompass(dir);
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

            switch (player.currentMap[(int)newPos.x, (int)newPos.y])
            {
                case FillMapTile f:

                    break;
                case HallMapTile h:
                    CalculateNextPosition(newPos);
                    break;
                case WallTile w:

                switch (w.wallFeature)
                {
                    case WallTile.WallFeature.ShopEntrance:
                        UseShopEntrance((int)newPos.x, (int)newPos.y);     //Use shop Entrance 
                        break;
                    case WallTile.WallFeature.Chest:
                        UseChest((int)newPos.x, (int)newPos.y);
                        break;
                    case WallTile.WallFeature.ShopExit:
                        useShopExit((int)newPos.x, (int)newPos.y);      //Use Shop Exit 
                        break;
                    case WallTile.WallFeature.None:
                        break;
                    default:
                        break;
                }
                break;
                case RoomMapTile r: //if you're trying to walk into a room tile

                switch (r.roomFeature)
                {
                    case RoomMapTile.RoomFeature.Entrance:
                        CalculateNextPosition(newPos);
                        break;
                    case RoomMapTile.RoomFeature.Exit:
                        MoveToNextFloor();
                        break;
                    case RoomMapTile.RoomFeature.LockedExit:
                        UseLockedExit(r);
                        break;
                    case RoomMapTile.RoomFeature.Fountain:
                        UseFountain();
                        break;
                    case RoomMapTile.RoomFeature.Boss:
                        break;
                    case RoomMapTile.RoomFeature.Key:
                        CalculateNextPosition(newPos);
                        break;
                    case RoomMapTile.RoomFeature.Shop:
                        if (player.direction == player.currentMap[(int)newPos.x, (int)newPos.y].OppositeDirection()) //Use Shop
                        {
                            OpenShop();
                        }
                        break;
                    case RoomMapTile.RoomFeature.None:
                        CalculateNextPosition(newPos);
                        break;
                    case RoomMapTile.RoomFeature.EndGame:
                        movementState = MovementState.FROZEN;
                        player.EndGame();
                        break;
                    default:
                        break;
                }
                    break;
                default:
                    break;
            }
    }

    private void UseChest(int x, int y)
    {
        if (player.direction == player.currentMap[x, y].OppositeDirection())
        {
            DungeonManager DM = (DungeonManager)player.mapManager;
            Chest c = DM.Chests[player.currentMap[x, y]].GetComponent<Chest>();
            if (!c.isOpen)
            {
                player.addGold(c.OpenChest(player.Level, player.currentMap[x, y].floor));

                movementState = MovementState.FROZEN;
                StartCoroutine(ResumeMovement(1));
            }
        }
     }

    private void OpenShop()
    {
        movementState = MovementState.FROZEN;
        player.DeactiveNavigationInterface();
        player.gameManager.shopManager.UseShop(player);

    }


    private void useShopExit(int posX, int posY)
    {
        if (player.direction == player.currentMap[posX, posY].OppositeDirection()) 
        {
            //go to Dungeon
            player.gameManager.shopManager.shopExit.Open(MoveFromShopToDungeon, CalculateShopDoor);


            
            inCutScene = true;

            movementState = MovementState.FROZEN;
        }
    }

    private void UseShopEntrance(int posX, int posY)
    {
        if (player.direction == player.currentMap[posX, posY].OppositeDirection())
        {
            //go to Shop

            //ShopEntrance 
            DungeonManager d = (DungeonManager)player.mapManager;
            d.currentShop.Open(MoveFromDungeonToShop, CalculateShopDoor);
                       

            inDungeon = false;
            inCutScene = true;
            movementState = MovementState.FROZEN;
        }
    }

    private void MoveFromDungeonToShop()
    {
        player.gameManager.MovePlayerToShop();
        movementState = MovementState.COOLDOWN;
        inCutScene = false;
    }

    private void MoveFromShopToDungeon()
    {
        player.gameManager.MovePlayerToDungeon();
        movementState = MovementState.COOLDOWN;
        inCutScene = false;
        inDungeon = true;
    }

    void CalculateShopDoor(PhysicalTile door)
    {
        Vector3 targetPos = door.gameObject.transform.position;

        targetPos = new Vector3(targetPos.x, player.gameObject.transform.position.y, targetPos.z);

        nextPosition = targetPos;
        movementState = MovementState.MOVING;
    }

    void CalculateNextPosition(Vector2 newPos)
    {
        Vector3 targetPos = player.mapManager.FreeTiles[player.currentMap[(int)newPos.x, (int)newPos.y]].transform.position;

        targetPos = new Vector3(targetPos.x, player.gameObject.transform.position.y, targetPos.z);

        //this.transform.position = targetPos;
        player.currentTile = player.currentMap[(int)newPos.x, (int)newPos.y];
        nextPosition = targetPos;
        movementState = MovementState.MOVING;
    }

    void UseLockedExit(RoomMapTile r)
    {
        DungeonManager m = (DungeonManager)player.mapManager;

        LockedStairs l = m.exit.GetComponent<LockedStairs>();
        if (l.Locked)
        {
            if (l.CheckKeys(player.keys))
            {
                movementState = MovementState.FROZEN;
                player.AddInterfaceMessage("You Opened the Stairs to the next Floor", TextMessage.MessageSpeed.NORMAL);
                StartCoroutine(ResumeMovement(2));
            }
            else
            {
                if (!interactCoolDown)
                {
                    player.AddInterfaceMessage("You lack all the keys to open this", TextMessage.MessageSpeed.NORMAL);
                    interactCoolDown = true;
                }
                movementState = MovementState.COOLDOWN;
            }
        } else
        {
            MoveToNextFloor();
        }
       
    }

    void UseExit()
    {
        player.EnterExit();
        movementState = MovementState.COOLDOWN;
    }

    void MoveToNextFloor()
    {
        GameObject o = Instantiate(DataBase.inst.confirmationInterface, Vector3.zero, Quaternion.identity);
        GenericConfirmInterface g = o.GetComponent<GenericConfirmInterface>();

        g.Init(UseExit, ResumeMovement, "Move to next floor?");
        movementState = MovementState.FROZEN;
    }


    public void EndBattle()
    {
        inBattle = false;
        movementState = MovementState.COOLDOWN;
    }

    void UseFountain()
    {
        if (player.UseFountain()) movementState = MovementState.COOLDOWN;
    }

    public void ResumeMovement()
    {
        movementState = MovementState.COOLDOWN;
    }

    public IEnumerator ResumeMovement(float time)
    {
        yield return new WaitForSeconds(time);

        movementState = MovementState.COOLDOWN;
    }


    public void enterDungeon()
    {
        inDungeon = true;
    }

    public void leaveDungeon()
    {
        inDungeon = false;
    }
}
