using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Grounded : State
{
    //This variable is used for managing input listeners
    private PlayerStateController privateController;

    //When the player enters the grounded state, then populate variables, reset the jump count, and add an input listener for aiming
    public override void Enter(PlayerStateController controller)
    {
        privateController = controller;
        controller.masterInput.Player.Aim.performed += AimPressed;
        controller.jumpCount = 0;
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
    }

    //When you exit the grounded state, remove the input listener for aiming
    public override void Exit(PlayerStateController controller)
    {
        controller.masterInput.Player.Aim.performed -= AimPressed;
    }

    //When the player presses the aim button, then change state to aiming
    private void AimPressed(InputAction.CallbackContext context)
    {
        privateController.SetState(new Aiming());
    }
}
