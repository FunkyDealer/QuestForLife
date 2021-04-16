using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSellMenuController : ShopMenuController
{

    protected void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void PopulateTable()
    {
        base.PopulateTable();

        Inventory inventory = manager.player.Inventory;

        for (int i = 0; i < inventory.Slots.Length; i++)
        {
            if (!inventory.getSlot(i).isEmpty())
            {
                InventorySlot IS = inventory.Slots[i];

                GameObject o = Instantiate(ItemDisplayPrefab, itemMenuTable.transform);
                SellItemDisplayController s = o.GetComponent<SellItemDisplayController>();
                s.Init(IS, manager.ShopManager, this);
                totalItems++;
                if (totalItems > TotalPages * itemsPerPage) TotalPages++;
            }
        }

        if (TotalPages > 0) currentPage = 1;
        else currentPage = 0;
    }

    public void SellItem(GameObject i)
    {
        Destroy(i);

        int previousTotalitem = totalItems;
        totalItems--;

        if (itemsPerPage * (TotalPages - 1) == totalItems)
        {
            TotalPages--;
            if (currentPage > TotalPages)
            {
                currentPage = TotalPages;
                TableDisplay.localPosition = new Vector3(400 * (currentPage - 1), 0);
            }
        }

        Setpages();
        
    }

    public void SellAmmount(int ammount, int cost)
    {
        player.addGold(cost * ammount);
        manager.UpdateMoneyDisplay();
    }

}
