using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    Player player;
    Text text;
    void Start()
    {
        player = FindObjectOfType<Player>();
        text = GetComponent<Text>();

    }

    
    void Update()
    {
        text.text = $"Health: {player.maxHealth}\nMana: {player.maxMana}\nPower: {player.Power}\nDefence: {player.Defence}\nAccuracy:{player.Accuracy}\nDodge: {player.Dodge}\nSpeed: {player.Speed}";
    }
}
