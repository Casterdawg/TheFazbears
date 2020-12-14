using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    public abstract void Update(GoldySC goldy);

    public virtual void Enter(GoldySC goldy) { }

    public virtual void Exit(GoldySC shareBear) { }
}
