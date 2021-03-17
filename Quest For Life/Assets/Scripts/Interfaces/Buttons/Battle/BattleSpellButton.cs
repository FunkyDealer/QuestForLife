using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpellButton : BattleButton
{
    public Global.Spell assignedSpell;

    

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

        battleManager.CastSpell(assignedSpell);
    }

    
}
