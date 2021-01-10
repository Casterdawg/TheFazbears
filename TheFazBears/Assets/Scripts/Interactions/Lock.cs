using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : InteractionBase
{
    public Rigidbody door;
    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    public override void InteractSucessful()
    {
        base.InteractSucessful();

        Debug.Log("Lock Interaction");

        rig.constraints = RigidbodyConstraints.None;

        door.constraints = RigidbodyConstraints.None;
    }
}
