using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : InteractionBase
{
    public override void InteractSucessful()
    {
        base.InteractSucessful();
        Debug.Log("Fire interaction");
    }

    public override void InteractFail()
    {
        base.InteractFail();
    }
}
