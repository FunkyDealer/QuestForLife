using System.Collections;
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

                castSpell(b);

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

                float tohit = (a.user.Accuracy * a.AttackAccuracy / a.Target.Dodge);
                int ToHit = (int)tohit;

                if (Random.Range(0, 100) < ToHit)
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));

                    ReceiveDamage(attackDamage);
                } else
                {
                    battleInterface.AddMessage($"The {a.user}'s Attack Missed!");
                }

                break;
            case CastSpellAction b:

                float tohit2 = (b.user.Accuracy * b.spell.Accuracy / b.Target.Dodge);
                int ToHit2 = (int)tohit2;

                if (Random.Range(0, 100) < ToHit2)
                {
                    ReceiveSpellAttack(b);
                }
                else
                 {
                    battleInterface.AddMessage($"The {b.user}'s Attack Missed!");
                 }

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
            //Debug.Log($"Monster Died!");
            battleInterface.AddMessage($"The {this.EntityName} Died!");
            dead = true;
        }
        else
        {
            HealthMessage();
        }
    }

    protected float checkForWeakness(Global.Type type)
    {
        if (type == Global.Type.NONE) return 1;

        if (Weakness == type) { battleInterface.AddMessage("That attack looks like it really hurt!"); Debug.Log("Super Effective Move!"); ; return 2; }
        else if (Resistence == type) { battleInterface.AddMessage("that Attacks seems to have had little effect!"); return 0.5f; }
        else return 1;
    }

    protected void castSpell(CastSpellAction b)
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
    }

    protected void ReceiveSpellAttack(CastSpellAction b)
    {
        if (b.Target == this)
        {
            int spellDamage = (((int)(b.user.Power * checkForWeakness(b.spell.type)) * b.spell.Power) / (Defence + 1));

            ReceiveDamage(spellDamage);
        }
    }

    public virtual void EndBattle()
    {

    }

    public virtual void HealthMessage()
    {

    }

}
