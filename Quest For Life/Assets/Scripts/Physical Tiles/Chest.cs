using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : PhysicalTile
{
    public bool isOpen;
    public int moneyBase = 10;
    public int moneyMul = 10;
    public int id;

    Animator animator;

    void Awake()
    {
        isOpen = false;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (isOpen == true)
        {
            Animation();
        }
    }

    public int OpenChest(int lvl, int floor)
    {
        isOpen = true;
        Animation();
        int money = moneyBase + moneyMul * (int)1.5 * floor * lvl;
        return money;
    }

    void Animation()
    {
        animator.SetBool("Open", true);
    }
}
