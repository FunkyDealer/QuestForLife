using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackAction : MonoBehaviour
{
    BattleManager battle;

    public Entity Target;
    public Attack ChosenAttack;

    private void Start()
    {
        battle = GetComponentInParent<BattleManager>();
    }

    private void Update()
    {
    }

    public void Atacar()
    {
        if (battle.round % 2 == 0)
        {
            battle.defender.DealDMG(20);
            battle.info_Display.text = "\n ATACASTE , HP: " + battle.defender.Cur_Healt();
            battle.Round();
        }
        else
        {
            battle.info_Display.text = "\n Nao podes";
        }

    }
}
