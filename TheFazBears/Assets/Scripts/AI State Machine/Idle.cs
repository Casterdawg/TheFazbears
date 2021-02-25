using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI is idle and looking for the player
public class Idle : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        AI.agent.isStopped = true;
        AI.animator.SetTrigger("Idle");
    }

    public override void Update(FollowAI AI)
    {
        AI.RayCastCheckForPlayers();
    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
        AI.animator.ResetTrigger("Idle");
        AI.audioManager.ScreamHuntsman();
    }
}
