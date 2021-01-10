using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDoor : MonoBehaviour
{
    public bool canOpenDoor;

    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");


        rig.AddForce(new Vector3(0, 1, 0));
    }
}
