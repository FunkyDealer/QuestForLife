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
    bool pauseMenuOn;
    float inputDelayTimer = 0;
    float inputDelayTime = 0.1f;

    [SerializeField]
    InventoryIFManager inventoryIFManager;

    [SerializeField]
    MiniMapManager miniMapManager;
    
    [SerializeField]
    Text FloorDisplaytext;

    [SerializeField]
    GameObject keyHolder;

    [SerializeField]
    GameObject pauseMenu;

    void Awake()
    {
        
    }

    public void getInformation(Player player, PlayerMov playerMov, MapManager mapmanager) 
    {
        this.player = player;
        this.mapManager = mapmanager;
        this.playerMov = playerMov;

        FloorDisplaytext.gameObject.SetActive(true);

        inventory.SetActive(false);
        inventoryOn = false;
        pauseMenu.SetActive(false);
        pauseMenuOn = false;

        Player.onFloorChange += updateFloor;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var k in player.keys)
        {
            AddKey(k);
        }
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
                    return;
                }
            }
        }

        if (pauseMenuOn)
        {
            if (inputDelayTimer < inputDelayTime) inputDelayTimer += Time.deltaTime;
            else
            {
                if (Input.GetButtonDown("CloseMenu"))
                {
                    ClosePauseMenu();
                    return;
                }
            }
        }
    }

    public void OpenPauseMenu()
    {
        CloseInventory();

        pauseMenu.SetActive(true);
        pauseMenuOn = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuOn = false;
        pauseMenu.SetActive(false);

        playerMov.ResumeMovement();
    }

   public void OpenInventory()
    {
        inventoryOn = true;

        FloorDisplaytext.gameObject.SetActive(false);
        inputDelayTimer = 0;

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

    public void AddKey(int id)
    {
        Instantiate(PropsDataBase.inst.HudKeys[id], keyHolder.transform);
    }

    public void ClearKeyHolder()
    {
        int childCount = keyHolder.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(keyHolder.transform.GetChild(i).gameObject);
        }
    }


}
