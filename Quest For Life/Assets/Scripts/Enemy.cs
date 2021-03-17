using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{      

    int HealthGainPerLevel;
    int ManaGainPerLevel;
    int PowerGainPerLevel;
    int DefenceGainPerLevel;
    int AccuracyGainPerLevel;
    int DodgeGainPerLevel;
    int SpeedGainPerLevel;

    public int BaseMoneyReward;
    public int BaseExpReward;

    public int MonsterID;

    Global.Spell[] KnownSpells;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{EntityName}: Level:{Level}; Health: {currentHealth}; - {currentMana};{Power};{Defence};{Accuracy};{Dodge};{Speed}{BaseAttackPower};{BaseMoneyReward}");
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
        this.BaseExpReward = info.BaseExpReward;
    }



    public override BattleAction ChooseAction(Entity enemy)
    {
        AttackAction AttackAction = new AttackAction(this, enemy, this.BaseAttackPower, Global.Type.NONE);

        Debug.Log("the Monster Chose to do a normal Attack");

        return AttackAction;
    }

}
