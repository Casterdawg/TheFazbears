using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI is idle and looking for the player
public class Idle : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is idle");
    }

    public override void Update(FollowAI AI)
    {

    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
    }
}
