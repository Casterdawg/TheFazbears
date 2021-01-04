using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI attacks the player
public class Stunned : AIState
{
    public override void Enter(AIBaseBehavior AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is attacking");
    }

    public override void Update(AIBaseBehavior AI)
    {

    }

    public override void Exit(AIBaseBehavior AI)
    {
        base.Exit(AI);
    }
}
