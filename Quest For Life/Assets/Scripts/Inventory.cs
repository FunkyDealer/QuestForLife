using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Stack> inventario;

    [SerializeField]
    int capacidade = 10;
    private void Start()
    {
        inventario = new List<Stack>();
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        Debug.Log(inventario.Count);
    //        foreach (Stack s in inventario)
    //        {
    //            Debug.Log(s.qnt);
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        HealItem heal = new HealItem(10, 2);
    //        AddItem(heal);
    //        Debug.Log("Poção dada");
    //    }
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        EquipableItem equipableItem = new EquipableItem(1);
    //        AddItem(equipableItem);
    //        Debug.Log("Item Dado");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Y)) 
    //    {
    //        HealItem heal = new HealItem(10, 2);
    //        RemoveItem(heal);
    //        Debug.Log("Removido");
    //    }
    //    if(Input.GetKeyDown(KeyCode.T)) 
    //    {
    //        EquipableItem equipableItem = new EquipableItem(1);
    //        RemoveItem(equipableItem);
    //        Debug.Log("Removido");
    //    }

    //}
    void AddItem(Item i)
    {
        if (capacidade > inventario.Count)
        {
            bool changes = false;
            foreach (Stack s in inventario)
            {
                if (s.item.ID == i.ID)
                {
                    s.qnt++;
                    changes = true;
                    break;
                }
            }
            if (!changes)
            {
                Stack s = new Stack(i);
                s.qnt++;
                inventario.Add(s);
            }
        }
    }

    void RemoveItem(Item i)
    {
        bool remover = false;
        Stack stack = new Stack(i);

        foreach (Stack s in inventario)
        {
            if (s.item.ID == i.ID)
            {
                s.qnt--;
                if (s.qnt == 0)
                {
                    stack = s;
                    remover = true;
                }
            }
        }
        if (remover)
        {
            inventario.Remove(stack);
        }
    }

    bool HaveThisItem(Item i)
    {
        foreach (Stack s in inventario)
        {
            if (s.item.ID == i.ID)
            {
                return true;
            }
        }

        return false;
    }
}
