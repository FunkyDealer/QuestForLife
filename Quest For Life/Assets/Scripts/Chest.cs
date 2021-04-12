using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen;
    public int moneyBase = 10;
    public int moneyMul = 10;

    void Start()
    {
        isOpen = false;
    }

    public int OpenChest(int lvl, int floor)
    {
        isOpen = true;
        return moneyBase + moneyMul * (int)1.5 * floor * lvl;
    }

    void Animação()
    {

    }
}
