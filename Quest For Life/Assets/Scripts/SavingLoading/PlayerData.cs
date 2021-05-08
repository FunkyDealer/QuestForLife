using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int[] currentTile;
    public int Orientation;

    public int maxHp;
    public int currentHp;
    public int maxMana;
    public int currentMana;

    public int Power;
    public int Defense;
    public int Accuracy;
    public int Dodge;
    public int Speed;

    public int CurrentExp;
    public int CurrentGold;

    public int[] knownSpells;

    public int[] keys;

    public bool inShop;
    public bool inFinalZone;
    public bool inDungeon;


    public PlayerData(Player player)
    {
        currentLevel = player.Level;

        currentTile = new int[2];
        currentTile[0] = player.currentTile.x;
        currentTile[1] = player.currentTile.y;

        Orientation = (int)player.direction;

        maxHp = player.maxHealth;
        currentHp = player.currentHealth;
        maxMana = player.maxMana;
        currentMana = player.currentMana;

        Power = player.Power;
        Defense = player.Defence;
        Accuracy = player.Accuracy;
        Dodge = player.Dodge;
        Speed = player.Speed;

        CurrentExp = player.Experience;
        CurrentGold = player.currentGold;

        knownSpells = new int[player.getKnownSpells().Count];
        if (knownSpells.Length > 0) {
            for (int i = 0; i < knownSpells.Length; i++)
            {
                knownSpells[i] = player.getKnownSpells()[i].Id;
            }
        }

        keys = new int[player.keys.Count];
        if (keys.Length > 0)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = player.keys[i];
            }
        }


        switch (player.location)
        {
            case Player.Location.DUNGEON:
                inShop = false;
                inFinalZone = false;
                inDungeon = true;
                break;
            case Player.Location.SHOP:
                inShop = true;
                inFinalZone = false;
                inDungeon = false;
                break;
            case Player.Location.FINALZONE:
                inShop = false;
                inFinalZone = true;
                inDungeon = false;
                break;
            default:
                break;
        }


    }



}
