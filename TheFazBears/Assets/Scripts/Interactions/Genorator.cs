using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genorator : InteractionBase
{
    public InteractionBase interactedObject;
    public override void InteractSucessful()
    {
        base.InteractSucessful();
        Debug.Log("Electronic Interaction");
        interactedObject.OnInteracted();
    }
}
