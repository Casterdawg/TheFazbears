using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : State
{
    private Vector3 currentPos;
    private Vector3 lastPos;

    private Vector3 forward;

    public override void Enter(PlayerStateController player)
    {
        player.currentAnimator.SetTrigger("Jump");
        player.jumpCount = 1;
        currentPos = player.currentController.transform.position;
        lastPos = currentPos;
    }

    public override void Update(PlayerStateController player)
    {
        forward = player.ledgeCheckX.TransformDirection(Vector3.forward);
        Debug.DrawRay(player.ledgeCheckX.position, forward * 0.1f, Color.green);
        Debug.DrawRay(player.ledgeCheckY.position, Vector3.down * 0.05f, Color.green);
        player.Move();
        if (player.currentController.isGrounded == true)
        {
            player.SetState(new Grounded());
            return;
        }

        currentPos = player.currentController.transform.position;

        if (currentPos.y < lastPos.y)
        {
          //  Debug.Log("Falling");
            if (!player.isGoldy)
            {
                Debug.Log("Check for ledge");
                RaycastHit hitX;
                RaycastHit hitY;
                if (Physics.Raycast(player.ledgeCheckX.position, forward, out hitX, 0.1f) && Physics.Raycast(player.ledgeCheckY.position, Vector3.down, out hitY, 0.05f))
                {
                    if (hitX.collider.gameObject.tag == "Ledge" && hitY.collider.gameObject.tag == "Ledge")
                    {
                        player.hitX = hitX;
                        player.hitY = hitY;
                        player.SetState(new Climbing());
                    }
                }
            }
        }

        lastPos = currentPos;
    }

    public override void Exit(PlayerStateController goldy)
    {
        goldy.currentAnimator.SetTrigger("Grounded");
    }
}

