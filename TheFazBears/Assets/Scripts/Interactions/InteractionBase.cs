using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBase : MonoBehaviour
{
    public bool multiInteraction;

    private bool interacted;

    public void OnInteracted()
    {
        if(interacted == false)
        {
            InteractSucessful();
        }
        else if(multiInteraction == true)
        {
            InteractSucessful();
        }
        else
        {
            InteractFail();
        }
    }

    public virtual void InteractSucessful()
    {
        interacted = true;
    }

    public virtual void InteractFail()
    {

    }
}
