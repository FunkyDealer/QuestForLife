using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    GameObject NavigationInterFacePrefab;
    [SerializeField]
    GameObject BattleInterfacePrefab;

    [HideInInspector]
    NaviagationInterfaceManager navigationInterFace;
    [HideInInspector]
    BattleInterFaceManager battleInterface;

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public DungeonManager dungeonManager;

    [HideInInspector]
    public Tile currentTile;
    [HideInInspector]
    public Global.FacingDirection direction;
    PlayerMov movementManager;

    [HideInInspector]
    public int currentMoney;
    int currentExperience;
    int totalExperience;

    int experienceToNextLevel;


    int minHealthGain = 5;
    int maxHealthGain = 20;

    int minManaGain = 5;
    int maxManaGain = 10;

    int minPowerGain = 1;
    int maxPowerGain = 5;

    int minDefenseGain = 2;
    int maxDefenseGain = 5;

    int minAccuracyGain = 2;
    int maxAccuracyGain = 5;

    int minDodgeGain = 1;
    int maxDodgeGain = 5;

    int minSpeedGain = 1;
    int maxSpeedGain = 5;


    void Awake()
    {
        movementManager = GetComponent<PlayerMov>();
        direction = Global.FacingDirection.EAST;

        EntityName = "Mage";

        navigationInterFace = Instantiate(NavigationInterFacePrefab, Vector3.zero, Quaternion.identity).GetComponent<NaviagationInterfaceManager>();
        navigationInterFace.getInformation(this, dungeonManager);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartBattle()
    {
        navigationInterFace.gameObject.SetActive(false);

        battleInterface = Instantiate(BattleInterfacePrefab, Vector3.zero, Quaternion.identity).GetComponent<BattleInterFaceManager>();
       // battleInterface.getInformation(this, , gameManager);
        gameManager.MonsterCamera.SetActive(true);
    }

    public void EndBattle()
    {   
        Destroy(battleInterface.gameObject);
        battleInterface = null;
        gameManager.MonsterCamera.SetActive(false);

        navigationInterFace.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void EnterExit()
    {
        dungeonManager.CreateNewFloor();


    }

    public void Spawn(Vector3 worldPos, Vector3 mapPos, Tile[,] map, GameManager gameManager, DungeonManager dungeonManager)
    {
        gameObject.transform.position = worldPos;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
        this.gameManager = gameManager;
        this.dungeonManager = dungeonManager;

        StartNewPlayer();
    }

    public void Move(Vector3 worldPos, Vector3 mapPos, Tile[,] map)
    {

        gameObject.transform.position = worldPos;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
    }


    void StartNewPlayer()
    {
        Level = 1;

        maxHealth = 100;
        currentHealth = maxHealth;
        maxMana = 50;
        currentMana = maxMana;

        Power = 10;
        Defence = 10;
        Accuracy = 10;
        Dodge = 10;
        Speed = 10;

        currentMoney = 100;

        experienceToNextLevel = 100 * (int)Mathf.Pow((Level + 1), 2) - (100 * (Level + 1));

        gameManager.startingNewGame = false;
    }

    public void gainExp(int ammount)
    {
        currentExperience += ammount;
        totalExperience += ammount;

        if (currentExperience >= experienceToNextLevel)
        {
            int excessExp = currentExperience - experienceToNextLevel;
            

            LevelUp(excessExp);
        }
    }

    public void LevelUp(int excessXp)
    {
        Level ++;
        currentExperience = excessXp;

        //Stats up
    }

}
