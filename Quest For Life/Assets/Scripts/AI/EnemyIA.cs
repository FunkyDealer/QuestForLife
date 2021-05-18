using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA 
{
    public Enemy enemy;

    //Habilidades
    List<SpellEnemy> spellEnemies;
    AtakEnemy atk;
    //IA
    Selector SeqNode;
    Selector SeqN;
    Inverter inv;

    public EnemyIA(List<SpellEnemy> habilidades, AtakEnemy atak )
    {
        spellEnemies = habilidades;
        atk = atak;

        CreateNodes();
    }

    public void CreateNodes() 
    {
        SeqNode = new Selector();

        foreach(SpellEnemy sp in spellEnemies)
        {
            SeqNode.m_nodes.Add(sp);
        }

        inv = new Inverter(SeqNode);

        SeqN = new Selector(new List<Node> { SeqNode, atk });
    }

    public void DoSeqN()
    {
        SeqN.Evaluate();
    }
}
