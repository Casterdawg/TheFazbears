using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : AbilityBase
{
    //The fireball that will be spawned when the ability is usedd
    public GameObject fireBallPrefab;
    //The speed the fireball will move at when it is spawned
    public float launchSpeed;

    //Varible used to reference the spawned fireball
    private GameObject spawnedFireBall;

    //The rigidbody of the spawned fireball
    private Rigidbody rig;

    public override void AbilityStart()
    {
        base.AbilityStart();

        //Spawn the fireball and populate the variable at the same time. Ensure it will spawn and rotate based on the object this script is attached to
        spawnedFireBall = Instantiate(fireBallPrefab, transform.position, transform.rotation);

        //Populate the rigidbody using the spawned fireball
        rig = spawnedFireBall.GetComponent<Rigidbody>();

        //Add force to the rigidbody using the launch speed provided
        rig.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);

        //End this ability by disabing the script
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
