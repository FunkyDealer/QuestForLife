﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global 
{
    public enum Type
    {
        FIRE,
        WATER,
        THUNDER,
        LIGHT,
        DARKNESS,
        NONE
    }

    public enum FacingDirection
    {
        NORTH,
        EAST,
        WEST,
        SOUTH
    }

    public enum AttackEffect
    {
        HEAL,
        DEATH,
        NONE
    }

   public enum HealType
    {
        HEALTH,
        MANA,
        BOTH
    }

    public enum GearType
    {
        HAT,
        RING,
        BODYCLOTHING,
        BELT,
        WEAPON
    }


    public struct DungeonMonsterInfo
    {
        public string Name;
        public int minFloor;
        public int maxFloor;
        public int id;
        public int BaseHealth;
        public int BaseMana;
        public int BasePower;
        public int BaseDefence;
        public int BaseAccuracy;
        public int BaseDodge;
        public int BaseSpeed;

        public int BaseAttackPower;
        public int BaseReward;
        public int BaseExpReward;

        public Type Resistence;
        public Type Weakness;

        public int HealthGainPerLevel;
        public int ManaGainPerLevel;
        public int PowerGainPerLevel;
        public int DefenceGainPerLevel;
        public int AccuracyGainPerLevel;
        public int DodgeGainPerLevel;
        public int SpeedGainPerLevel;

        public Spell[] spells;

    }

    public struct Spell
    {
        public string Name;
        public int Id;
        public Type type;
        public int Power;
        public int Cost;
        public int Accuracy;
        public AttackEffect effect;
        public bool PlayerLearn;
        public int LevelLearn;
    }

    static int getSeed(MapManager m)
    {
        int seed = m.MapSeeds[m.CurrentSeed];
        m.increaseSeed();
        return seed;
    }

    /// <summary>
    /// Return an integer number between min[INCLUSIVE] and max[EXLUSIVE] (read only)
    /// </summary>
    public static int Range(int min, int max, MapManager m)
    {
        int n = 0;
        Random.State originalState = Random.state;
        Random.InitState(getSeed(m));
        n = Random.Range(min, max);
        Random.state = originalState;
        return n;
    }

    public static float Value(MapManager m)
    {
        float n = 0;
        Random.State originalState = Random.state;
        Random.InitState(getSeed(m));
        n = Random.value;
        Random.state = originalState;
        return n;

    }

}
