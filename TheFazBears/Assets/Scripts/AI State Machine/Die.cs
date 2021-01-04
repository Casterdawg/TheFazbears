using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI dies in the game
public class Die : AIState
{
    public override void Enter(AIBaseBehavior AI)
    {
        base.Enter(AI);

        Debug.Log("The AI has died");
    }

    public override void Update(AIBaseBehavior AI)
    {

    }

    public override void Exit(AIBaseBehavior AI)
    {
        base.Exit(AI);
    }
}
