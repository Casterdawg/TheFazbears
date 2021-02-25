using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This state will be used when the AI attacks the player
public class Attack : AIState
{

    private RaycastHit hit;
    private Vector3 forward;
    private float attackRange = 0.5f;
    private float damage = 15;

    public override void Enter(FollowAI AI)
    {
        base.Enter(AI);

        Debug.Log("The AI is attacking");
        //start playing attack animation that plays on loop
        AI.animator.SetTrigger("Attack");
    }

    public override void Update(FollowAI AI)
    {
        AI.Move();
        AI.CheckDistance();
        //set up if statement checking the animation frame that deals damage
        forward = AI.transform.TransformDirection(Vector3.forward);

       // Debug.DrawRay(AI.transform.position, forward * attackRange, Color.green, 1);

        if (Physics.SphereCast(AI.transform.position, attackRange, forward, out RaycastHit hit, attackRange))
        {
            //Debug.Log(hit);
            if (hit.collider.CompareTag("Player"))
            {
                AI.controller.DoDamage(damage, hit.collider.gameObject);
                Debug.Log("Attacked");
            }
        }
    }

    public override void Exit(FollowAI AI)
    {
        //stop playing the attack animation and return to the chase state
        base.Exit(AI);
        AI.animator.ResetTrigger("Attack");
    }
}
