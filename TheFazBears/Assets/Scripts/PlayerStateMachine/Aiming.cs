using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : State
{
    private GoldySC privateGoldy;
    public override void Enter(GoldySC goldy)
    {
        privateGoldy = goldy;
        Debug.Log("Aim started");
        goldy.masterInput.Player.Aim.canceled += AimCanceled;
        goldy.reticle.SetActive(true);
        goldy.aimCam.SetActive(true);
    }

    public override void Update(GoldySC goldy)
    {
        goldy.AimingMove();

        float animationSpeedPercent = ((goldy.running) ? goldy.currentSpeed / goldy.runSpeed : goldy.currentSpeed / goldy.walkSpeed * .5f);
        goldy.animator.SetFloat("speedPercent", animationSpeedPercent, goldy.speedSmoothTime, Time.deltaTime);

        if (goldy.player.isGrounded == false)
        {
            goldy.SetState(new Airborn());
        }
    }

    public override void Exit(GoldySC goldy)
    {
        goldy.masterInput.Player.Aim.canceled -= AimCanceled;
        goldy.reticle.SetActive(false);
        goldy.aimCam.SetActive(false);
    }

    private void AimCanceled(InputAction.CallbackContext context)
    {
        privateGoldy.SetState(new Grounded());
    }
}
