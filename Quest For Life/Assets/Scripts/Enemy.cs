using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{   
    public int BaseAttackPower;
    public Global.Type Resistence;
    public Global.Type Weakness;


    int HealthGainPerLevel;
    int ManaGainPerLevel;
    int PowerGainPerLevel;
    int DefenceGainPerLevel;
    int AccuracyGainPerLevel;
    int DodgeGainPerLevel;
    int SpeedGainPerLevel;

    public int BaseMoneyReward;

    public int MonsterID;

    Global.Spell[] KnownSpells;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{Level};{currentHealth};{currentMana};{Power};{Defence};{Accuracy};{Dodge};{Speed}{BaseAttackPower};{BaseMoneyReward}");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getStats(Global.DungeonMonsterInfo info, int level)
    {
        EntityName = info.Name;
        this.Level = level;
        this.MonsterID = info.id;

        maxHealth = info.BaseHealth + info.HealthGainPerLevel * level;
        currentHealth = maxHealth;
        maxMana = info.BaseHealth + info.HealthGainPerLevel * level; 
        currentMana = maxMana;


        Power = info.BasePower + info.PowerGainPerLevel * level; 
        Defence = info.BaseDefence + info.DefenceGainPerLevel * level; 
        Accuracy = info.BaseAccuracy + info.AccuracyGainPerLevel * level; 
        Dodge = info.BaseDodge + info.DodgeGainPerLevel * level; 
        Speed = info.BaseSpeed + info.SpeedGainPerLevel * level;

        KnownSpells = info.spells;

        this.Resistence = info.Resistence;
        this.Weakness = info.Weakness;

        this.BaseAttackPower = info.BaseAttackPower;
        this.BaseMoneyReward = info.BaseReward;
    }

    void getDamage()
    {

    }

    void CheckForDefeat()
    {

    }

}
