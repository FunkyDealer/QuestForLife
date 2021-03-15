using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BattleAction
{
    public Entity Target;
    public int attackBasePower;
    public Global.Type type;

    public AttackAction(Entity user,Entity Target, int attackPower, Global.Type type)
    {
        this.attackBasePower = attackPower;
        this.Target = Target;
        this.user = user;
        this.speed = user.Speed;
        this.type = type;
    }


}
