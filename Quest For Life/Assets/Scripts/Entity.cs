﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [HideInInspector]
    public string EntityName;
    [HideInInspector]
    public int Level;

    //Health and Mana
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int maxMana;
    [HideInInspector]
    public int currentMana;

    //Other Stats
    [HideInInspector]
    public int Power;
    [HideInInspector]
    public int Defence;
    [HideInInspector]
    public int Accuracy;
    [HideInInspector]
    public int Dodge;
    [HideInInspector]
    public int Speed;

    [HideInInspector]
    public int BaseAttackPower;

    [HideInInspector]
    public BattleManager battleManager;

    public Global.Type Resistence;
    public Global.Type Weakness;

    [HideInInspector]
    public BattleInterFaceManager battleInterface;

    public bool dead;

    void Awake()
    {
        battleManager = null;
        dead = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual BattleAction ChooseAction(Entity enemy)
    {
        return null;
    }


    public virtual void PerformAction(BattleAction action)
    {
        switch (action)
        {
            case AttackAction a:
                break;
            case CastSpellAction b:

                currentMana -= b.spell.Cost;

                break;
            case ItemUseAction c:

                HealItem item = (HealItem)c.itemToUse;

                switch (item.HealType)
                {
                    case Global.HealType.HEALTH:
                        currentHealth += item.HealAmmount;
                        if (currentHealth > maxHealth) currentHealth = maxHealth;
                        break;
                    case Global.HealType.MANA:
                        currentMana += item.HealAmmount;
                        if (currentMana > maxMana) currentMana = maxMana;
                        break;
                    default:
                        break;
                }
                break;
            case InvestigationAction d:
                break;
            case RunAction e:
                break;
        }
    }

    public virtual void ReceiveAction(BattleAction action)
    {
        switch (action)
        {
            case AttackAction a:

                int attackDamage = (( (int) (a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1) );

                ReceiveDamage(attackDamage);


                break;
            case CastSpellAction b:
                int spellDamage = (( (int) (b.user.Power * checkForWeakness(b.spell.type)) * b.spell.Power) / (Defence + 1) );

                ReceiveDamage(spellDamage);
                break;
            case ItemUseAction c:

                break;
            case InvestigationAction d:

                break;
            case RunAction e:

                break;             
        }      
    }

    protected virtual void ReceiveDamage(int attackPower)
    {
        currentHealth -= attackPower;

        Debug.Log($"Monster Received {attackPower} damage!");

        if (currentHealth <= 0)
        {
            battleManager.MonsterDeath();
            battleInterface.MonsterDeath();
            Debug.Log($"Monster Died!");
            dead = true;
        }
    }

    protected float checkForWeakness(Global.Type type)
    {
        if (type == Global.Type.NONE) return 1;

        if (Weakness == type) { Debug.Log("Super Effective Move!"); return 2; }
        else if (Resistence == type) return 0.5f;
        else return 1;
    }

    public virtual void EndBattle()
    {

    }

}
