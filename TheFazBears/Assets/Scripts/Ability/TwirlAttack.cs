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

    public AudioSource audioSource;
    public AudioClip[] impact;
    public AudioClip[] miss;

    //When the ability starts, cast a raycast from the player to check for an enemy
    public override void AbilityStart()
    {
        int randNum = Random.Range(0, miss.Length - 1);
        base.AbilityStart();

        audioSource.clip = miss[randNum];

        forward = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, forward, out hit, hitRange))
        {
            if (hit.collider.TryGetComponent(out FollowAI AI))
            {
                randNum = Random.Range(0, impact.Length - 1);
                audioSource.clip = impact[randNum];
                AI.OnSmacked();
            }
        }

        audioSource.Play();

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
