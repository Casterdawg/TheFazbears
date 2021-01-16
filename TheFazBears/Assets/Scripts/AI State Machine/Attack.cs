using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI attacks the player
public class Attack : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is attacking");
    }

    public override void Update(FollowAI AI)
    {
        AI.Move();
    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
    }
}
