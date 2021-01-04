using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : State
{
    //This variable is used so that the playercontroller can be accessed in the non override methods
    private PlayerStateController privateController;

    //When the player enters the aiming state, set up event listeners for when the user lets go of the aiming button and uses aimed abilities
    public override void Enter(PlayerStateController controller)
    {
        privateController = controller;
        controller.masterInput.Player.Aim.canceled += AimCanceled;
        controller.masterInput.Player.AimAbility.started += AbilityUsed;
        //Turn on the reticle to help the player aim
        controller.reticle.SetActive(true);
        //Enable the aim camera so it will take priority over the freelook camera
        controller.aimCam.gameObject.SetActive(true);

    }

    //While in the aim state, have the player use a different movement to make shooting and aiming easier, play walking and running animations and check to see if the player is on the ground.
    public override void Update(PlayerStateController controller)
    {
        controller.AimingMove();

        float animationSpeedPercent = ((controller.running) ? controller.currentSpeed / controller.runSpeed : controller.currentSpeed / controller.walkSpeed * .5f);
        controller.currentAnimator.SetFloat("speedPercent", animationSpeedPercent, controller.speedSmoothTime, Time.deltaTime);

        //If if the player is not on the ground, exit the aiming state and move to the airborn state
        if (controller.currentController.isGrounded == false)
        {
            controller.SetState(new Airborn());
        }
    }

    //When the player leaves the aiming state, disable event listeners that were started earlier, turn off the reticle, turn off the aim camera and reset the lookpoint to default rotation.
    public override void Exit(PlayerStateController controller)
    {
        controller.masterInput.Player.Aim.canceled -= AimCanceled;
        controller.masterInput.Player.AimAbility.started -= AbilityUsed;
        controller.reticle.SetActive(false);
        controller.aimCam.gameObject.SetActive(false);
        controller.currentLookPoint.transform.localRotation = Quaternion.identity;
    }

    //When the user uses an ability, then enable the current aimed ability.
    private void AbilityUsed(InputAction.CallbackContext context)
    {
        privateController.currentAimAbility.enabled = true;
    }

    //When the user cancels aiming, then disable the current aimability and set the state back to aimed
    private void AimCanceled(InputAction.CallbackContext context)
    {
        privateController.currentAimAbility.enabled = false;
        privateController.SetState(new Grounded());
    }
}
