using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Controller for the individual Inventory Slots in the Inventory
/// 
/// </summary>
public class InventorySlotIF : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    InventoryIFManager manager;

    [SerializeField]
    GameObject InventoryManagerMenuPrefab;

    [SerializeField]
    HudManager navigationInterface;

    GameObject itemImage;

    [SerializeField]
    int id;

    public int ID => id;

    [SerializeField]
    GameObject QuantityDisplayer;

    Text QuantityText;

    bool InItemSwitch;

    void Awake()
    {
        InItemSwitch = false;

        rectTransform = GetComponent<RectTransform>();

        InventorySlot.onSlotNewItem += newItem;
        InventorySlot.onSlotClearItem += ClearItem;
        InventorySlot.onSlotUpdateItem += UpdateQuantity;

        QuantityText = QuantityDisplayer.GetComponentInChildren<Text>();
        QuantityDisplayer.SetActive(false);
    }

    void OnDestroy()
    {
        InventorySlot.onSlotNewItem -= newItem;
        InventorySlot.onSlotClearItem -= ClearItem;
        InventorySlot.onSlotUpdateItem -= UpdateQuantity;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (navigationInterface is BattleInterFaceManager) { 
        InventorySlot s = manager.Inventory.getSlot(ID);
        Item t = s.getItem();
        if (t != null)
        {
            if (t is HealItem)
            {
                GameObject imgPrefab = DataBase.inst.consumablePrefabs[s.getItem().ID];
                itemImage = Instantiate(imgPrefab, this.gameObject.transform);
                itemImage.transform.SetAsFirstSibling();

                QuantityDisplayer.SetActive(true);
                UpdateQuantity(ID, s.CurrentQuantity());
            }
            else if (t is EquipableItem)
            {
                GameObject imgPrefab = DataBase.inst.gearPrefabs[s.getItem().ID];
                itemImage = Instantiate(imgPrefab, this.gameObject.transform);
                itemImage.transform.SetAsFirstSibling();

                QuantityDisplayer.SetActive(false);
                UpdateQuantity(ID, s.CurrentQuantity());
            }
        }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openMenu()
    {
        if (!InItemSwitch)
        {
            manager.CloseAllMenus();

            if (getItem() != null)
            {
                GameObject o = Instantiate(InventoryManagerMenuPrefab, rectTransform.position, Quaternion.identity, navigationInterface.gameObject.transform);
                ItemManagementMenu s = o.GetComponent<ItemManagementMenu>();
                manager.menus.Add(o);
                s.inventorySlotIF = this;
                s.inventoryIFManager = manager;
            }
        } else
        {
            manager.PerformItemSwitch(ID);
        }
    }

    public void openBattleMenu()
    {
        manager.CloseAllMenus();
        Item i = getItem();
        if (i != null && i is HealItem)
        {            
            GameObject o = Instantiate(InventoryManagerMenuPrefab, rectTransform.position, Quaternion.identity, navigationInterface.gameObject.transform);
            ItemManagementMenu s = o.GetComponent<ItemManagementMenu>();
            manager.menus.Add(o);
            s.inventorySlotIF = this;
            s.inventoryIFManager = manager;
        }
    }

    public void CloseMenu()
    {

    }

    protected void newItem(int i)
    {
        if (this.id == i)
        {
            InventorySlot s = manager.Inventory.getSlot(i);
            Item t = s.getItem();

            if (t is HealItem)
            {
                GameObject imgPrefab = DataBase.inst.consumablePrefabs[s.getItem().ID];
                itemImage = Instantiate(imgPrefab, this.gameObject.transform);
                itemImage.transform.SetAsFirstSibling();

                QuantityDisplayer.SetActive(true);
                UpdateQuantity(i, s.CurrentQuantity());
            }
            else if (t is EquipableItem)
            {
                GameObject imgPrefab = DataBase.inst.gearPrefabs[s.getItem().ID];
                itemImage = Instantiate(imgPrefab, this.gameObject.transform);
                itemImage.transform.SetAsFirstSibling();

                QuantityDisplayer.SetActive(false);
                UpdateQuantity(i, s.CurrentQuantity());

            }
        }
    }

    public void ClearItem(int i)
    {
        if (this.id == i)
        {
            Destroy(itemImage.gameObject);
            QuantityDisplayer.SetActive(false);
        }
    }

    public void UpdateQuantity(int i, int newQuantity)
    {
        if (this.id == i) QuantityText.text = newQuantity.ToString();

    }

    public void UseItem()
    {

    }

    public void MoveItem()
    {

    }

    public void DiscardItem()
    {
        manager.Inventory.getSlot(id).Discard();
    }

    public Item getItem()
    {
        return manager.Inventory.getSlot(id).getItem();
    }

    public void StartItemSwitch()
    {
        InItemSwitch = true;
    }

    public void EndItemSwitch()
    {
        InItemSwitch = false;
    }





}
