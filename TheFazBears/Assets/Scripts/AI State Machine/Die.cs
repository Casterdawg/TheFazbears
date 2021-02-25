using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI dies in the game
public class Die : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI has died");

        //start playing death animation
        AI.animator.SetTrigger("Death");
        AI.audioManager.DeathHuntsman();
    }

    public override void Update(FollowAI AI)
    {
        //When the death animation finishes playing, exit the state

        if (AI.animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1)
        {
            Exit(AI);
        }
    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
        AI.Destroy();
    }
}
