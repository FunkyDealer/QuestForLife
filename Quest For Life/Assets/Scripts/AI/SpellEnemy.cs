using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEnemy : Node
{
    Global.Spell spell;
    Enemy enemy;
    // EnemyAI aI;
    float prob;

    public SpellEnemy(Enemy en, Global.Spell spell, float prob)
    {
        enemy = en;
        this.spell = spell;
        this.prob = prob * 10;
    }


    public override NodeState Evaluate()
    {
        int i = Random.Range(0, 10);

        //Debug.Log("\n" + i + "_" + prob);

        if (spell.Cost > enemy.currentMana)
        {
          //  Debug.Log("\nERROR_MANA");
            return NodeState.FAILURE;
        }
        else
        {
            if (i <= prob)
            {
              //  Debug.Log("\nATACOU " + spell.Name);
                CastSpellAction _spell = new CastSpellAction();
                _spell.spell = this.spell;
                _spell.Target = enemy.entityEnemy;
                _spell.user = enemy;
                enemy.BattleActionChangetoSpell(_spell);
                return NodeState.SUCCESS;
            }
            else
            {
              //  Debug.Log("\nERROR_PROB");
                return NodeState.FAILURE;

            }
        }
    }


}
