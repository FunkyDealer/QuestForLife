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

    List<Global.Spell> knownSpells;

    public List<Global.Spell> getKnownSpells() => knownSpells;

    void Awake()
    {
        this.Weakness = Global.Type.NONE;
        this.Resistence = Global.Type.NONE;

        movementManager = GetComponent<PlayerMov>();
        direction = Global.FacingDirection.EAST;

        EntityName = "Mage";

        navigationInterFace = Instantiate(NavigationInterFacePrefab, Vector3.zero, Quaternion.identity).GetComponent<NaviagationInterfaceManager>();
        navigationInterFace.getInformation(this, dungeonManager);

        BaseAttackPower = 40;
        knownSpells = new List<Global.Spell>();

        getAllSpells();
    }

    void getAllSpells()
    {

        foreach (var s in DataBase.inst.Spells)
        {
            knownSpells.Add(s.Value);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public BattleInterFaceManager StartBattle()
    {
        navigationInterFace.gameObject.SetActive(false);

        battleInterface = Instantiate(BattleInterfacePrefab, Vector3.zero, Quaternion.identity).GetComponent<BattleInterFaceManager>();
        
        return battleInterface;
    }

    public override void EndBattle()
    {   
        battleInterface = null;

        navigationInterFace.gameObject.SetActive(true);
        movementManager.EndBattle();
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
        Defence = 15;
        Accuracy = 10;
        Dodge = 10;
        Speed = 10;

        currentMoney = 100;

        experienceToNextLevel = 100 * (int)Mathf.Pow((Level + 1), 2) - (100 * (Level + 1));

        gameManager.startingNewGame = false;
    }

    public void gainExp(Enemy enemy)
    {
        int baseExp = enemy.BaseExpReward;
        int Elevel = enemy.Level;
        int multiplier = 3;

        int ammount = (int)(baseExp + (baseExp * multiplier) * Mathf.Log(Elevel, 2));
        Debug.Log($"Gained {ammount} exp!");

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

        experienceToNextLevel = 100 * (int)Mathf.Pow((Level + 1), 2) - (100 * (Level + 1));        

        //Stats up
        maxHealth += Random.Range(5, 21);
        maxMana += Random.Range(5, 11);

        Power += Random.Range(1, 6);
        Defence += Random.Range(2, 6);
        Accuracy += Random.Range(2, 6);
        Dodge += Random.Range(1, 6);
        Speed += Random.Range(1, 6);


        RecoverAll();
    }

    protected override void ReceiveDamage(int attackPower)
    {
        currentHealth -= attackPower;

        Debug.Log($"Player Received {attackPower} damage!");
        Debug.Log($"current health is {currentHealth}");

        if (currentHealth <= 0)
        {
            battleManager.PlayerDeath();
            battleInterface.PlayerDeath();
           // Debug.Log($"Player Died!");
            dead = true;
        }
    }

    public void UseFountain()
    {
        RecoverAll();

        Debug.Log("Player Used fountain, health and mana restaured!");
    }


    void RecoverAll()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
}
