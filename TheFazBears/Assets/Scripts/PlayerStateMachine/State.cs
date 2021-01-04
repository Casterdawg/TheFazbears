using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the basis of the the player state machine. It is pretty much the same as the AI state class, with the main difference being that the methods of this class accepting the player state controller instead
/// </summary>
public abstract class State
{
    public abstract void Update(PlayerStateController player);

    public virtual void Enter(PlayerStateController player) { }

    public virtual void Exit(PlayerStateController player) { }
}
