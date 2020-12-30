using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPower : AbilityBase
{
    public override void AbilityStart()
    {
        base.AbilityStart();
        Debug.Log("Electric used");
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();
        Debug.Log("Electric ended");
    }
}
