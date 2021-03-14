using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [HideInInspector]
    public string EntityName;
    [HideInInspector]
    public int Level;

    //Health and Mana
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int maxMana;
    [HideInInspector]
    public int currentMana;

    //Other Stats
    [HideInInspector]
    public int Power;
    [HideInInspector]
    public int Defence;
    [HideInInspector]
    public int Accuracy;
    [HideInInspector]
    public int Dodge;
    [HideInInspector]
    public int Speed;

    [HideInInspector]
    public BattleManager battleManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
