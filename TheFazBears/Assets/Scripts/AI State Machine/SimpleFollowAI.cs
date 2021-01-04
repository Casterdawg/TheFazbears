﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This will be an AI that follows the player as it's primary behavior
/// </summary>
public class SimpleFollowAI : AIBaseBehavior
{
    public Transform goldyPosition;
    public Transform foxyPosition;


    private RaycastHit hit;


    public bool unMasked = false;

    private void Start()
    {
        SetState(new Idle());
    }


   public void Electricuted()
    {

    }


}
