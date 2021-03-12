using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int Level;

    //Health and Mana
    protected int maxHealth;
    protected int currentHealth;
    protected int maxMana;
    protected int currentMana;

    //Other Stats
    protected int Power;
    protected int Defence;
    protected int Accuracy;
    protected int Dodge;
    protected int Speed;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float Cur_Healt()
    {
        return currentHealth;
    }
    public float Cur_Mana()
    {
        return currentMana;
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        Debug.Log("You heal " + heal + "Current healt is" + currentHealth);
    }
    public void DealDMG(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("You damage " + dmg + "Current healt is" + currentHealth);
    }
}
