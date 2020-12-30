using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : State
{
    private PlayerStateController privateController;
    public override void Enter(PlayerStateController controller)
    {
        privateController = controller;
       // Debug.Log("Aim started");
        controller.masterInput.Player.Aim.canceled += AimCanceled;
        controller.masterInput.Player.AimAbility.performed += AbilityUsed;
        controller.reticle.SetActive(true);
        controller.aimCam.gameObject.SetActive(true);

    }

    public override void Update(PlayerStateController controller)
    {
        controller.AimingMove();

        float animationSpeedPercent = ((controller.running) ? controller.currentSpeed / controller.runSpeed : controller.currentSpeed / controller.walkSpeed * .5f);
        controller.currentAnimator.SetFloat("speedPercent", animationSpeedPercent, controller.speedSmoothTime, Time.deltaTime);

        if (controller.currentController.isGrounded == false)
        {
            controller.SetState(new Airborn());
        }
    }

    public override void Exit(PlayerStateController controller)
    {
        controller.masterInput.Player.Aim.canceled -= AimCanceled;
        controller.masterInput.Player.AimAbility.performed -= AbilityUsed;
        controller.reticle.SetActive(false);
        controller.aimCam.gameObject.SetActive(false);
    }

    private void AbilityUsed(InputAction.CallbackContext context)
    {
        //Debug.Log("Aim ability used");

        privateController.currentAimAbility.enabled = true;
    }

    private void AimCanceled(InputAction.CallbackContext context)
    {
        privateController.currentAimAbility.enabled = false;
        privateController.SetState(new Grounded());
    }
}
