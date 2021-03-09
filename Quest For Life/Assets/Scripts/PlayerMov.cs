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


    void Awake()
    {
        inputTimer = 0;
        player = GetComponent<Player>();
    }

    void Start()
    {

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
            
             CheckNewPos(vertical);
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

    void MovePlayer(Vector2 newPos)
    {
        Vector3 targetPos = player.dungeonManager.FreeTiles[player.dungeonManager.map[(int)newPos.x, (int)newPos.y]].transform.position;

        targetPos = new Vector3(targetPos.x, player.gameObject.transform.position.y, targetPos.z);

        this.transform.position = targetPos;
        player.currentTile = player.dungeonManager.map[(int)newPos.x, (int)newPos.y];
    }

    void RotatePlayer(float dir)
    {
        switch (player.direction)
        {
            case Player.FacingDirection.North:
                if (dir > 0) player.direction = Player.FacingDirection.East;
                else player.direction = Player.FacingDirection.West;
                break;
            case Player.FacingDirection.East:
                if (dir > 0) player.direction = Player.FacingDirection.South;
                else player.direction = Player.FacingDirection.North;
                break;
            case Player.FacingDirection.West:
                if (dir > 0) player.direction = Player.FacingDirection.North;
                else player.direction = Player.FacingDirection.South;
                break;
            case Player.FacingDirection.South:
                if (dir > 0) player.direction = Player.FacingDirection.West;
                else player.direction = Player.FacingDirection.East;
                break;
        }

        //ROTATE CAM
        Quaternion r90 = Quaternion.AngleAxis(90 * dir, Vector3.up);
        this.transform.localRotation *= r90;
    }

    void CheckNewPos(float i)
    {
        Vector2 newPos = new Vector2(player.currentTile.x, player.currentTile.y);

        switch (player.direction)
        {
            case Player.FacingDirection.North:
                newPos = new Vector2(player.currentTile.x-i, player.currentTile.y);
                break;
            case Player.FacingDirection.East:
                newPos = new Vector2(player.currentTile.x, player.currentTile.y+i);
                break;
            case Player.FacingDirection.West:
                newPos = new Vector2(player.currentTile.x, player.currentTile.y-i);
                break;
            case Player.FacingDirection.South:
                newPos = new Vector2(player.currentTile.x+i, player.currentTile.y);
                break;
        }

        switch (player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type)
        {
            case Tile.Type.filling:

                break;
            case Tile.Type.hall:
                MovePlayer(newPos);
                break;
            case Tile.Type.none:

                break;
            case Tile.Type.room:
                MovePlayer(newPos);
                break;
            default:
                Debug.Log(player.dungeonManager.map[(int)newPos.x, (int)newPos.y].type);
                break;
        }

    }


}
