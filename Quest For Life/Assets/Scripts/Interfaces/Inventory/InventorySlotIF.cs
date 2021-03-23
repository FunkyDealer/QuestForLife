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
    InventoryIFManager inventoryIFManager;

    [SerializeField]
    GameObject InventoryManagerMenuPrefab;

    [SerializeField]
    NavigationInterfaceManager navigationInterface;

    void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();        

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
        inventoryIFManager.Close();


        GameObject o = Instantiate(InventoryManagerMenuPrefab, rectTransform.position, Quaternion.identity, navigationInterface.gameObject.transform);
        inventoryIFManager.menus.Add(o);    

        

    }

    public void CloseMenu()
    {

    }

}
