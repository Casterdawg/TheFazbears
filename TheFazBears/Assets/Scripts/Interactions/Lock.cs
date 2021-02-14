using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : InteractionBase
{
    public Rigidbody door;
    private Rigidbody rig;
    private AudioSource player;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        player = GetComponent<AudioSource>();
    }

    public override void InteractSucessful()
    {
        base.InteractSucessful();

        player.Play();

        Debug.Log("Lock Interaction");

        rig.constraints = RigidbodyConstraints.None;

        door.constraints = RigidbodyConstraints.None;

        rig.useGravity = true;
    }
}
