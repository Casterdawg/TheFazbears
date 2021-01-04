using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI is idle and looking for the player
public class Idle : AIState
{
    public override void Enter(AIBaseBehavior AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is idle");
    }

    public override void Update(AIBaseBehavior AI)
    {

    }

    public override void Exit(AIBaseBehavior AI)
    {
        base.Exit(AI);
    }
}
