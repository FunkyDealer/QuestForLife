using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Tile[,] currentMap;

    [SerializeField]
    GameObject NavigationInterFacePrefab;
    [SerializeField]
    GameObject BattleInterfacePrefab;

    [HideInInspector]
    NavigationInterfaceManager navigationInterFace;

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public MapManager mapManager;

    [HideInInspector]
    public Tile currentTile;

    [SerializeField]
    int x;
    [SerializeField]
    int y;

   // [HideInInspector]
    public Global.FacingDirection direction;
    PlayerMov movementManager;
    public PlayerMov MovementManager => movementManager;

    int currentMoney;
    int currentExperience;
    public int Experience => currentExperience;
    int totalExperience;

    int experienceToNextLevel;
    public int toNextLevelExp => experienceToNextLevel;

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

    public delegate void OnStatsChange();
    public static event OnStatsChange onStatsChange;

    public delegate void OnGoldChange(int newAmmount);
    public static event OnGoldChange onGoldChange;

    public delegate void OnPlayerMove(Vector2 newPos);
    public static event OnPlayerMove onPlayerMove;

    public delegate void onExperienceChange();
    public static event onExperienceChange onExperience;

    public Inventory Inventory;
    public GearSlot HatSlot;
    public GearSlot BodySlot;
    public GearSlot BeltSlot;
    public GearSlot RingSlot1;
    public GearSlot RingSlot2;
    public GearSlot WeaponSlot;
    
    public CompassController compass;

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


        navigationInterFace = Instantiate(NavigationInterFacePrefab, Vector3.zero, Quaternion.identity).GetComponent<NavigationInterfaceManager>();
        navigationInterFace.getInformation(this, movementManager, mapManager);

        Inventory = new Inventory(10, this);

        HatSlot = new GearSlot(Global.GearType.HAT, 1, this);
        BodySlot = new GearSlot(Global.GearType.BODYCLOTHING, 2, this);      
        RingSlot1 = new GearSlot(Global.GearType.RING, 3, this);
        RingSlot2 = new GearSlot(Global.GearType.RING, 4, this);
        BeltSlot = new GearSlot(Global.GearType.BELT, 5, this);
        WeaponSlot = new GearSlot(Global.GearType.WEAPON, 6, this);

        Debug.Log("Inventory and Hud Created");



      
    }


    // Start is called before the first frame update
    void Start()
    {
        compass.Initiate(this, direction);

      
    }

    void getAllSpells()
    {
        foreach (var s in DataBase.inst.Spells)
        {
            knownSpells.Add(s.Value);
        }
    }

    public void DeactiveNavigationInterface()
    {
        navigationInterFace.gameObject.SetActive(false);
    }

    public void ActivateNavigationInterface()
    {
        navigationInterFace.gameObject.SetActive(true);
    }

    public BattleInterFaceManager StartBattle()
    {
        DeactiveNavigationInterface();

        battleInterface = Instantiate(BattleInterfacePrefab, Vector3.zero, Quaternion.identity).GetComponent<BattleInterFaceManager>();       

        return battleInterface;
    }

    public override void EndBattle()
    {   
        battleInterface = null;

        ActivateNavigationInterface();
        movementManager.EndBattle();
    }

    // Update is called once per frame
    void Update()
    {
        x = currentTile.x;
        y = currentTile.y;

    }

   

    public void EnterExit()
    {
        DungeonManager m = (DungeonManager)mapManager;
        m.CreateNewFloor();
        navigationInterFace.CreateNewMinimap();

    }

    public void Spawn(Vector3 worldPos, Vector3 mapPos, Tile[,] map, GameManager gameManager, DungeonManager dungeonManager)
    {
        gameObject.transform.position = worldPos;
        currentMap = map;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
        this.gameManager = gameManager;
        this.mapManager = dungeonManager;        

        StartNewPlayer();
    }



    public void Move(Vector3 worldPos, Vector2 mapPos, Tile[,] map, MapManager mapManager)
    {
        this.mapManager = mapManager;
        currentMap = map;
        gameObject.transform.position = worldPos;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
        navigationInterFace.CreateNewMinimap();
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

        //Inventory
        Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 3); //add Small health Potions
        Inventory.TryToAddToInventory(DataBase.inst.Consumables[2], 1); //add small mana Potion

        //  Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 50); //add 50 health potions
        //  Inventory.TryToAddToInventory(DataBase.inst.Consumables[1], 10); //add 10 health potions

        HatSlot.AttemptToPlaceItem(DataBase.inst.Gears[1]);
        BodySlot.AttemptToPlaceItem(DataBase.inst.Gears[2]);
        WeaponSlot.AttemptToPlaceItem(DataBase.inst.Gears[3]);

        //RingSlot1.AttemptToPlaceItem(DataBase.inst.Gears[4]);
        //RingSlot2.AttemptToPlaceItem(DataBase.inst.Gears[5]);

        //Inventory.TryToAddToInventory(DataBase.inst.Gears[6], 1);

        gameManager.startingNewGame = false;
    }

    public bool gainExp(Enemy enemy)
    {
        bool levelUp = false; ;
        int baseExp = enemy.BaseExpReward;
        int Elevel = enemy.Level;
        int multiplier = 3;

        int ammount = (int)(baseExp + (baseExp * multiplier) * Mathf.Log(Elevel, 2));
        Debug.Log($"Gained {ammount} exp!");
        battleInterface.AddMessage($"I gained {ammount} experience Points!", TextMessage.MessageSpeed.NORMAL);

        currentExperience += ammount;
        totalExperience += ammount;            

        levelUp = LevelUp();        

        try
        {
            onExperience();
        } catch (System.NullReferenceException)
        {

        }

        return levelUp;
    }

    public bool LevelUp()
    {
        if (currentExperience >= experienceToNextLevel)
        {
        battleInterface.AddMessage($"I'm feeling really confident after that battle, maybe I've gotten stronger...", TextMessage.MessageSpeed.VERYSLOW);
            Level++;
        currentExperience = currentExperience - experienceToNextLevel;

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
            onStatsChange();
        }
        catch { }

        RecoverAll();

            return true;
        }

        return false;
    }

    protected override void ReceiveDamage(int attackPower)
    {
        currentHealth -= attackPower;

        //Debug.Log($"Player Received {attackPower} damage!");

        battleInterface.AddMessage($"Ouch! I was attacked for {attackPower} damage!", TextMessage.MessageSpeed.NORMAL);

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

        Debug.Log("Player Used fountain, health and mana restored!");
    }


    void RecoverAll()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        sendUpdateHealth();
        sendUpdateMana();
    }

    public override float PerformAction(BattleAction action, Entity enemy)
    {
        float animationTime = 0;
        switch (action)
        {
            case AttackAction a:
                animationTime += 0.5f;
                animationTime += enemy.ReceiveAction(action);
                break;
            case CastSpellAction b:

                castSpell(b);
                sendUpdateMana();
                sendUpdateHealth();

                animationTime += 0.5f;
                animationTime += enemy.ReceiveAction(action);
                break;
            case ItemUseAction c:
                Item i = Inventory.getSlot(c.slot).getItem();

                Inventory.ConsumeItem(c.slot);
                battleInterface.AddMessage($"The mage used a {i.Name}", TextMessage.MessageSpeed.FAST);

                animationTime += 0.5f;
                animationTime += enemy.ReceiveAction(action);
                break;
            case InvestigationAction d:
                animationTime += 0.5f;
                animationTime += enemy.ReceiveAction(action);
                break;
            case RunAction e:

                animationTime += 0.5f;
                animationTime += enemy.ReceiveAction(action);
                break;
        }

        return animationTime;
    }

    public override float ReceiveAction(BattleAction action)
    {
        float animationTime = 0;
        switch (action)
        {
            case AttackAction a:

                float tohit = (a.user.Accuracy * a.AttackAccuracy / a.Target.Dodge);
                int ToHit = (int)tohit;

                if (ToHit > 100)
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animationTime += 1.3f;
                }
                else if (ToHit > Random.Range(0, 100))
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animationTime += 1.3f;
                }
                else
                {
                    battleInterface.AddMessage($"The {a.user}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                    animationTime += 1.3f;
                }

                break;
            case CastSpellAction b:

                float tohit2 = (b.user.Accuracy * b.spell.Accuracy / b.Target.Dodge);
                int ToHit2 = (int)tohit2;

                if (tohit2 > 100)
                {
                    ReceiveSpellAttack(b);
                    animationTime += 1f;
                }
                else if (ToHit2 > Random.Range(0, 100))
                {
                    ReceiveSpellAttack(b);
                    animationTime += 1f;
                }
                else
                {
                    battleInterface.AddMessage($"The {b.user}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                    animationTime += 1f;
                }
                break;
            case ItemUseAction c:
                animationTime += 1f;
                break;
            case InvestigationAction d:
                animationTime += 1f;
                break;
            case RunAction e:

                float chanceToEscape = ((e.speed * 40) / this.Speed) + 30;
                Debug.Log($"Chance to escape: {chanceToEscape}");
                if (chanceToEscape > 100)
                {
                    battleManager.RunAway();
                    animationTime += 1f;
                }
                else if (chanceToEscape > Random.Range(0, 100))
                {
                    battleManager.RunAway();
                    animationTime += 1f;
                }
                else
                {
                    battleInterface.AddMessage($"You Failed at running away!", TextMessage.MessageSpeed.NORMAL);
                    animationTime += 1f;
                }

                break;
        }

        return animationTime;
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

        try
        {
            onStatsChange();
        }
        catch 
        {

        }       
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

        try
        {
            onStatsChange();
        }
        catch
        {

        }
    }

    public void rotateCompass(float dir) => compass.rotate(dir);
     
    public void addGold(int ammount)
    {
        currentMoney += ammount;
        updateGold();
    }

    public void spendGold(int ammount)
    {
        currentMoney -= ammount;
        updateGold();
    }

    public int currentGold => currentMoney;

    void updateGold()
    {
        try
        {
            onGoldChange(currentMoney);
        } catch
        {

        }
    }

    public void AnnounceMove()
    {
        try
        {
            onPlayerMove(new Vector2(currentTile.x, currentTile.y));
        }
        catch
        {

        }
    }


}
