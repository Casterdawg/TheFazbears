using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genorator : InteractionBase
{
    public override void InteractSucessful()
    {
        base.InteractSucessful();
        Debug.Log("Electronic Interaction");
    }

    public override void InteractFail()
    {
        base.InteractFail();
    }
}
