using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Grounded : State
{
    //This variable is used for managing input listeners
    private PlayerStateController privateController;
    private RaycastHit hit;
    private int downwardRange = 10;

    //When the player enters the grounded state, then populate variables, reset the jump count, and add an input listener for aiming
    public override void Enter(PlayerStateController controller)
    {
        privateController = controller;
        controller.masterInput.Player.Aim.performed += AimPressed;
        controller.jumpCount = 0;
        controller.currentAnimator.SetTrigger("Grounded");
    }

    //Slope check is used to make sure the player can't jump up steep surfaces
    private void SlopeCheck()
    {
        if (privateController.currentController != null)
        {
            Transform transform = privateController.currentController.transform;

            Vector3 downward = transform.TransformDirection(Vector3.down);

            if (Physics.Raycast(transform.position, downward, out hit, downwardRange))
            {
                Vector3 tangent = Vector3.Cross(hit.normal, Vector3.up);
                Vector3 down = Vector3.Cross(hit.normal, tangent);

                if(down.y < -0.8)
                {
                    if(privateController.masterInput.Player.Jump.enabled == true)
                    {
                       // Debug.Log("Jump disabled");
                        privateController.masterInput.Player.Jump.Disable();
                    }
                }
                else
                {
                    if(privateController.masterInput.Player.Jump.enabled == false)
                    {
                       // Debug.Log("Jump enabled");
                        privateController.masterInput.Player.Jump.Enable();
                    }
                }
            }
        }
    }

    //While in the grounded state, use normal movement, utilize the animator, and when the player is not grounded, set state to airborn.
    public override void Update(PlayerStateController controller)
    {
        controller.Move();
        float animationSpeedPercent = ((controller.running) ? controller.currentSpeed / controller.runSpeed : controller.currentSpeed / controller.walkSpeed * .5f);
        controller.currentAnimator.SetFloat("speedPercent", animationSpeedPercent, controller.speedSmoothTime, Time.deltaTime);

        if(controller.currentController.isGrounded == false)
        {
            controller.SetState(new Airborn());
        }

        SlopeCheck();
    }

    //When you exit the grounded state, remove the input listener for aiming
    public override void Exit(PlayerStateController controller)
    {
        controller.masterInput.Player.Aim.performed -= AimPressed;
        controller.currentAnimator.ResetTrigger("Grounded");
    }

    //When the player presses the aim button, then change state to aiming
    private void AimPressed(InputAction.CallbackContext context)
    {
        privateController.SetState(new Aiming());
    }
}
