using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPower : MonoBehaviour
{
    public Transform floatPoint;
    public float launchSpeed;

    //Camera cam;

    //GameObject target;
   // Rigidbody targetRig;

    public float weaponRange = 20f;

    bool isAttracting;

   // bool isLaunching;

    //void Start()
    //{
    ////    cam = Camera.main;
    //}

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           // print("Click works");
            isAttracting = !isAttracting;
        }
    }

    private void FixedUpdate()
    {
        if (isAttracting)
        {
            //Attract();
        }
        else if (!isAttracting)
        {
            Throw();
        }
    }


    //private void Attract()
    //{
    //    RaycastHit hit;

    //    if(target == null)
    //    {
    //        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weaponRange))
    //        {
    //            if (hit.transform.tag == "Throwable")
    //            {
    //                target = hit.transform.gameObject;
    //                targetRig = target.GetComponent<Rigidbody>();
    //                target.transform.position = Vector3.MoveTowards(target.transform.position, floatPoint.position, 0.3f);
    //                targetRig.useGravity = false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        target.transform.position = Vector3.MoveTowards(target.transform.position, floatPoint.position, 0.3f);
    //    }
    //}

    private void Throw()
    {
        //if(targetRig != null)
        //{
        //    targetRig.useGravity = true;
        //    targetRig.AddForce(floatPoint.transform.forward * launchSpeed, ForceMode.Impulse);
        //   // target = null;
        //}
    }
}
