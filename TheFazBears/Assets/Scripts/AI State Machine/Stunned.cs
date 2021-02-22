using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI attacks the player
public class Stunned : AIState
{
    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is stunned");

        AI.Invoke("EndStun", 5);

        AI.animation.SetTrigger("Stun");
        AI.audioManager.StunHuntsman();
    }

    public override void Update(FollowAI AI)
    {

    }

    public override void Exit(FollowAI AI)
    {
        base.Exit(AI);
        Debug.Log("Stun ended");
        AI.animation.ResetTrigger("Stun");
    }
}
