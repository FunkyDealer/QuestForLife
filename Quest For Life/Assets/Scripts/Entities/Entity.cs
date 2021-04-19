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

    protected BattleAction _currentBattleAction;
    public BattleAction currentBattleAction => _currentBattleAction;


    void Awake()
    {
        battleManager = null;
        dead = false;
        _currentBattleAction = null;
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


    public virtual float PerformAction(BattleAction action, Entity enemy)
    {
        float animationTime = 1;
        switch (action)
        {
            case AttackAction a:

                break;
            case CastSpellAction b:

                castSpell(b);

                break;
            case ItemUseAction c:


                break;
            case InvestigationAction d:
                break;
            case RunAction e:
                break;
        }

        return animationTime;
    }

    public virtual float ReceiveAction(BattleAction action)
    {
        float animationTime = 1;
        switch (action)
        {
            case AttackAction a:

                float tohit = (a.user.Accuracy * a.AttackAccuracy / a.Target.Dodge);
                int ToHit = (int)tohit;

                if (ToHit > 100)
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                }
                else if (ToHit > Random.Range(0, 100))
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                } else
                {
                    battleInterface.AddMessage($"The {a.user}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                }

                break;
            case CastSpellAction b:

                float tohit2 = (b.user.Accuracy * b.spell.Accuracy / b.Target.Dodge);
                int ToHit2 = (int)tohit2;

                if (tohit2 > 100) ReceiveSpellAttack(b);
                else if (ToHit2 > Random.Range(0, 100)) ReceiveSpellAttack(b);
                else battleInterface.AddMessage($"The {b.user}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                break;
            case ItemUseAction c:

                break;
            case InvestigationAction d:

                break;
            case RunAction e:

                float chanceToEscape = ((e.speed * 40) / this.Speed) + 30;
                Debug.Log($"Chance to escape: {chanceToEscape}");
                if (chanceToEscape > 100) { battleManager.RunAway(); }
                else if (chanceToEscape > Random.Range(0, 100)) {
                    battleManager.RunAway();
                } else
                {
                    battleInterface.AddMessage($"You Failed at running away!", TextMessage.MessageSpeed.NORMAL);
                }

                break;             
        }

        return animationTime;
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
            battleInterface.AddMessage($"The {this.EntityName} Died!", TextMessage.MessageSpeed.VERYFAST);
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

        if (Weakness == type) { battleInterface.AddMessage("That attack looks like it really hurt!", TextMessage.MessageSpeed.FAST); ; return 2; }
        else if (Resistence == type) { battleInterface.AddMessage("that Attacks seems to have had little effect!", TextMessage.MessageSpeed.NORMAL); return 0.5f; }
        else return 1;
    }

    protected virtual void castSpell(CastSpellAction b)
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


    public virtual void SetBattleAction(BattleAction action)
    {
        this._currentBattleAction = action;
    }

}
