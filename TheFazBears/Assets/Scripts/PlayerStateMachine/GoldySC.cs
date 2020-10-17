using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldySC : MonoBehaviour
{

    public State currentState;

    private void Start()
    {

    }

    private void Update()
    {
        currentState.Update(this);
    }


    public void SetState(State state)
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
