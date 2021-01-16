using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//This class will be used as a base for all future AI that will be developed. All the AI will inherit from this script
public class FollowAI : MonoBehaviour
{

    public AIState currentState;

    private bool unmasked = false;

    private GameObject mask;

    public GameObject axe;

    private Ray playerDetectionCone;


    public Transform goldy;
    public Transform foxy;
    private Transform currentTarget;

    private float distanceToGoldy;
    private float distanceToFoxy;

    private NavMeshAgent agent;

    private void Awake()
    {
        mask = transform.GetChild(0).gameObject;

        agent = GetComponent<NavMeshAgent>();

        SetState(new Idle());
    }

    private void FixedUpdate()
    {
        currentState.Update(this);
    }

    public void CheckPlayers()
    {
        distanceToFoxy = Vector3.Distance(foxy.position, transform.position);

        distanceToGoldy = Vector3.Distance(goldy.position, transform.position);

        if (distanceToGoldy > distanceToFoxy)
        {
            currentTarget = foxy;
        }
        else
        {
            currentTarget = goldy;
        }
    }

    public void Move()
    {

        agent.SetDestination(currentTarget.position);
    }

    public void BuzzSawHit()
    {
        Debug.Log("BuzzSaw hit");
        unmasked = true;
        mask.SetActive(false);

        SetState(new Stunned());
    }

    public void OnExploded()
    {
        Debug.Log("Exploded");
        unmasked = true;
        mask.SetActive(false);

        SetState(new Stunned());
    }

    public void OnElectricuted()
    {
        Debug.Log("Electricuted");
        if (unmasked)
        {
            SetState(new Stunned());
        }
    }

    public void OnSmacked()
    {
        Debug.Log("Smacked");

        if (currentState.ToString() == "Stunned")
        {
            SetState(new Die());
        }
    }

    public void OnFireBallHit()
    {
        Debug.Log("Hit by fireball");

        if (unmasked)
        {
            SetState(new Stunned());
        }

    }

    public void SetState(AIState state)
    {

        //if (currentState.ToString() != "Die")
        //{
        if (currentState != null)
        {
            currentState.Exit(this);

            if (currentState.ToString() == "Die")
            {
                return;
            }

        }
        currentState = state;

        if (currentState != null)
        {
            currentState.Enter(this);
        }
        //}

       // Debug.Log(currentState.ToString());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
