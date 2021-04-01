using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    GameObject NavigationInterFacePrefab;
    [SerializeField]
    GameObject BattleInterfacePrefab;

    [HideInInspector]
    NavigationInterfaceManager navigationInterFace;

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

    public delegate void UpdateHealthEvent(int h, int maxH);
    public static event UpdateHealthEvent onHealthUpdate;

    public delegate void UpdateManaEvent(int e, int maxE);
    public static event UpdateManaEvent onManaUpdate;

    public delegate void onLevelUpEvent(int newLevel);
    public static event onLevelUpEvent onLevelUp;

    public Inventory Inventory;
    public GearSlot HatSlot;
    public GearSlot BodySlot;
    public GearSlot BeltSlot;
    public GearSlot RingSlot1;
    public GearSlot RingSlot2;
    public GearSlot WeaponSlot;


    void Awake()
    {
        this.Weakness = Global.Type.NONE;
        this.Resistence = Global.Type.NONE;

        movementManager = GetComponent<PlayerMov>();
        direction = Global.FacingDirection.EAST;

        EntityName = "Mage";



        BaseAttackPower = 40;
        knownSpells = new List<Global.Spell>();

        getAllSpells();

        Inventory = new Inventory(10, this);

        HatSlot = new GearSlot(Global.GearType.HAT, 1, this);
        BodySlot = new GearSlot(Global.GearType.BODYCLOTHING, 2, this);      
        RingSlot1 = new GearSlot(Global.GearType.RING, 3, this);
        RingSlot2 = new GearSlot(Global.GearType.RING, 4, this);
        BeltSlot = new GearSlot(Global.GearType.BELT, 5, this);
        WeaponSlot = new GearSlot(Global.GearType.WEAPON, 6, this);

        navigationInterFace = Instantiate(NavigationInterFacePrefab, Vector3.zero, Quaternion.identity).GetComponent<NavigationInterfaceManager>();
        navigationInterFace.getInformation(this, movementManager, dungeonManager);

        //Inventory
        Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 99);
        Inventory.TryToAddToInventory(DataBase.inst.Consumables[2], 1);

        Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 50);
        Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 10);

        HatSlot.AttemptToPlaceItem(DataBase.inst.Gears[1]);
        BodySlot.AttemptToPlaceItem(DataBase.inst.Gears[2]);
        WeaponSlot.AttemptToPlaceItem(DataBase.inst.Gears[3]);

        RingSlot1.AttemptToPlaceItem(DataBase.inst.Gears[4]);
        RingSlot2.AttemptToPlaceItem(DataBase.inst.Gears[5]);

        Inventory.TryToAddToInventory(DataBase.inst.Gears[6], 1);
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
        battleInterface.AddMessage($"I gained {ammount} experience Point!");

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

        try
        {
            onLevelUp(Level);
        }
        catch { }

        RecoverAll();
    }

    protected override void ReceiveDamage(int attackPower)
    {
        currentHealth -= attackPower;

        //Debug.Log($"Player Received {attackPower} damage!");

        battleInterface.AddMessage($"Ouch! I was attacked for {attackPower} damage!");

        sendUpdateHealth();

        if (currentHealth <= 0)
        {
            battleManager.PlayerDeath();
            battleInterface.PlayerDeath();
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
        sendUpdateHealth();
        sendUpdateMana();
    }

    public override void PerformAction(BattleAction action)
    {
        switch (action)
        {
            case AttackAction a:
                break;
            case CastSpellAction b:

                castSpell(b);
                sendUpdateMana();
                sendUpdateHealth();
                break;
            case ItemUseAction c:

                HealItem item = (HealItem)c.itemToUse;

                switch (item.HealType)
                {
                    case Global.HealType.HEALTH:
                        currentHealth += item.HealAmmount;
                        if (currentHealth > maxHealth) currentHealth = maxHealth;
                        sendUpdateMana();
                        break;
                    case Global.HealType.MANA:
                        currentMana += item.HealAmmount;
                        if (currentMana > maxMana) currentMana = maxMana;
                        sendUpdateMana();
                        break;
                    default:
                        break;
                }
                break;
            case InvestigationAction d:
                break;
            case RunAction e:
                if (e.canRun > 5)
                {
                    Debug.Log("Saiu com sucesso");
                }
                else
                {
                    Debug.Log("Nao saiu");
                }
                break;
        }
    }

    void sendUpdateHealth()
    {
        try
        {
            onHealthUpdate(currentHealth, maxHealth);
        }
        catch
        {

        }
    }

    void sendUpdateMana()
    {
        try
        {
            onManaUpdate(currentMana, maxMana);
        }
        catch
        {

        }
    }

    public void OpenInventory()
    {      
        navigationInterFace.OpenInventory();
    }

    public void HealHealth(int ammount)
    {
        currentHealth += ammount; //Health
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        onHealthUpdate(currentHealth, maxHealth);
    }

    public void RestoreMana(int ammount)
    {
        currentMana += ammount; //mana
        if (currentMana > maxMana) currentMana = maxMana;
        onManaUpdate(currentMana, maxMana);

    }

    public void RestoreHealthAndMana(int ammount)
    {
        currentHealth += ammount; //Health
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        currentMana += ammount; //mana
        if (currentMana > maxMana) currentMana = maxMana;

        onHealthUpdate(currentHealth, maxHealth);
        onManaUpdate(currentMana, maxMana);
    }


    public void AddStatsFromItem(EquipableItem i)
    {
        int h = maxHealth;
        int m = maxMana;

        maxHealth += i.HealthBonus;
        maxMana += i.ManaBonus;

        int hDif = maxHealth - h;
        currentHealth += hDif;
        onHealthUpdate(currentHealth, maxHealth);

        int mDif = maxMana - m;
        currentMana += mDif;
        onManaUpdate(currentMana, maxMana);

        
        Power += i.PowerBonus;
        Defence += i.DefenceBonus;
        Accuracy += i.AccuracyBonus;
        Dodge += i.DodgeBonus;
        Speed += i.SpeedBonus;
    }

    public void RemoveStatsFromItem(EquipableItem i)
    {
        int h = maxHealth;
        int m = maxMana;

        maxHealth -= i.HealthBonus;
        maxMana -= i.ManaBonus;

        int hDif = h - maxHealth;
        currentHealth -= hDif;
        onHealthUpdate(currentHealth, maxHealth);

        int mDif = m - maxMana;
        currentMana -= mDif;
        onManaUpdate(currentMana, maxMana);


        Power -= i.PowerBonus;
        Defence -= i.DefenceBonus;
        Accuracy -= i.AccuracyBonus;
        Dodge -= i.DodgeBonus;
        Speed -= i.SpeedBonus;
    }

}
