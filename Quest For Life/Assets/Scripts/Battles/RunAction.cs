using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunAction : BattleAction
{
    public int canRun = 0;
    public RunAction() 
    {
        System.Random rnd = new System.Random();
        canRun = rnd.Next(0, 10);
    }

}
