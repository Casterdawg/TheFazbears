using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will act as the base for the state machine for the AI in the game
public abstract class AIState
{
    public abstract void Update(AIBaseBehavior AI);

    public virtual void Enter(AIBaseBehavior AI) { }

    public virtual void Exit(AIBaseBehavior AI) { }
}
