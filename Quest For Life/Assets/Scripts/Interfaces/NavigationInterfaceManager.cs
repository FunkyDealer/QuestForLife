using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// Controller for the Navigation Interface, Mother Controller for the while interface
/// 
/// </summary>
public class NavigationInterfaceManager : HudManager
{
    [HideInInspector]
    public DungeonManager dungeonManager;

    [SerializeField]
    GameObject inventory;

    [HideInInspector]
    public PlayerMov playerMov;

    bool inventoryOn;
    float inputDelayTimer = 0;
    float inputDelayTime = 0.1f;

    [SerializeField]
    InventoryIFManager inventoryIFManager;


    public void getInformation(Player player, PlayerMov playerMov, DungeonManager dungeonManager) 
    {
        this.player = player;
        this.dungeonManager = dungeonManager;
        this.playerMov = playerMov;

        inventory.SetActive(false);
        inventoryOn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryOn)
        {
            if (inputDelayTimer < inputDelayTime) inputDelayTimer += Time.deltaTime;
            else
            {
                if (Input.GetButtonDown("Inventory"))
                {
                    CloseInventory();
                }
            }
        }
    }

   public void OpenInventory()
    {
        inventoryOn = true;

        inventory.SetActive(true);
    }

   void CloseInventory()
    {
        CloseAllLooseMenus();

        inventoryIFManager.StopItemSwitch();

        inputDelayTimer = 0;
        inventoryOn = false;
        inventory.SetActive(false);

        playerMov.ResumeMovement();
    }

    void CloseAllLooseMenus()
    {
        inventoryIFManager.CloseAllMenus();
    }
}
