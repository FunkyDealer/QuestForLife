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

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        AttackAction AttackAction = new AttackAction(this, enemy, this.BaseAttackPower, 100, Global.Type.NONE);

        Debug.Log("the Monster Chose to do a normal Attack");

        return AttackAction;
    }


    public override void HealthMessage()
    {
        base.HealthMessage();

        float healthPercentage = currentHealth / maxHealth;
        healthPercentage *= 100;

        switch (healthPercentage)
        {
            case var expression when healthPercentage < 80 && healthPercentage > 50:
                battleInterface.AddMessage($"The {EntityName} looks slightly hurt.");
                break;
            case var expression when healthPercentage < 50 && healthPercentage > 30:
                battleInterface.AddMessage($"The {EntityName} looks hurt.");
                break;
            case var expression when healthPercentage < 30 && healthPercentage > 10:
                battleInterface.AddMessage($"The {EntityName} looks really really hurt.");
                break;
            case var expression when healthPercentage < 10:
                battleInterface.AddMessage($"The {EntityName} looks like he's on the brink of death!");
                break;
            default:
                battleInterface.AddMessage($"The {EntityName} doesn't look hurt at all...");
                break;
        }

    }

    protected override void ReceiveDamage(int attackPower)
    {
        base.ReceiveDamage(attackPower);

        

    }

    protected override void castSpell(CastSpellAction b)
    {
        base.castSpell(b);

        
    }

    public override float PerformAction(BattleAction action)
    {
        float animationTime = 1;
        switch (action)
        {
            case AttackAction a:
                animator.SetTrigger("Attack");
                animationTime = 1.4f;
                break;
            case CastSpellAction b:
                castSpell(b);
                animator.SetTrigger("Cast");
                animationTime = 2;
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

    public override float ReceiveAction(BattleAction action)
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
                    animator.SetTrigger("TakeDmg");
                    animationTime = 1.3f;
                }
                else if (ToHit > Random.Range(0, 100))
                {
                    int attackDamage = (((int)(a.user.Power * checkForWeakness(a.type)) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animator.SetTrigger("TakeDmg");
                    animationTime = 1.3f;
                }
                else
                {
                    animator.SetTrigger("Dodge");
                    animationTime = 2.1f;
                    battleInterface.AddMessage($"The {a.user}'s Attack Missed!");
                }
                break;
            case CastSpellAction b:
                float tohit2 = (b.user.Accuracy * b.spell.Accuracy / b.Target.Dodge);
                int ToHit2 = (int)tohit2;

                if (tohit2 > 100) ReceiveSpellAttack(b);
                else if (ToHit2 > Random.Range(0, 100)) ReceiveSpellAttack(b);
                else { animator.SetTrigger("Dodge"); animationTime = 2.1f; battleInterface.AddMessage($"The {b.user}'s Attack Missed!"); }
                break;
            case ItemUseAction c:

                break;
            case InvestigationAction d:

                break;
            case RunAction e:
                float chanceToEscape = ((e.speed * 40) / this.Speed) + 30;
                Debug.Log($"Chance to escape: {chanceToEscape}");
                if (chanceToEscape > 100) { battleManager.RunAway(); }
                else if (chanceToEscape > Random.Range(0, 100))
                {
                    battleManager.RunAway();
                }
                else
                {
                    battleInterface.AddMessage($"You Failed at running away!");
                }

                break;
        }

        return animationTime;
    }

}
