using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Grounded : State
{
    private PlayerStateController privateController;

    public override void Enter(PlayerStateController controller)
    {
        //Debug.Log("Walk started");
        privateController = controller;
        controller.masterInput.Player.Aim.performed += AimPressed;
        controller.jumpCount = 0;
    }

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

    public override void Exit(PlayerStateController controller)
    {
        controller.masterInput.Player.Aim.performed -= AimPressed;
    }

    private void AimPressed(InputAction.CallbackContext context)
    {
        privateController.SetState(new Aiming());
    }
}
