using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Grounded : State
{
    private GoldySC privateGoldy;

    public override void Enter(GoldySC goldy)
    {
        Debug.Log("Walk started");
        privateGoldy = goldy;
        goldy.masterInput.Player.Aim.performed += AimPressed;
        goldy.jumpCount = 0;
    }

    public override void Update(GoldySC goldy)
    {
        goldy.Move();
        float animationSpeedPercent = ((goldy.running) ? goldy.currentSpeed / goldy.runSpeed : goldy.currentSpeed / goldy.walkSpeed * .5f);
        goldy.animator.SetFloat("speedPercent", animationSpeedPercent, goldy.speedSmoothTime, Time.deltaTime);

        if(goldy.player.isGrounded == false)
        {
            goldy.SetState(new Airborn());
        }
    }

    public override void Exit(GoldySC goldy)
    {
        goldy.masterInput.Player.Aim.performed -= AimPressed;
    }

    private void AimPressed(InputAction.CallbackContext context)
    {
        privateGoldy.SetState(new Aiming());
    }
}
