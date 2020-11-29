using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborn : State
{
    public override void Enter(GoldySC goldy)
    {
        Debug.Log("fall started");
        goldy.animator.SetTrigger("Jump");
        goldy.jumpCount = 1;
    }

    public override void Update(GoldySC goldy)
    {
        goldy.Move();
        if(goldy.player.isGrounded == true)
        {
            goldy.SetState(new Grounded());
        }
    }

    public override void Exit(GoldySC goldy)
    {
        goldy.animator.SetTrigger("Grounded");
    }
}
