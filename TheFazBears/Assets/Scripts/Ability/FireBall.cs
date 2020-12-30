using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AbilityBase
{
    public override void AbilityStart()
    {
        base.AbilityStart();
        Debug.Log("Fireball used");
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();
        Debug.Log("Fireball ended");
    }
}
