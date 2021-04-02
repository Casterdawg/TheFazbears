using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : InteractionBase
{
    public InteractionBase interactedObject;

    public override void InteractSucessful()
    {
        base.InteractSucessful();
        Debug.Log("Fire interaction");
        interactedObject.OnInteracted();
    }
}
