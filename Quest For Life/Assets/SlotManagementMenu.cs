using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Controller for the Menu that appears when clicking the individual Inventory Slot
/// 
/// </summary>
public class SlotManagementMenu : MonoBehaviour
{
    [HideInInspector]
    public InventorySlotIF inventorySlotIF;
    [HideInInspector]
    public InventoryIFManager inventoryIFManager;

    [SerializeField]
    Text UseButtonText;

    bool Consumable = false;

    // Start is called before the first frame update
    void Start()
    {
        if (inventorySlotIF.getItem() is HealItem)
        {
            Consumable = true;
            UseButtonText.text = "Use";
        }


    }

    void OnDestroy()
    {
       // inventoryIFManager.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useButton()
    {
        

    }

    void UseItem()
    {

    }

    void EquipItem()
    {

    }

    public void MoveStack()
    {
        inventoryIFManager.InitiateItemSwitch(inventorySlotIF.ID);

        CloseMenu();
    }

    public void DiscardStack()
    {
        inventorySlotIF.DiscardItem();
        CloseMenu();
    }

    void CloseMenu()
    {
        inventoryIFManager.CloseAllMenus();
    }

}
