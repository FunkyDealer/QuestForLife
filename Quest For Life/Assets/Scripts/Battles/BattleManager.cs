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

    bool battleOver;

    void Awake()
    {
        monsterAction = null;
        playerAction = null;
        roundNumber = 0;
        death = false;
        battleOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (battleOver == true)
        {

        }
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

       interfaceManager.AddMessage($"A {monster.EntityName} Attacked!");
    }

    //Start Choice Of Action from the Player (Ran by this script)
    IEnumerator ChooseActions()
    {
        yield return new WaitForSeconds(1); //Wait for 1 second

        roundNumber++;
        interfaceManager.StartChoice();

         monsterAction = monster.ChooseAction(player);
    }

    //Receives the Actions From the player and Monster (Ran by the entities) Comes after ChooseAction()
    public void ReceiveActions(BattleAction action, Entity actor)
    {
        if (actor is Player) playerAction = action;



        if (playerAction != null && monsterAction != null) StartTurn();
    }

    //Started the Turn (Run by ReceiveActions() after both actions have been received
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

    //Continue Turn (Called After someone has attacked)
    IEnumerator ContinueTurn(float time)
    {
        yield return new WaitForSeconds(time);

        if (playerAction != null) PerformPlayerAction();

        else if (monsterAction != null) PerformMonsterAction();

        else EndTurn();
    }

    //Perform the Player's Action
    void PerformPlayerAction()
    {
        float totalAnimationTime = 0;
        //Debug.Log("player is attacking");
        totalAnimationTime += player.PerformAction(playerAction);
        totalAnimationTime += monster.ReceiveAction(playerAction);

        playerAction = null;
        StartCoroutine(ConfirmStatus(totalAnimationTime));
    }

    //Perform the Monster's Action
    void PerformMonsterAction()
    {
        float totalAnimationTime = 0;
        //Debug.Log("Monster is Attacking");
        totalAnimationTime += monster.PerformAction(monsterAction);
        totalAnimationTime += player.ReceiveAction(monsterAction);

        monsterAction = null;
        StartCoroutine(ConfirmStatus(totalAnimationTime));
    }

    //Confirm the Status of who was attacked, Called After Someone is attacked (Check for Deaths)
    public IEnumerator ConfirmStatus(float time)
    {
        yield return new WaitForSeconds(time);

        if (!death) StartCoroutine(ContinueTurn(0.3f));

        else if (death) EndBattle();
    }

    //Ends the Turn and Allows Participants to choose Actions Again
    void EndTurn()
    {

        playerAction = null;
        monsterAction = null;
       // Debug.Log("Turn Ending");

        StartCoroutine(ChooseActions()); //Run after 1 second
    }

    //Ends the Battle After Someone has Died
    void EndBattle()
    {
        if (player.dead)
        {
            Debug.Log("Player Died");
            interfaceManager.AddMessage("You Have Died a tragic death in the dungeon. another adventure's body to zombify");
        }
        else if (monster.dead)
        {
            StartCoroutine(FinishBattle(2, false));

            Debug.Log("Player has Won the battle!");
        }
    }

    public void RunAway()
    {
        StartCoroutine(FinishBattle(2, true));
        interfaceManager.AddMessage("You Successefully Ran Away!");
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

    IEnumerator FinishBattle(float time, bool ranAway)
    {
        yield return new WaitForSeconds(time);

        MonsterRoomManager.inst.EndBattle();
        if (ranAway) MonsterRoomManager.inst.RemoveMonster();
        interfaceManager.EndBattle(ranAway);
    }

}
