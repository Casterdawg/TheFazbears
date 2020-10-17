using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Update(GoldySC goldy);

    public virtual void Enter(GoldySC goldy) { }

    public virtual void Exit(GoldySC shareBear) { }
}
