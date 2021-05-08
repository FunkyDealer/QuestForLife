using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public MapData mapData;
    public PlayerData playerData;
    public ShopData shopData;


    public SaveData(MapData mdata, PlayerData pData, ShopData sdata)
    {
        this.mapData = mdata;
        this.playerData = pData;
        this.shopData = sdata;
    }
}
