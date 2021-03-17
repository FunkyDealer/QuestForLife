using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<int, Global.DungeonMonsterInfo> Monsters;
    [HideInInspector]
    public Dictionary<int, Global.Spell> Spells;

    private static DataBase _instance;
    public static DataBase inst { get { return _instance; } }

    [HideInInspector]
    public Dictionary<int, GameObject> MonsterPrefabs;
    [SerializeField]
    List<GameObject> MPrefabs;

    [SerializeField]
    TextAsset MonsterInfoFile;
    [SerializeField]
    TextAsset SpellInfoFile;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        MonsterPrefabs = new Dictionary<int, GameObject>();

        int i = 1;
        foreach (var o in MPrefabs)
        {
            MonsterPrefabs.Add(i, o);
            i++;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        readSpells(SpellInfoFile);
        ReadMonsters(MonsterInfoFile);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readSpells(TextAsset file)
    {
        Spells = new Dictionary<int, Global.Spell>();

        string[] lines = file.text.Split(new string[] { "\n" }, StringSplitOptions.None);
        int lineCount = lines.Length;
        Debug.Log($"{lineCount - 2} spells found");

        for (int i = 1; i < lineCount - 1; i++)
        {
            string[] data = (lines[i].Trim()).Split(","[0]);

            Global.Spell spell = new Global.Spell();

            spell.Name = data[0];
            spell.Id = int.Parse(data[1]);
            spell.type = ConvertStringToType(data[2]);
            spell.Power = int.Parse(data[3]);
            spell.Cost = int.Parse(data[4]);
            spell.Accuracy = int.Parse(data[5]);
            spell.effect = ConvertToAttackEffect(int.Parse(data[6]));

            Spells.Add(spell.Id, spell);
           // Debug.Log($"Adding new Spell: {spell.Name}, {spell.Id}, {spell.type}, {spell.Power},  {spell.Cost}, {spell.Accuracy},  {spell.effect}");
        }

        Debug.Log("Spells Database Finished");
    }

    Global.Type ConvertStringToType(string t)
    {
        switch (t.ToUpper())
        {
            case "FIRE":
                return Global.Type.FIRE;
            case "WATER":
                return Global.Type.WATER;
            case "THUNDER":
                return Global.Type.THUNDER;
            case "LIGHT":
                return Global.Type.LIGHT;
            case "DARKNESS":
                return Global.Type.DARKNESS;
            default:
                return Global.Type.NONE;
        }
    }

    Global.AttackEffect ConvertToAttackEffect(int i)
    {
        switch (i)
        {                
            case 1:
                return Global.AttackEffect.HEAL;               
            case 2:
                return Global.AttackEffect.DEATH;                
            default:
                return Global.AttackEffect.NONE;
        }
    }

    void ReadMonsters(TextAsset file)
    {
        Monsters = new Dictionary<int, Global.DungeonMonsterInfo>();

         string[] lines = file.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            int lineCount = lines.Length;
        Debug.Log($"{lineCount - 2} Monsters found");

        for (int i = 1; i < lineCount - 1; i++)
            {
            string[] data = (lines[i].Trim()).Split(","[0]);
            Global.DungeonMonsterInfo monster = new Global.DungeonMonsterInfo();

                monster.Name = data[0];
                monster.minFloor = int.Parse(data[1]);
                monster.maxFloor = int.Parse(data[2]);
                monster.id = int.Parse(data[3]);
                monster.BaseHealth = int.Parse(data[4]);
                monster.HealthGainPerLevel = int.Parse(data[5]);
                monster.BaseMana = int.Parse(data[6]);
                monster.ManaGainPerLevel = int.Parse(data[7]);
                monster.BasePower = int.Parse(data[8]);
                monster.PowerGainPerLevel = int.Parse(data[9]);
                monster.BaseDefence = int.Parse(data[10]);
                monster.DefenceGainPerLevel = int.Parse(data[11]);
                monster.BaseAccuracy = int.Parse(data[12]);
                monster.AccuracyGainPerLevel = int.Parse(data[13]);
                monster.BaseDodge = int.Parse(data[14]);
                monster.DodgeGainPerLevel = int.Parse(data[15]);
                monster.BaseSpeed = int.Parse(data[16]);
                monster.SpeedGainPerLevel = int.Parse(data[17]);
                monster.BaseAttackPower = int.Parse(data[18]);
                monster.BaseReward = int.Parse(data[19]);
                monster.BaseExpReward = int.Parse(data[20]);

                monster.Resistence = ConvertStringToType(data[21]);
                monster.Weakness = ConvertStringToType(data[22]);

                int[] spellIds = new int[3];
                int spellCounter = 0;
                for (int s = 0; s < 3; s++)
                 {
                    spellIds[s] = int.Parse(data[23 + s]);
                if (spellIds[s] != 0) spellCounter++;
                 }

                monster.spells = new Global.Spell[spellCounter];

                spellCounter = 0;
                for (int S = 0; S < 3; S++)
                {
                     if (spellIds[S] != 0) { monster.spells[spellCounter] = convertToSpell(spellIds[S]); spellCounter++; }           
                }

            Monsters.Add(monster.id, monster);
           // if (monster.spells.Length > 0) Debug.Log($"Adding new monster: {monster.Name}, {monster.id}, {monster.Weakness}, {monster.spells[monster.spells.Length-1].Name}");
           // else Debug.Log($"Adding new monster: {monster.Name}, {monster.id}, {monster.Weakness}, no spells");
        }

        Debug.Log("Monsters Database Finished");
    }

    Global.Spell convertToSpell(int i)
    {
        try
        {
            Global.Spell spell = Spells[i];
            return spell;
        }
        catch (Exception)
        {
            Debug.Log($"Trying to return an empty Spell - Spell ID: {i}");
            return new Global.Spell();
        }        
    }

    
}
