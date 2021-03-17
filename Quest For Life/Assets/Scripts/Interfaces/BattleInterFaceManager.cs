using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInterFaceManager : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public BattleManager battleManager;

    [HideInInspector]
    public Enemy enemy;

    public bool canAct;

    [SerializeField]
    GameObject SpellSelector;


    public void getInformation(Entity player, Entity enemy,  BattleManager battleManager)
    {
        this.player = (Player)player;
        this.enemy = (Enemy)enemy;
        this.battleManager = battleManager;


        canAct = false;

       
    }

    // Start is called before the first frame update
    void Start()
    {
        SpellSelector.SetActive(false);

        player.gameManager.MonsterCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ChooseAction();
            
        }
        else if (canAct && Input.GetKeyDown(KeyCode.S))
        {
            SpellMenu();
        }


    }

    public void StartChoice()
    {
        canAct = true;
    }

    public void EndChoice()
    {
        canAct = false;
    }

    public void PlayerDeath()
    {

    }

    public void MonsterDeath()
    {

    }

    void ChooseAction()
    {
        EndChoice();
        AttackAction a = new AttackAction(player, enemy, player.BaseAttackPower, Global.Type.LIGHT);
        battleManager.ReceiveActions(a, player);

        Debug.Log("the Player Chose to do a normal Attack");

       
    }

    public void EndBattle()
    {
        battleManager.CleanUp();

        player.gameManager.MonsterCamera.SetActive(false);


        player.gainExp(enemy);


        player.EndBattle();

        Destroy(this.gameObject);
    }

    void SpellMenu()
    {
        if (SpellSelector.activeInHierarchy)
        {
            SpellSelector.SetActive(false);
        }
        else SpellSelector.SetActive(true);
    }

    public void CastSpell(Global.Spell spell)
    {
        if (canAct && player.currentHealth >= spell.Cost)
        {
            EndChoice();
            SpellMenu();

            CastSpellAction action = new CastSpellAction();

            action.speed = player.Speed;
            action.spell = spell;
            action.Target = enemy;
            action.user = player;

            Debug.Log($"Choosing to do {action.spell.Name}");
            


            battleManager.ReceiveActions(action, player);
        }
    }

}
