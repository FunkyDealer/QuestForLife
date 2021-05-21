using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager
{

    static PlayerData savePlayer(Player player)
    {
        PlayerData data = new PlayerData(player);

        return data;
    }

    static int[] getOpenedChests(DungeonManager m)
    {
        if (m.Chests.Count > 0)
        {
            List<int> chests = new List<int>();

            foreach (var s in m.Chests.Values)
            {
                Chest C = s.GetComponent<Chest>();
                if (C.isOpen) chests.Add(C.id);
            }

            if (chests.Count > 0)
            {
                int[] ch = new int[chests.Count];

                for (int i = 0; i < ch.Length; i++)
                {
                    ch[i] = chests[i];
                }
                return ch;
            }
        }
        int[] c = new int[0];
        return c;
    }

    public static void saveGame(int slot, Player p)
    {
        int[] openedChests = getOpenedChests(p.gameManager.dungeonManager);

        MapData mapData = new MapData(p.gameManager.dungeonManager.Floor, p.currentMap.GetLength(0), p.currentMap.GetLength(1), p.gameManager.dungeonManager.MapSeeds, openedChests);
        PlayerData playerData = savePlayer(p);
        InventoryData inventoryData = new InventoryData(p);
        ShopData shopData = new ShopData(p.gameManager.shopManager.Shop);        

        SaveData data = new SaveData(mapData, playerData, inventoryData, shopData);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Slot" + slot + ".SAVE";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static SaveData LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/Slot" + slot + ".SAVE";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            stream.Close();

            return data;
        } else
        {

            Debug.Log($"Error trying to load game on slot {slot}");


        }


        return null;
    }

    public static bool isData(int slot)
    {
        string path = Application.persistentDataPath + "/Slot" + slot + ".SAVE";

        return File.Exists(path);

    }

    

}
