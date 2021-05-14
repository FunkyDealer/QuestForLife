using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public MapData mapData;
    public PlayerData playerData;
    public InventoryData inventoryData;
    public ShopData shopData;


    public SaveData(MapData mdata, PlayerData pData, InventoryData iData, ShopData sdata)
    {
        this.mapData = mdata;
        this.playerData = pData;
        this.inventoryData = iData;
        this.shopData = sdata;
    }
}
