using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    [SerializeField]
    protected BattlePlayerStatsManager StatsManager;

    [SerializeField]
    MessageDisplayer messageDisplayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UseItemAction(int slot)
    {

    }

    public void AddMessage(string message, TextMessage.MessageSpeed speed) => messageDisplayer.AddMessage(message, speed);
}
