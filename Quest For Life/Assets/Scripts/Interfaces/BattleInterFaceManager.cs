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
        player.gameManager.MonsterCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ChooseAction();
            
        }


    }

    public void PlayerDeath()
    {

    }

    public void MonsterDeath()
    {

    }

    void ChooseAction()
    {
        canAct = false;
        AttackAction a = new AttackAction(player, enemy, player.BaseAttackPower, Global.Type.LIGHT);
        battleManager.ReceiveActions(a, player);

        Debug.Log("the Player Chose to do a normal Attack");

       
    }

    public void EndBattle()
    {
        battleManager.CleanUp();

        player.gameManager.MonsterCamera.SetActive(false);

        player.EndBattle();

        Destroy(this.gameObject);
    }

}
