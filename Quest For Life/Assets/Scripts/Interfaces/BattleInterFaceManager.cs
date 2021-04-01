using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInterFaceManager : HudManager
{
    [HideInInspector]
    public BattleManager battleManager;

    [HideInInspector]
    public Enemy enemy;

    public bool canAct;

    [SerializeField]
    GameObject SpellSelector;

    [SerializeField]
    GameObject PlayerSelectButton;
    [SerializeField]
    GameObject EnemySelectButton;

    public bool selectingTarget;

    [SerializeField]
    MessageDisplayer messageDisplayer;

    [SerializeField]
    GameObject blockers;

    public void getInformation(Entity player, Entity enemy, BattleManager battleManager)
    {
        this.player = (Player)player;
        this.enemy = (Enemy)enemy;
        this.battleManager = battleManager;


        canAct = false;
        selectingTarget = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        //SpellSelector.SetActive(false);

        player.gameManager.MonsterCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            AttackAction();
        }


    }

    public void StartChoice()
    {
        canAct = true;
        blockers.SetActive(!canAct);
        selectingTarget = false;

    }

    public void EndChoice()
    {
        canAct = false;
        blockers.SetActive(!canAct);
    }

    public void PlayerDeath()
    {

    }

    public void MonsterDeath()
    {

    }

    public void EndBattle()
    {
        battleManager.CleanUp();

        player.gameManager.MonsterCamera.SetActive(false);


        player.gainExp(enemy);


        player.EndBattle();

        Destroy(this.gameObject);
    }


    public void AttackAction()
    {
        if (canAct && !selectingTarget)
        {
            EndChoice();
            AttackAction a = new AttackAction(player, enemy, player.BaseAttackPower, 100, Global.Type.LIGHT);
            battleManager.ReceiveActions(a, player);

            Debug.Log("the Player Chose to do a normal Attack");
        }
    }

    public void SpellMenu()
    {
        if (canAct)
        {
            if (SpellSelector.activeInHierarchy)
            {
                SpellSelector.SetActive(false);
            }
            else SpellSelector.SetActive(true);
        }
    }

    public bool CastSpell(Global.Spell spell, Entity target)
    {
        if (canAct && player.currentMana >= spell.Cost)
        {
            EndChoice();
            SpellMenu();

            CastSpellAction action = new CastSpellAction();

            action.speed = player.Speed;
            action.spell = spell;
            action.Target = target;
            action.user = player;

            Debug.Log($"Choosing to do {action.spell.Name}");



            battleManager.ReceiveActions(action, player);
            selectingTarget = false;

            PlayerSelectButton.SetActive(false);
            EnemySelectButton.SetActive(false);
            return true;
        }
        return false;
    }

    public void ItemMenu()
    {
        if (canAct && !selectingTarget)
        {

        }
    }

    public void StatsMenu()
    {
        if (canAct && !selectingTarget)
        {

        }
    }

    public void InvestigateAction()
    {
        if (canAct && !selectingTarget)
        {

        }
    }

    public void RunAction()
    {
        if (canAct && !selectingTarget)
        {
            RunAction run = new RunAction();
            Debug.Log(run.canRun);

            if (run.canRun > 5)
            {
                battleManager.ReceiveActions(run,player);

                battleManager.CleanUp();

                player.gameManager.MonsterCamera.SetActive(false);

                player.EndBattle();

                Destroy(this.gameObject);
            }
            else
            {
                battleManager.ReceiveActions(run, player);
            }
            
        }
    }


    public bool ChooseTarget(Global.Spell spell)
    {
        if (canAct && player.currentMana >= spell.Cost)
        {
            SpellMenuManager s = SpellSelector.GetComponent<SpellMenuManager>();
            s.disableAllContentMenus();

            PlayerSelectButton.SetActive(true);
            SpellTargetButton p = PlayerSelectButton.GetComponent<SpellTargetButton>();
            p.assignedSpell = spell;

            EnemySelectButton.SetActive(true);
            SpellTargetButton e = EnemySelectButton.GetComponent<SpellTargetButton>();
            e.assignedSpell = spell;
            selectingTarget = true;
            return true;
        }
        return false;
    }

    public void AddMessage(string message) => messageDisplayer.AddMessage(message);

}
