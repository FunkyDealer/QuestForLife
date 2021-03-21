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

    [HideInInspector]
    public SpellMenuManager spellMenuManager;

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

        bool cast = false;

        if (!target) cast = battleManager.CastSpell(assignedSpell, battleManager.enemy);
        else cast = battleManager.ChooseTarget(assignedSpell);

        if (cast) spellMenuManager.disableAllContentMenus();
    }

    
}
