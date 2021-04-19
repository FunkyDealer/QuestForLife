using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSlotIF : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField]
    InventoryIFManager manager;

    [SerializeField]
    GameObject GearManagerMenuPrefab;

    [SerializeField]
    NavigationInterfaceManager navigationInterface;

    GameObject itemImage;

    [SerializeField]
    Global.GearType type;

    bool InItemSwitch;

    [SerializeField]
    int ID;

    void Awake()
    {
        InItemSwitch = false;

        rectTransform = GetComponent<RectTransform>();

        GearSlot.onSlotNewItem += newItem;
        GearSlot.onSlotClearItem += ClearItem;
    }

    void OnDestroy()
    {
        GearSlot.onSlotNewItem -= newItem;
        GearSlot.onSlotClearItem -= ClearItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

            if (hasItem())
            {
                GameObject o = Instantiate(GearManagerMenuPrefab, rectTransform.position, Quaternion.identity, navigationInterface.gameObject.transform);
                GearManagementMenu s = o.GetComponent<GearManagementMenu>();
                manager.menus.Add(o);
                s.gearSlot = this;
                s.inventoryIFManager = manager;
            }
        }
        else
        {
           
        }
    }

    void newItem(int i)
    {
        if (this.ID == i)
        {
            GearSlot s = manager.GetGearSlot(ID);
            Debug.Log($"adding a {s.GetItem().Name}");

            GameObject imgPrefab = DataBase.inst.gearPrefabs[s.GetItem().ID];
            itemImage = Instantiate(imgPrefab, this.gameObject.transform);
            itemImage.transform.SetAsFirstSibling();
        }
    }

    public void ClearItem(int i)
    {
        if (this.ID == i)
        {
            Destroy(itemImage.gameObject);
        }
    }

    public bool hasItem()
    {
        return manager.hasGear(ID);

        return false;
    }


    public bool UnequipItem()
    {
        if (manager.UnequipItem(ID)) return true;


        return false;
    }

}
