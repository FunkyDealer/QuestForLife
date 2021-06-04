using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    enum Type
    {
        Slime,
        GiantSlime,
        Ghost
    }
    [SerializeField]
    Type type;

    EnemyIA iA;

    [HideInInspector]
    public Entity entityEnemy;

    int HealthGainPerLevel;
    int ManaGainPerLevel;
    int PowerGainPerLevel;
    int DefenceGainPerLevel;
    int AccuracyGainPerLevel;
    int DodgeGainPerLevel;
    int SpeedGainPerLevel;

    [HideInInspector]
    public int BaseMoneyReward;
    [HideInInspector]
    public int BaseExpReward;

    [HideInInspector]
    public int MonsterID;

    Global.Spell[] KnownSpells;

    Animator animator;
    Animator MonsterAnimator;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    protected Material deathmat;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log($"{EntityName}: Level:{Level}; Health: {currentHealth}; - {currentMana};{Power};{Defence};{Accuracy};{Dodge};{Speed}{BaseAttackPower};{BaseMoneyReward}");
        //AQUI
        AtakEnemy atk = new AtakEnemy(this);
        Global.Spell i = new Global.Spell();
        i.Accuracy = 50;
        i.Name = "I";
        Global.Spell a = new Global.Spell();
        a.Accuracy = 100;
        a.Name = "A";
        KnownSpells = new Global.Spell[2] { i, a };

        iA = new EnemyIA(CreateNodeList(), atk);

        PlayIntroductionSound();
    }

    // Update is called once per frame
    void Update()
    {
        //if (animator.GetBool("Dead") && animator.


    }

    public void SetMonsterAnimator(Animator m)
    {
        this.MonsterAnimator = m;
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

    public List<SpellEnemy> CreateNodeList()
    {
        List<SpellEnemy> lista = new List<SpellEnemy>();

        for (int i = 0; i < KnownSpells.Length; i++)
        {
            int p = KnownSpells[i].Power;
            float prob = (float)(((p - 100000) * -1) * 0.000001);

            SpellEnemy s = new SpellEnemy(this, KnownSpells[i], (float)(KnownSpells[i].Accuracy * 0.01));
            lista.Add(s);
        }

        return lista;
    }

    public void BattleActionChangetoSpell(CastSpellAction spell) 
    {
        _currentBattleAction = spell;
    }

    public void BattleActionChangetoAtk(AttackAction attack)
    {
        _currentBattleAction = attack;
    }
    public override BattleAction ChooseAction(Entity enemy)
    {
        //AQUI
        entityEnemy = enemy;

        //iA.DoSeqN();

        AttackAction AttackAction = new AttackAction(this, enemy, this.BaseAttackPower, 100, Global.Type.NONE);

        //Debug.Log("the Monster Chose to do a normal Attack");

        this._currentBattleAction = AttackAction;

        return _currentBattleAction;
    }


    public override void HealthMessage()
    {
        base.HealthMessage();

        float healthPercentage = currentHealth / maxHealth;
        healthPercentage *= 100;

        switch (healthPercentage)
        {
            case var expression when healthPercentage < 80 && healthPercentage > 50:
                battleInterface.AddMessage($"The {EntityName} looks slightly hurt.", TextMessage.MessageSpeed.NORMAL);
                break;
            case var expression when healthPercentage < 50 && healthPercentage > 30:
                battleInterface.AddMessage($"The {EntityName} looks hurt.", TextMessage.MessageSpeed.NORMAL);
                break;
            case var expression when healthPercentage < 30 && healthPercentage > 10:
                battleInterface.AddMessage($"The {EntityName} looks really really hurt.", TextMessage.MessageSpeed.FAST);
                break;
            case var expression when healthPercentage < 10:
                battleInterface.AddMessage($"The {EntityName} looks like he's on the brink of death!", TextMessage.MessageSpeed.FAST);
                break;
            default:
                battleInterface.AddMessage($"The {EntityName} doesn't look hurt at all...", TextMessage.MessageSpeed.SLOW);
                break;
        }

    }

    protected override void ReceiveDamage(int attackPower)
    {
        base.ReceiveDamage(attackPower);

        PlayHurtSound();

    }

    protected override void castSpell(CastSpellAction b)
    {
        base.castSpell(b);


    }

    protected override void Death()
    {
        base.Death();

        battleManager.MonsterDeath();
        animator.SetBool("Dead", true);

        battleInterface.MonsterDeath();
        //Debug.Log($"Monster Died!");
        battleInterface.AddMessage($"The {this.EntityName} Died!", TextMessage.MessageSpeed.VERYFAST);
        dead = true;
    }

    public override float PerformAction(BattleAction action, Entity Enemy)
    {

        float animationTime = 0;
        switch (action)
        {
            case AttackAction a:
                animator.SetTrigger("Attack");
                animationTime += Enemy.ReceiveAction(action);
                animationTime += 1.4f;
                break;
            case CastSpellAction b:
                castSpell(b);
                animator.SetTrigger("Cast");
                animationTime += Enemy.ReceiveAction(action);
                animationTime += 2;
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
        float animationTime = 0;
        switch (action)
        {
            case AttackAction a:
                float tohit = (a.user.Accuracy * a.AttackAccuracy / a.Target.Dodge);
                int ToHit = (int)tohit;

                if (ToHit > 100)
                {
                    Vector2 weakness = checkForWeakness(a.type);

                    int attackDamage = (((int)(a.user.Power * weakness.x) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animator.SetTrigger("TakeDmg");
                    MonsterAnimator.SetTrigger("Damaged");
                    animationTime += 1.3f + weakness.y;
                }
                else if (ToHit > Random.Range(0, 100))
                {
                    Vector2 weakness = checkForWeakness(a.type);

                    int attackDamage = (((int)(a.user.Power * weakness.x) * a.attackBasePower) / (Defence + 1));
                    ReceiveDamage(attackDamage);
                    animator.SetTrigger("TakeDmg");
                    MonsterAnimator.SetTrigger("Damaged");
                    animationTime += 1.3f + weakness.y;
                }
                else
                {
                    animator.SetTrigger("Dodge");
                    animationTime += 2.1f;
                    battleInterface.AddMessage($"The {a.user.EntityName}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                }
                break;
            case CastSpellAction b:
                if (b.Target == this)
                {
                    if (b.spell.type != Global.Type.LIGHT)
                    {
                        float tohit2 = (b.user.Accuracy * b.spell.Accuracy / b.Target.Dodge);
                        int ToHit2 = (int)tohit2;

                        if (tohit2 > 100)
                        {
                            ReceiveSpellAttack(b);
                            animator.SetTrigger("TakeDmg");
                            animationTime += 1.3f;
                        }
                        else if (ToHit2 > Random.Range(0, 100))
                        {
                            ReceiveSpellAttack(b);
                            animator.SetTrigger("TakeDmg");
                            animationTime += 1.3f;
                        }
                        else
                        {
                            animator.SetTrigger("Dodge");
                            animationTime += 2.1f;
                            battleInterface.AddMessage($"The {b.user.EntityName}'s Attack Missed!", TextMessage.MessageSpeed.FAST);
                        }
                    }
                    else
                    {
                        receiveHolySpell(b);
                        animationTime += 1.3f;
                    }
                }
                break;
            case ItemUseAction c:
                animationTime += 1;
                break;
            case InvestigationAction d:
                animationTime += 1;
                break;
            case RunAction e:
                float chanceToEscape = ((e.speed * 40) / this.Speed) + 30;
                //Debug.Log($"Chance to escape: {chanceToEscape}");
                if (chanceToEscape > 100)
                {
                    battleManager.RunAway();
                    animationTime += 1;
                }
                else if (chanceToEscape > Random.Range(0, 100))
                {
                    battleManager.RunAway();
                    animationTime += 1;
                }
                else
                {
                    battleInterface.AddMessage($"You Failed at running away!", TextMessage.MessageSpeed.NORMAL);
                    animationTime += 1;
                }

                break;
        }

        return animationTime;
    }

    void receiveHolySpell(CastSpellAction b)
    {
        if (this.Weakness == Global.Type.LIGHT)
        {
            int spellDamage = (((int)(b.user.Power * 2) * b.spell.Power) / (Defence + 1));
            battleInterface.AddMessage($"That attack did massive ammount of damage!", TextMessage.MessageSpeed.FAST);
            animator.SetTrigger("TakeDmg");

            ReceiveDamage(spellDamage);
        } else
        {
            this.currentHealth += b.spell.Power;
            if (currentHealth >= maxHealth) currentHealth = maxHealth;
            battleInterface.AddMessage($"You Healed the enemy!", TextMessage.MessageSpeed.FAST);
        }
    }

    void PlayDodgeSound()
    {
        audioSource.PlayOneShot(AudioDataBase.inst.dodgeSound, AppManager.inst.appdata.EffectsVolume);
    }

    void PlayCastSpell()
    {
        switch (type)
        {
            case Type.Slime:

                break;
            case Type.GiantSlime:
                break;
            case Type.Ghost:
                break;
            default:
                break;
        }
    }

    void PlayHurtSound()
    {
        switch (type)
        {
            case Type.Slime:
                audioSource.PlayOneShot(AudioDataBase.inst.getSlimeHurt(), AppManager.inst.appdata.EffectsVolume);
                break;
            case Type.GiantSlime:
                audioSource.PlayOneShot(AudioDataBase.inst.getSlimeHurt(), AppManager.inst.appdata.EffectsVolume);
                break;
            case Type.Ghost:
                audioSource.PlayOneShot(AudioDataBase.inst.getGhostIntroduction(), AppManager.inst.appdata.EffectsVolume);
                break;
            default:
                break;
        }
    }

    void PlayIntroductionSound()
    {
        switch (type)
        {
            case Type.Slime:
                audioSource.PlayOneShot(AudioDataBase.inst.getSlimeIntroduction(), AppManager.inst.appdata.EffectsVolume);
                break;
            case Type.GiantSlime:
                audioSource.PlayOneShot(AudioDataBase.inst.getSlimeIntroduction(), AppManager.inst.appdata.EffectsVolume);
                break;
            case Type.Ghost:
                audioSource.PlayOneShot(AudioDataBase.inst.getGhostIntroduction(), AppManager.inst.appdata.EffectsVolume);
                break;
            default:
                break;
        }
    }

    public void ChangeToDeathmat()
    {
        Renderer rend = gameObject.GetComponentInChildren<Renderer>();

        rend.material = deathmat;
        rend.material.SetFloat("startTime", Time.time);

    }


}
