using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    public int moneyBase = 10;
    public int moneyMul = 10;

    public Animator animator;
    void Start()
    {
        isOpen = false;
    }

    public int OpenChest(int lvl, int floor)
    {
        isOpen = true;
        Animação();
        return moneyBase + moneyMul * (int)1.5 * floor * lvl;
    }

    void Animação()
    {
        animator.SetBool("Open", true);
    }
}
