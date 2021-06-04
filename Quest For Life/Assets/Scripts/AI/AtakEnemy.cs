using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtakEnemy : Node
{
    Enemy enemy;
    float dmg;

    public AtakEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override NodeState Evaluate()
    {
       // Debug.Log("\nATK");

        AttackAction attack = new AttackAction(enemy, enemy.entityEnemy, enemy.BaseAttackPower, 100, Global.Type.NONE);
        enemy.BattleActionChangetoAtk(attack);
        return NodeState.SUCCESS;
    }
}
