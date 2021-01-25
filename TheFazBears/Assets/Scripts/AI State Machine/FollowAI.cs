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
    private Rigidbody axeRig;

    private Vector3 forward;
    private readonly float coneRadius = 3;
    private readonly float maxDistance = 10;

    public Transform goldy;
    public Transform foxy;
    private Transform currentTarget;
    public PlayerStateController controller;

    private float distanceToGoldy;
    private float distanceToFoxy;
    private float distanceToTarget;
    private readonly float distanceToAttack = 2;
    private readonly float distanceToIdle = 20;

    private int distaceCount = 0;
    private int timeToIdle = 20;
    private readonly float timeToUpdate = 0.5f;

    private float explosionForce = 10;
    private float explosionRadius = 5;
    private float upwardForce = 5;

    public NavMeshAgent agent;

    private void Awake()
    {
        mask = transform.GetChild(0).gameObject;

        agent = GetComponent<NavMeshAgent>();

        axeRig = axe.GetComponent<Rigidbody>();

        SetState(new Idle());

        StartCoroutine(DelayedUpdate());
    }

    private IEnumerator DelayedUpdate()
    {
        while (true)
        {
            currentState.Update(this);
            yield return new WaitForSeconds(timeToUpdate);
        }
    }

    public void RayCastCheckForPlayers()
    {
        forward = transform.TransformDirection(Vector3.forward);

        if(Physics.SphereCast(transform.position, coneRadius, forward, out RaycastHit hit, maxDistance))
        {
            if(hit.collider.CompareTag("Player"))
            {
                SetState(new Chase());
            }
        }
    }

    public void CheckClosestPlayer()
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

     

    public void CheckDistance()
    {
        distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if(distanceToTarget <= distanceToAttack && currentState.ToString() == "Chase")
        {
            SetState(new Attack());
        }
        else if (distanceToTarget >= distanceToAttack && currentState.ToString() == "Attack")
        {
            SetState(new Chase());
        }
        
        
        if (distanceToTarget >= distanceToIdle)
        {
            distaceCount++;
        }
        else
        {
            distaceCount = 0;
        }

        if(distaceCount >= timeToIdle)
        {
            SetState(new Idle());
        }
        
    }

    public void Move()
    {
        agent.SetDestination(currentTarget.position);
    }

    public void BuzzSawHit()
    {
        Debug.Log("BuzzSaw hit");
        Unmask();
    }

    public void OnExploded()
    {
        Debug.Log("Exploded");
        Unmask();
    }

    private void Unmask()
    {
        if (!unmasked)
        {
            unmasked = true;
            mask.SetActive(false);
            axeRig.constraints = RigidbodyConstraints.None;
            axe.tag = "Throwable";
            axe.transform.parent = null;
            axeRig.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardForce);

            SetState(new Stunned());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Throwable"))
        {
            DeathCheck();
        }
        if (collision.collider.CompareTag("Player"))
        {
            controller.DoDamage(15, collision.collider.gameObject);
        }
    }

    public void OnElectricuted()
    {
        Debug.Log("Electricuted");
        StunCheck();
    }

    public void OnSmacked()
    {
        Debug.Log("Smacked");
        DeathCheck();
    }

    public void OnFireBallHit()
    {
        Debug.Log("Hit by fireball");
        StunCheck();
    }

    private void StunCheck()
    {
        if (unmasked)
        {
            SetState(new Stunned());
        }
    }

    private void DeathCheck()
    {
        if (currentState.ToString() == "Stunned")
        {
            SetState(new Die());
        }
    }

    public void SetState(AIState state)
    {
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
    }

    private void EndStun()
    {
        SetState(new Chase());
    }

    public void Destroy()
    {
        Destroy(axe);
        Destroy(gameObject);
    }
}
