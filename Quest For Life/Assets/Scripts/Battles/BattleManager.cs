using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [HideInInspector]
    public Entity player;
    [HideInInspector]
    public Entity monster;

    //BattleAction monsterAction;
    //BattleAction playerAction;

    [HideInInspector]
    public BattleInterFaceManager interfaceManager;

    int roundNumber;

    bool death;

    bool battleOver;

    void Awake()
    {
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

        
        StartCoroutine(ChooseActions(0.7f)); //Run after 1 second

       interfaceManager.AddMessage($"A {monster.EntityName} Attacked!", TextMessage.MessageSpeed.SLOW);
    }

    //Start Choice Of Action from the Player (Ran by this script)
    IEnumerator ChooseActions(float time)
    {
        yield return new WaitForSeconds(time); //Wait for 1 second

        roundNumber++;
        interfaceManager.StartChoice();

        monster.ChooseAction(player);
    }

    //Receives the Actions From the player and Monster (Ran by the entities) Comes after ChooseAction()
    public void ReceiveActions(BattleAction action, Entity actor)
    {
        if (actor is Player) player.SetBattleAction(action);
        interfaceManager.EndChoice();
        interfaceManager.CloseAllMenus();

        if (player.currentBattleAction != null && monster.currentBattleAction != null) StartTurn();
    }

    //Started the Turn (Run by ReceiveActions() after both actions have been received
    void StartTurn()
    {
        if (player.currentBattleAction.speed > monster.currentBattleAction.speed)
        {
            PerformAction(player, player.currentBattleAction, monster);
        }
        else
        {
            PerformAction(monster, monster.currentBattleAction, player);
        }       
    }

    //Continue Turn (Called After someone has attacked)
    IEnumerator ContinueTurn(float time)
    {
        yield return new WaitForSeconds(time);

        if (player.currentBattleAction != null) PerformAction(player, player.currentBattleAction, monster);

        else if (monster.currentBattleAction != null) PerformAction(monster, monster.currentBattleAction, player);

        else EndTurn();
    }

    void PerformAction(Entity actor, BattleAction action, Entity target)
    {
        float totalAnimationTime = 0;
        totalAnimationTime += actor.PerformAction(action, target);

        actor.SetBattleAction(null);

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
        player.SetBattleAction(null);
        monster.SetBattleAction(null);

        StartCoroutine(ChooseActions(0.3f)); //Run after 1 second
    }

    //Ends the Battle After Someone has Died
    void EndBattle()
    {
        player.SetBattleAction(null);
        monster.SetBattleAction(null);

        if (player.dead)
        {
            Debug.Log("Player Died");
            interfaceManager.AddMessage("You Have Died a tragic death in the dungeon. another adventure's body to zombify", TextMessage.MessageSpeed.VERYSLOW);
        }
        else if (monster.dead)
        {
            float TimeforFinish = 2;
            Player p = (Player)player;
            Enemy m = (Enemy)monster;
            if (p.gainExp(m)) TimeforFinish += 4.5f;

            StartCoroutine(FinishBattle(TimeforFinish, false));

            Debug.Log("Player has Won the battle!");
        }
    }

    public void RunAway()
    {
        StartCoroutine(FinishBattle(2, true));
        interfaceManager.AddMessage("You Successefully Ran Away!", TextMessage.MessageSpeed.FAST);
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
