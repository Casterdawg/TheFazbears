using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBase : MonoBehaviour
{
    public bool multiInteraction;

    private bool interacted;

    public bool interactionCancelable;

    private void Start()
    {
        interacted = false;
    }

    public void OnInteracted()
    {
        if(interacted == false)
        {
            InteractSucessful();
        }
        else if(multiInteraction == true && interacted == true)
        {
            InteractSucessful();
        }
    }

    public void OnInteractCancel()
    {
        if(interacted == true && interactionCancelable == true)
        {
            OnInteractCancel();
        }
    }

    public virtual void InteractSucessful()
    {
        interacted = true;
    }

    public virtual void InteractCancel()
    {
        interacted = false;
    }
}
