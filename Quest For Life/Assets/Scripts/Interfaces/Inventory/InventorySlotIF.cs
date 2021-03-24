using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    NavigationInterfaceManager navigationInterface;

    GameObject itemImage;

    [SerializeField]
    int id;

    void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();

        InventorySlot.onSlotNewItem += newItem;

    }

    void OnDestroy()
    {
        InventorySlot.onSlotNewItem -= newItem;

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
        manager.Close();


        GameObject o = Instantiate(InventoryManagerMenuPrefab, rectTransform.position, Quaternion.identity, navigationInterface.gameObject.transform);
        manager.menus.Add(o);    

        

    }

    public void CloseMenu()
    {

    }

    void newItem(int i)
    {
        if (this.id == i)
        {

            InventorySlot s = manager.Inventory.getSlot(i);

            GameObject imgPrefab = DataBase.inst.ItemsPrefabs[s.getItem().ID];
            itemImage = Instantiate(imgPrefab, this.gameObject.transform);








        }
    }

}
