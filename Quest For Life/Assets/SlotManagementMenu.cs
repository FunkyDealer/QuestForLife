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
    public InventorySlot slot;
    [HideInInspector]
    public InventorySlotIF inventorySlotIF;

    [SerializeField]
    Text UseButtonText;

    bool Consumable = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (slot.getItem() is HealItem)
        //{
        //    Consumable = true;
        //    UseButtonText.text = "Use";
        //}


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

    }

    public void DiscardStack()
    {

    }

}
