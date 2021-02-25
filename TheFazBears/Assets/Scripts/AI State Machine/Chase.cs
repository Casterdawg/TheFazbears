using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI chases the player
public class Chase : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is chasing the player");
        AI.CheckClosestPlayer();
        AI.agent.isStopped = false;
        AI.animator.SetTrigger("Walk");
    }

    public override void Update(FollowAI AI)
    {
        AI.Move();
        AI.CheckDistance();
    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
        AI.animator.ResetTrigger("Walk");
    }
}
