using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviagationInterfaceManager : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public DungeonManager dungeonManager;

    public void getInformation(Player player, DungeonManager dungeonManager) 
    {
        this.player = player;
        this.dungeonManager = dungeonManager;
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
