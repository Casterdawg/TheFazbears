using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwirlAttack : AbilityBase
{
    public override void AbilityStart()
    {
        base.AbilityStart();
        Debug.Log("Twirl used");
        this.enabled = false;
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();
        Debug.Log("Twirl ended");
    }
}
