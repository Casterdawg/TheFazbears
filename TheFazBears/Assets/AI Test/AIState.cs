using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    public abstract void Update(PlayerStateController goldy);

    public virtual void Enter(PlayerStateController goldy) { }

    public virtual void Exit(PlayerStateController shareBear) { }
}
