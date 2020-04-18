using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{

    public static bool isGrounded;


    void OnTriggerEnter(Collider theCollision)
    {

        isGrounded = true;
        print("collided");
    }
    void OnTriggerExit(Collider theCollision)
    {
        isGrounded = false;
        print("notcollided");
        Event Space = Event.current;
        if (Space.isKey)
        {
            isGrounded = false;
        }

    }
}