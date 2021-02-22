using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : State
{
    //These two variables are used to check to see if you are falling while airborn to check for ledges
    private Vector3 currentPos;
    private Vector3 lastPos;

    private Vector3 forward;

    //When the player enters the airborn state, play the jump animation (Will be changed to a fall animation in the future)
    public override void Enter(PlayerStateController player)
    {
        player.currentAnimator.SetTrigger("Jump");
        //Make the jump count 1 to make sure you can't double jump while midair
        player.jumpCount = 1;
        //Record the most recent postion of the player
        currentPos = player.currentController.transform.position;
        lastPos = currentPos;
    }

    //A multidude of things are checked while airborn, with most of this code relating to checking for ledges
    public override void Update(PlayerStateController player)
    {
        //get the forward vector to ensure the raycast works
        forward = player.ledgeCheckX.TransformDirection(Vector3.forward);

        //These two lines are for debugging the game. It will show a visual representation of the raycasts we will produce later. This helps when adjusting how long we will make the raycasts in the final build
        Debug.DrawRay(player.ledgeCheckX.position, forward * 0.1f, Color.green);
        Debug.DrawRay(player.ledgeCheckY.position, Vector3.down * 0.05f, Color.green);

        //Have the player move normally, but without the running animations
        player.Move();

        //If the player becomes grounded then, switch backk to the grounded state
        if (player.currentController.isGrounded == true)
        {
            player.SetState(new Grounded());
            return;
        }

        //Record the current position the player is in
        currentPos = player.currentController.transform.position;

        //Check the current position of the player and compare it to the last recorded position
        //If the y postion of the current position is less than the last recorded position the prior frame, then start the check for ledges
        if (currentPos.y < lastPos.y)
        {
            //If the player is not goldy, then continue the check for ledges
            if (!player.isGoldy)
            {
                //Store the raycast data for future use
                RaycastHit hitX;
                RaycastHit hitY;
                //If both of the raycasts are hitting an object, then continue the check for ledges
                if (Physics.Raycast(player.ledgeCheckX.position, forward, out hitX, 0.1f) && Physics.Raycast(player.ledgeCheckY.position, Vector3.down, out hitY, 0.05f))
                {
                    //If both of the collisions have the Ledge tag, then enter the climbing state and record the hit information to be used later on
                    if (hitX.collider.gameObject.CompareTag("Ledge") && hitY.collider.gameObject.CompareTag("Ledge"))
                    {
                        player.hitX = hitX;
                        player.hitY = hitY;
                        player.SetState(new Climbing());
                    }
                }
            }
        }

        //Now that all of the actions in this method are complete, then record the last recorded position for the next check
        lastPos = currentPos;
    }

    //When you exit the airborn state, switch to grounded animations
    //I will have to look into this later, but I have noticed cases where the player is stuck in the airborn animations even when they exit the state.
    public override void Exit(PlayerStateController controller)
    {
        controller.currentAnimator.ResetTrigger("Jump");
    }
}

