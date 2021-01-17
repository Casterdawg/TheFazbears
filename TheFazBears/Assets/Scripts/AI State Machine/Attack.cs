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
        //start playing attack animation that plays on loop
    }

    public override void Update(FollowAI AI)
    {
        AI.Move();
        AI.CheckDistance();
    }

    public override void Exit(FollowAI AI)
    {
        //stop playing the attack animation and return to the chase state
        base.Exit(AI);
    }
}
