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

    //Health and Mana
    [HideInInspector]
    public int ExtraMaxHealth = 0;
    [HideInInspector]
    public int ExtraMaxMana = 0;

    //Other Stats
    [HideInInspector]
    public int ExtraPower = 0;
    [HideInInspector]
    public int ExtraDefence = 0;
    [HideInInspector]
    public int ExtraAccuracy = 0;
    [HideInInspector]
    public int ExtraDodge = 0;
    [HideInInspector]
    public int ExtraSpeed = 0;

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
    Dictionary<Global.Spell, bool> learnableSpells;

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

    public delegate void FloorUpdateEvent(int floor);
    public static event FloorUpdateEvent onFloorChange;

    public Inventory Inventory;
    public GearSlot HatSlot;
    public GearSlot BodySlot;
    public GearSlot BeltSlot;
    public GearSlot RingSlot1;
    public GearSlot RingSlot2;
    public GearSlot WeaponSlot;
    
    [HideInInspector]
    public CompassController compass;

    int currentFloor;

    public List<int> keys;

    //Sound
    [SerializeField]
    AudioSource SpellVoicePlayer;

    public enum Location
    {
        DUNGEON,
        SHOP,
        FINALZONE
    }
    public Location location;

    void Awake()
    {
        currentFloor = 1;

        keys = new List<int>();        

        this.Weakness = Global.Type.NONE;
        this.Resistence = Global.Type.NONE;

        movementManager = GetComponent<PlayerMov>();
        direction = Global.FacingDirection.EAST;        

        EntityName = "Mage";

        BaseAttackPower = 40;
        learnableSpells = new Dictionary<Global.Spell, bool>();
        knownSpells = new List<Global.Spell>();       


        navigationInterFace = Instantiate(NavigationInterFacePrefab, Vector3.zero, Quaternion.identity).GetComponent<NavigationInterfaceManager>();
        navigationInterFace.getInformation(this, movementManager, mapManager);

        Inventory = new Inventory(10, this);

        HatSlot = new GearSlot(Global.GearType.HAT, 1, this);
        BodySlot = new GearSlot(Global.GearType.BODYCLOTHING, 2, this);      
        RingSlot1 = new GearSlot(Global.GearType.RING, 3, this);
        RingSlot2 = new GearSlot(Global.GearType.RING, 4, this);
        BeltSlot = new GearSlot(Global.GearType.BELT, 5, this);
        WeaponSlot = new GearSlot(Global.GearType.WEAPON, 6, this);

        location = Location.DUNGEON;

      
    }

    // Start is called before the first frame update
    void Start()
    {
        compass.Initiate(this, direction);

        onFloorChange(currentFloor);
    }

    void getLearnableSpells()
    {
        foreach (var s in DataBase.inst.Spells)
        {
            if (s.Value.PlayerLearn) learnableSpells.Add(s.Value, false);
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
        if (m.Floor < DataBase.inst.FinalFloor)
        {
            m.CreateNewFloor();
            currentFloor++;
            navigationInterFace.CreateNewMinimap();
            onFloorChange(currentFloor);
            if (m.Floor == DataBase.inst.FinalFloor) AddInterfaceMessage($"You Climb to a new Floor\nIt feels life the end is near...", TextMessage.MessageSpeed.VERYSLOW);
            else AddInterfaceMessage($"You Climb to a new Floor", TextMessage.MessageSpeed.VERYSLOW);

            location = Location.DUNGEON;

            keys.Clear();
            navigationInterFace.ClearKeyHolder();
        }
        else if (m.Floor == DataBase.inst.FinalFloor)
        {
            currentFloor++;
            m.CreateFinalZone();
            navigationInterFace.CreateNewMinimap();
            onFloorChange(currentFloor);
            movementManager.leaveDungeon();
            AddInterfaceMessage($"You Climb to a new Floor\nYou feel like this is it.", TextMessage.MessageSpeed.VERYSLOW);

            location = Location.FINALZONE;

            keys.Clear();
            navigationInterFace.ClearKeyHolder();
        }
    }

    public void Spawn(Vector3 worldPos, Vector3 mapPos, Tile[,] map, GameManager gameManager, DungeonManager dungeonManager, SaveData data = null)
    {
        gameObject.transform.position = worldPos;
        currentMap = map;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
        this.gameManager = gameManager;
        this.mapManager = dungeonManager;

        if (data == null) StartPlayer();
        else LoadPlayer(data);
    }

    public void Move(Vector3 worldPos, Vector2 mapPos, Tile[,] map, MapManager mapManager)
    {
        this.mapManager = mapManager;
        currentMap = map;
        gameObject.transform.position = worldPos;
        currentTile = map[(int)mapPos.x, (int)mapPos.y];
        navigationInterFace.CreateNewMinimap();
    }

    public void TurnPlayer(Global.FacingDirection dir)
    {
        direction = dir;
        RotatePlayer(dir);
        compass.SetDirectionFast(dir);
    }

    void RotatePlayer(Global.FacingDirection dir)
    {
        switch (dir)
        {
            case Global.FacingDirection.NORTH:
                transform.localEulerAngles = new Vector3(0, -90, 0);
              // transform.localRotation = new Quaternion(0, 90, 0, 0);
                break;
            case Global.FacingDirection.EAST:
                transform.localEulerAngles = new Vector3(0, 0, 0);
              //  transform.localRotation = new Quaternion(0, 0, 0, 0);
                break;
            case Global.FacingDirection.WEST:
                transform.localEulerAngles = new Vector3(0, 180, 0);
              //  transform.localRotation = new Quaternion(0, -90, 0, 0);
                break;
            case Global.FacingDirection.SOUTH:
                transform.localEulerAngles = new Vector3(0, 90, 0);
              //  transform.localRotation = new Quaternion(0, 180, 0, 0);
                break;
            default:
                break;
        }
    }

    void StartPlayer()
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

        FillNewInventory();      

        getLearnableSpells();

        gameManager.startingNewGame = false;
    }

    void LoadPlayer(SaveData data)
    {
        Level = data.playerData.currentLevel;
        maxHealth = data.playerData.maxHp;
        currentHealth = data.playerData.currentHp;
        maxMana = data.playerData.maxMana;
        currentMana = data.playerData.currentMana;

        Power = data.playerData.Power;
        Defence = data.playerData.Defense;
        Accuracy = data.playerData.Accuracy;
        Dodge = data.playerData.Dodge;
        Speed = data.playerData.Speed;

        currentMoney = data.playerData.CurrentGold;
        currentExperience = data.playerData.CurrentExp;
        experienceToNextLevel = 100 * (int)Mathf.Pow((Level + 1), 2) - (100 * (Level + 1));

        currentFloor = data.mapData.floor;

        if (data.playerData.inFinalZone)
        {
            location = Location.FINALZONE;
            movementManager.leaveDungeon();
        }
        else if (data.playerData.inShop)
        {
            location = Location.SHOP;
            movementManager.leaveDungeon();
            mapManager = gameManager.shopManager;
        }

        foreach (var k in data.playerData.keys)
        {
            keys.Add(k);
        }

        getLearnableSpells();

        foreach (var s in data.playerData.knownSpells)
        {
            Global.Spell spell = DataBase.inst.Spells[s];
            knownSpells.Add(spell);
            learnableSpells[spell] = true;
        }

        LoadItems(data.inventoryData);

        gameManager.startingNewGame = false;
    }

    void FillNewInventory()
    {
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
    }

    void LoadItems(InventoryData data)
    {
        for (int i = 0; i < Inventory.Slots.Length; i++)
        {
            if (data.inventoryIds[i] != -1)
            {
                if (data.inventoryType[i] == 0) Inventory.ForceIntoSlot(DataBase.inst.Consumables[data.inventoryIds[i]], data.inventoryQt[i], i);
                else if (data.inventoryType[i] == 1) Inventory.ForceIntoSlot(DataBase.inst.Gears[data.inventoryIds[i]], data.inventoryQt[i], i);
            }
        }

        if (data.HatSlot != -1) HatSlot.AttemptToPlaceItem(DataBase.inst.Gears[data.HatSlot]);
        if (data.BodySlot != -1) BodySlot.AttemptToPlaceItem(DataBase.inst.Gears[data.BodySlot]);
        if (data.BeltSlot != -1) BeltSlot.AttemptToPlaceItem(DataBase.inst.Gears[data.BeltSlot]);
        if (data.RingSlot1 != -1) RingSlot1.AttemptToPlaceItem(DataBase.inst.Gears[data.RingSlot1]);
        if (data.RingSlot2 != -1) RingSlot2.AttemptToPlaceItem(DataBase.inst.Gears[data.RingSlot2]);
        if (data.WeaponSlot != -1) WeaponSlot.AttemptToPlaceItem(DataBase.inst.Gears[data.WeaponSlot]);

    }

    public float gainExp(Enemy enemy)
    {
        float levelUp = 0;
        int baseExp = enemy.BaseExpReward;
        int Elevel = enemy.Level;
        int multiplier = 3;

        int ammount = (int)(baseExp + (baseExp * multiplier) * Mathf.Log(Elevel, 2));
        Debug.Log($"Gained {ammount} exp!");
        battleInterface.AddMessage($"I gained {ammount} experience Points!", TextMessage.MessageSpeed.NORMAL);

        currentExperience += ammount;
        totalExperience += ammount;            

        levelUp += LevelUp();        

        try
        {
            onExperience();
        } catch (System.NullReferenceException)
        {

        }

        return levelUp;
    }

    public float LevelUp()
    {
        float time = 0;
        if (currentExperience >= experienceToNextLevel)
        {
            time += 3.5f;

            Level++;

            if (battleInterface != null)
            {
                battleInterface.AddMessage($"I'm feeling really confident after that battle, maybe I've gotten stronger...", TextMessage.MessageSpeed.VERYSLOW);
            }
            else if (navigationInterFace != null)
            {
                navigationInterFace.AddMessage($"I've gained a level! I'm now level {Level}", TextMessage.MessageSpeed.VERYSLOW);
            }

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

          if ( CheckForNewSpells()) time += 2f ;

        try
        {
            onLevelUp(Level);
            onStatsChange();
        }
        catch { }

        RecoverAll();
        }

        return time;
    }

    public float gainMoney(Enemy enemy)
    {
        float time = 2;

        int quantity = enemy.BaseMoneyReward * enemy.Level;

        currentMoney += quantity;
        updateGold();

        battleInterface.AddMessage($"Gained {quantity} Gold!", TextMessage.MessageSpeed.NORMAL);

        return time;
    }

    private bool CheckForNewSpells()
    {
        foreach (var s in learnableSpells)
        {
            if (s.Key.LevelLearn <= Level)
            {
                if (!s.Value)
                {
                    knownSpells.Add(s.Key);
                    if (battleInterface != null) battleInterface.AddMessage($"I learned {s.Key.Name}!", TextMessage.MessageSpeed.NORMAL);
                    else navigationInterFace.AddMessage($"I learned {s.Key.Name}!", TextMessage.MessageSpeed.NORMAL);
                    learnableSpells[s.Key] = true;
                    return true;
                }                
            }
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

    public bool UseFountain()
    {
        if (currentHealth < maxHealth || currentMana < maxMana)
        {
            RecoverAll();
            AddInterfaceMessage($"You Drink From the fountain.\n You Feel refreshed.", TextMessage.MessageSpeed.VERYSLOW);
            return true;
        }
        return false;
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

                animationTime += 1f;
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
                    int attackDamage = (((int)(a.user.Power) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animationTime += 1.3f;
                }
                else if (ToHit > Random.Range(0, 100))
                {
                    int attackDamage = (((int)(a.user.Power) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animationTime += 1.3f;
                }
                else
                {
                    battleInterface.AddMessage($"The {a.user}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                    animationTime += 1.8f;
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
                    animationTime += 2f;
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

    protected override void castSpell(CastSpellAction b)
    {
        currentMana -= b.spell.Cost;
        if (b.Target == this)
        {
            if (b.spell.type == Global.Type.LIGHT)
            {
                this.currentHealth += b.spell.Power;
                if (currentHealth >= maxHealth) currentHealth = maxHealth;
                Debug.Log($"{b.user}  healed himself for {b.spell.Power} hp points!");
            }
        }

        playSpellVoiceClip(b.spell.Id);
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

    public void OpenPauseMenu()
    {
        navigationInterFace.OpenPauseMenu();
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
        ExtraMaxHealth += i.HealthBonus;
        maxMana += i.ManaBonus;
        ExtraMaxMana += i.ManaBonus;

        int hDif = maxHealth - h;
        currentHealth += hDif;
        onHealthUpdate(currentHealth, maxHealth);

        int mDif = maxMana - m;
        currentMana += mDif;
        onManaUpdate(currentMana, maxMana);

        
        Power += i.PowerBonus;
        ExtraPower += i.PowerBonus;
        Defence += i.DefenceBonus;
        ExtraDefence += i.DefenceBonus;
        Accuracy += i.AccuracyBonus;
        ExtraAccuracy += i.AccuracyBonus;
        Dodge += i.DodgeBonus;
        ExtraDodge += i.DodgeBonus;
        Speed += i.SpeedBonus;
        ExtraSpeed += i.SpeedBonus;

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
        ExtraMaxHealth -= i.HealthBonus;
        maxMana -= i.ManaBonus;
        ExtraMaxMana -= i.ManaBonus;

        int hDif = h - maxHealth;
        currentHealth -= hDif;
        onHealthUpdate(currentHealth, maxHealth);

        int mDif = m - maxMana;
        currentMana -= mDif;
        onManaUpdate(currentMana, maxMana);

        Power -= i.PowerBonus;
        ExtraPower -= i.PowerBonus;
        Defence -= i.DefenceBonus;
        ExtraDefence -= i.DefenceBonus;
        Accuracy -= i.AccuracyBonus;
        ExtraAccuracy -= i.AccuracyBonus;
        Dodge -= i.DodgeBonus;
        ExtraDodge -= i.DodgeBonus;
        Speed -= i.SpeedBonus;
        ExtraSpeed -= i.SpeedBonus;

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
        AddInterfaceMessage($"Received {ammount} gold pieces", TextMessage.MessageSpeed.VERYSLOW);
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

    public void AddInterfaceMessage(string message, TextMessage.MessageSpeed speed) => navigationInterFace.AddMessage(message, speed);



    public void PickKey(int id)
    {
        keys.Add(id);

        AddInterfaceMessage($"Picked up a {getKeyColorFromId(id)} Key", TextMessage.MessageSpeed.NORMAL);
        navigationInterFace.AddKey(id);
    }

    public bool SearchKey(int id)
    {
        return keys.Contains(id);
    }


    string getKeyColorFromId(int id)
    {
        switch (id)
        {
            case 1:
                return "Green";
            case 2:
                return "Blue";
            case 3:
                return "Yellow";
        }


        return "Red";
    }

    public void EndGame()
    {
        gameManager.EndGame();
    }

    public int KnownTypeSpellsCount(Global.Type type)
    {
        int spellCount = 0;
        int knownSpellsCount = knownSpells.Count;
        if (knownSpellsCount == 0) return 0;
        else
        {
            for (int i = 0; i < knownSpellsCount; i++)
            {
                if (knownSpells[i].type == type) spellCount++;
            }

            return spellCount;
        }  
    }

    public void LevelUpManually()
    {
        Level++;

        if (battleInterface != null)
        {
            battleInterface.AddMessage($"I'm feeling really confident after that battle, maybe I've gotten stronger...", TextMessage.MessageSpeed.VERYSLOW);
        }
        else if (navigationInterFace != null)
        {
            navigationInterFace.AddMessage($"I've gained a level! I'm now level {Level}", TextMessage.MessageSpeed.VERYSLOW);
        }

        totalExperience += experienceToNextLevel;
        currentExperience = 0;

        experienceToNextLevel = 100 * (int)Mathf.Pow((Level + 1), 2) - (100 * (Level + 1));

        //Stats up
        maxHealth += Random.Range(5, 21);
        maxMana += Random.Range(5, 11);

        Power += Random.Range(1, 6);
        Defence += Random.Range(2, 6);
        Accuracy += Random.Range(2, 6);
        Dodge += Random.Range(1, 6);
        Speed += Random.Range(1, 6);

        CheckForNewSpells();

        try
        {
            onLevelUp(Level);
            onStatsChange();
        }
        catch { }

        RecoverAll();
    }


    public void playSpellVoiceClip(int id)
    {
        //AudioClip original = SpellVoicePlayer.clip;
        //SpellVoicePlayer.clip = AudioDataBase.inst.spellVoice[id];
        //AudioClip clip;
        //try
        //{
        //    clip = AudioDataBase.inst.spellVoice[id];

        //    SpellVoicePlayer.PlayOneShot(clip);
        //}
        //catch (KeyNotFoundException)
        //{
        //    SpellVoicePlayer.PlayOneShot(SpellVoicePlayer.clip);
        //}



    }

}
