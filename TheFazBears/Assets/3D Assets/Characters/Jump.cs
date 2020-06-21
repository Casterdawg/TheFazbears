using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private bool isGrounded;

    private bool doubleJump;

    public bool TryJump()
    {
        bool canJump = isGrounded || doubleJump;

        if (!isGrounded)
            doubleJump = false;

        return canJump;
    }

    void OnTriggerEnter(Collider theCollision)
    {
        doubleJump = true;
        isGrounded = true;
        print("collided");
    }

    void OnTriggerExit(Collider theCollision)
    {
        isGrounded = false;
        print("notcollided");
    }
}