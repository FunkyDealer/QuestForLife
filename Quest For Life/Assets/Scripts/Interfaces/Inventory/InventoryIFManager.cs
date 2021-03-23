using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// manager for the Inventory Interface, controls the Inventory Window
/// 
/// </summary>
public class InventoryIFManager : MonoBehaviour
{
    [SerializeField]
    NavigationInterfaceManager navigationInterface;

    [SerializeField]
    List<InventorySlotIF> interfaceSlots;

    Inventory inventory;

    [HideInInspector]
    public List<GameObject> menus;


    void Awake()
    {
        menus = new List<GameObject>();
        


    }

    // Start is called before the first frame update
    void Start()
    {

        inventory = navigationInterface.player.Inventory;


    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public void Close()
    {

        foreach (var m in menus)
        {
            Destroy(m);
        }

        menus.Clear();
    }



}
