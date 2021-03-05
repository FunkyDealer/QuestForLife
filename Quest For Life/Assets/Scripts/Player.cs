using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    enum FacingDirection
    {
        North,
        East,
        West,
        South
    }

    [SerializeField]
    public GameManager gameManager;

    [HideInInspector]
    public DungeonManager dungeonManager;

    [HideInInspector]
    Tile currentTile;
    FacingDirection direction;
    PlayerMov movementManager;

    void Awake()
    {
        movementManager = GetComponent<PlayerMov>();
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
