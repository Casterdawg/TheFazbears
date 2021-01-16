using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricPower : AbilityBase
{
    //The visual effect that will be utilized
    public VisualEffect lightning;

    //The range of the raycast
    public float stunRange = 2f;

    //For further explination of these variables, see the gravpower script
    private RaycastHit hit;
    private Vector3 forward;


    //When the ability starts, populate variables and play the animation for the lighting effect
    public override void AbilityStart()
    {
        base.AbilityStart();
        forward = transform.TransformDirection(Vector3.forward);
        lightning.gameObject.SetActive(true);
        lightning.Stop();
        lightning.Play();

        //Produce a raycast that will check for enemies of interactables
        if(Physics.Raycast(transform.position, forward, out hit, stunRange))
        {
            if(hit.collider.TryGetComponent(out FollowAI AI))
            {
                AI.OnElectricuted();
            }
            if(hit.collider.CompareTag("ElectricInteractable"))
            {
                hit.collider.GetComponent<InteractionBase>().OnInteracted();
            }
        }

        //Disable this ability once it is used
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
