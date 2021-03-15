using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [HideInInspector]
    public Entity player;
    [HideInInspector]
    public Entity monster;

    BattleAction monsterAction;
    BattleAction playerAction;

    [HideInInspector]
    public BattleInterFaceManager interfaceManager;

    int roundNumber;

    bool death;

    void Awake()
    {
        monsterAction = null;
        playerAction = null;
        roundNumber = 0;
        death = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        



    }

    public void StartBattle(Player p, BattleInterFaceManager m)
    {
        this.interfaceManager = m;
        player = p;
        monster = MonsterRoomManager.inst.GetMonster();
        monster.battleInterface = interfaceManager;

        player.battleManager = this;
        monster.battleManager = this;


        interfaceManager.getInformation(player, monster, this);

        
        StartCoroutine(ChooseActions()); //Run after 1 second
    }

    IEnumerator ChooseActions()
    {
        yield return new WaitForSeconds(1); //Wait for 1 second

        roundNumber++;    
        interfaceManager.canAct = true;
        Debug.Log("Choose your Action! (Enter)");


         monsterAction = monster.ChooseAction(player);

    }

    public void ReceiveActions(BattleAction action, Entity actor)
    {
        if (actor is Player) playerAction = action;



        if (playerAction != null && monsterAction != null) StartTurn();
    }


    void StartTurn()
    {
        if (playerAction.speed > monsterAction.speed)
        {
            PerformPlayerAction();
        }
        else
        {
            PerformMonsterAction();
        }
        

    }

    void ContinueTurn()
    {
        if (playerAction != null) PerformPlayerAction();

        else if (monsterAction != null) PerformMonsterAction();

        else EndTurn();

    }

    void PerformPlayerAction()
    {
        Debug.Log("player is attacking");
        player.PerformAction(playerAction);
        monster.ReceiveAction(playerAction);

        playerAction = null;
        ConfirmStatus();
    }

    void PerformMonsterAction()
    {
        Debug.Log("Monster is Attacking");
        monster.PerformAction(monsterAction);
        player.ReceiveAction(monsterAction);

        monsterAction = null;
        ConfirmStatus();
    }

    public void ConfirmStatus()
    {
        if (!death) ContinueTurn();

        else if (death) EndBattle();
    }

    void EndTurn()
    {

        playerAction = null;
        monsterAction = null;
        Debug.Log("Turn Ending");

        StartCoroutine(ChooseActions()); //Run after 1 second
    }

    void EndBattle()
    {
        if (player.dead)
        {
            Debug.Log("Player Died");
        }
        else if (monster.dead)
        {
            MonsterRoomManager.inst.EndBattle();
            interfaceManager.EndBattle();            

            Debug.Log("Player has Won the battle!");
        }
    }

    public void CleanUp()
    {
        Destroy(this.gameObject);
    }


    public void PlayerDeath() //If the Player Dies
    {
        death = true;

        MonsterRoomManager.inst.PlayerDeath();
    }

    public void MonsterDeath() //If the Monster Dies
    {
        death = true;

        MonsterRoomManager.inst.MonsterDeath();
    }

}
