using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class will be used as a base for all future AI that will be developed. All the AI will inherit from this script
public class AIBaseBehavior : MonoBehaviour
{
    public AIState currentState;

    private void FixedUpdate()
    {
        UpdateAI();
    }

    private void UpdateAI() 
    {
        currentState.Update(this);
    }

    public void SetState(AIState state)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = state;

        if (currentState != null)
        {
            currentState.Enter(this);
        }
    }
}
