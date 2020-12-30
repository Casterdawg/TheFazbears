using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravPower : AbilityBase
{
    public Transform floatPoint;
    public float launchSpeed;
    public float abilityRange;
    public PlayerStateController player;
    public Transform cam;

    private GameObject target;
    private Rigidbody targetRig;
    private RaycastHit hit;

    private Vector3 forward;


    public override void AbilityStart()
    {
        base.AbilityStart();

        forward = cam.TransformDirection(Vector3.forward);

        if (Physics.Raycast(cam.transform.position, forward, out hit, abilityRange)
            && hit.transform.tag == "Throwable")
        {
            target = hit.transform.gameObject;
            targetRig = target.GetComponent<Rigidbody>();
            targetRig.useGravity = false;
            player.masterInput.Player.AimAbility.started += ShootObject;
            targetRig.velocity = Vector3.zero;
        }
        else
        {
            enabled = false;
        }
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
        if(target != null)
            target.transform.position = Vector3.MoveTowards(target.transform.position, floatPoint.position, 0.3f);
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();

        player.masterInput.Player.AimAbility.started -= ShootObject;

        if(target != null)
        {
            targetRig.useGravity = true;
            target = null;
        }
        
    }


    private void Throw()
    {
        if(targetRig != null)
        {
            targetRig.AddForce(floatPoint.transform.forward * launchSpeed, ForceMode.Impulse);
        }
    }

    private void ShootObject(InputAction.CallbackContext context)
    {
        Throw();
        enabled = false;
    }
}
