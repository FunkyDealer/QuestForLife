using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargetButton : BattleButton
{
    [SerializeField]
    bool enemy;
    [HideInInspector]
    public Global.Spell assignedSpell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        base.Activate();

        if (enemy) battleManager.CastSpell(assignedSpell, battleManager.enemy);
        else battleManager.CastSpell(assignedSpell, battleManager.player);
    }
}
