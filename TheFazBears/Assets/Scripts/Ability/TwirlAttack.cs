using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwirlAttack : AbilityBase
{
    //The range of the attackk
    public float hitRange = 1;

    //For more info on this variables, chec the gravpower script
    private RaycastHit hit;
    private Vector3 forward;

    //When the ability starts, cast a raycast from the player to check for an enemy
    public override void AbilityStart()
    {
        base.AbilityStart();

        forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, forward, out hit, hitRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy!");
            }
        }

        //Later on, when an animation for this attack is made, it will be activated here

        //Disable the script when this action is compleated
        enabled = false;
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();
    }
}
