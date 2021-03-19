using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpellButton : BattleButton
{
    [HideInInspector]
    public Global.Spell assignedSpell;
    [HideInInspector]
    public bool target;
    

    // Start is called before the first frame update
    void Start()
    {
        Text t = GetComponentInChildren<Text>();

        t.text = $"{assignedSpell.Name} : {assignedSpell.Cost}";


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate()
    {
        base.Activate();

        if (!target) battleManager.CastSpell(assignedSpell, battleManager.enemy);
        else battleManager.ChooseTarget(assignedSpell);
    }

    
}
