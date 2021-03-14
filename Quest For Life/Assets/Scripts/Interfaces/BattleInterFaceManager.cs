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
    public GameManager gameManager;

    public void getInformation(Player player, BattleManager battleManager, GameManager gameManager)
    {
        this.player = player;
        this.battleManager = battleManager;
        this.gameManager = gameManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
