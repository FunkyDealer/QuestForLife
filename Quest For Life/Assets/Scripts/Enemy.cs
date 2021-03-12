using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int BaseAttackPower;
    public Global.Type Resistence;
    public Global.Type Weakness;


    int HealthGainPerLevel;
    int ManaGainPerLevel;
    int PowerGainPerLevel;
    int DefenceGainPerLevel;
    int AccuracyGainPerLevel;
    int DodgeGainPerLevel;
    int SpeedGainPerLevel;

    int BaseMoneyReward;

    int MonsterID;
    int PrefabID;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
