using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class will be used as a base for all future AI that will be developed. All the AI will inherit from this script
public class AIBaseBehavior : MonoBehaviour
{
    public AIState currentState;

    //private void FixedUpdate()
    //{
    //    UpdateAI();
    //}

    public void UpdateAI() 
    {
        currentState.Update(this);
    }

    public virtual void OnElectricuted()
    {

    }

    public virtual void OnSmacked()
    {

    }

    public virtual void OnFireBallHit()
    {

    }

    public virtual void ProjectileCollision()
    {

    }


    public virtual void InteractionManager(string action)
    {
        switch (action)
        {
            case "Electricute":

                break;

            //case ""
        }
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
