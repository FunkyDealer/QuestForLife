using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{


    [SerializeField]
    public GameManager gameManager;

    [HideInInspector]
    public DungeonManager dungeonManager;

    [HideInInspector]
    public Tile currentTile;
    public Global.FacingDirection direction;
    PlayerMov movementManager;

    void Awake()
    {
        movementManager = GetComponent<PlayerMov>();
        direction = Global.FacingDirection.EAST;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
