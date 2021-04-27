using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Controller for the Navigation Interface, Mother Controller for the while interface
/// 
/// </summary>
public class NavigationInterfaceManager : HudManager
{
    [HideInInspector]
    public MapManager mapManager;

    [SerializeField]
    GameObject inventory;

    [HideInInspector]
    public PlayerMov playerMov;

    bool inventoryOn;
    float inputDelayTimer = 0;
    float inputDelayTime = 0.1f;

    [SerializeField]
    InventoryIFManager inventoryIFManager;

    [SerializeField]
    MiniMapManager miniMapManager;
    
    [SerializeField]
    Text FloorDisplaytext;

    void Awake()
    {
        
    }



    public void getInformation(Player player, PlayerMov playerMov, MapManager mapmanager) 
    {
        this.player = player;
        this.mapManager = mapmanager;
        this.playerMov = playerMov;

        inventory.SetActive(false);
        inventoryOn = false;

        Player.onFloorChange += updateFloor;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDestroy()
    {
        Player.onFloorChange -= updateFloor;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryOn)
        {
            if (inputDelayTimer < inputDelayTime) inputDelayTimer += Time.deltaTime;
            else
            {
                if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("CloseMenu"))
                {
                    CloseInventory();
                }
            }
        }
    }

   public void OpenInventory()
    {
        inventoryOn = true;

        FloorDisplaytext.gameObject.SetActive(false);

        inventory.SetActive(true);
    }

   void CloseInventory()
    {
        CloseAllLooseMenus();

        inventoryIFManager.StopItemSwitch();

        inputDelayTimer = 0;
        inventoryOn = false;
        inventory.SetActive(false);

        FloorDisplaytext.gameObject.SetActive(true);

        playerMov.ResumeMovement();
    }

    void CloseAllLooseMenus()
    {
        inventoryIFManager.CloseAllMenus();
    }

    public void CreateNewMinimap()
    {
        miniMapManager.GenerateNewMiniMap();
    }

    void updateFloor(int floor)
    {
        this.FloorDisplaytext.text = $"Floor {floor}";
    }
    

}
