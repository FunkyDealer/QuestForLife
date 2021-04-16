using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyMenuController : ShopMenuController
{



    protected  void Awake()
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

        for (int i = 0; i < manager.shop.EquipableInventory.Length; i++)
        {
            EquipableItem e = manager.shop.EquipableInventory[i];
            if (e != null)
            {
                GameObject o = Instantiate(ItemDisplayPrefab, itemMenuTable.transform);
                BuyItemDisplayController s = o.GetComponent<BuyItemDisplayController>();
                s.Init(e, i, manager.ShopManager, this);
                totalItems++;
                if (totalItems > TotalPages * itemsPerPage) TotalPages++;
            }
        }

        for (int i = 0; i < manager.shop.ConsumableInventory.Length; i++)
        {
            HealItem e = manager.shop.ConsumableInventory[i];
            if (e != null)
            {
                GameObject o = Instantiate(ItemDisplayPrefab, itemMenuTable.transform);
                BuyItemDisplayController s = o.GetComponent<BuyItemDisplayController>();
                s.Init(e, i, manager.ShopManager, this);
                totalItems++;
                if (totalItems > TotalPages * itemsPerPage) TotalPages++;
            }
        }

        if (TotalPages > 0) currentPage = 1;
        else currentPage = 0;
    }  

    public void BuyItem(GameObject i)
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
        manager.UpdateMoneyDisplay();
    }



}
