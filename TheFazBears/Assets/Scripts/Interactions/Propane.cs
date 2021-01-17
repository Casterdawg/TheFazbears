using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propane : MonoBehaviour
{
    private Rigidbody rig;

    private readonly float neededSpeed = 2f;

    private readonly float explosionRadius = 4;
    private readonly float explosionPower = 300;
    private readonly float upwardForce = 30;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (rig.velocity.magnitude >= neededSpeed)
        {
            Debug.Log("Exploded");
            Explode();
        }
    }

    private void Explode()
    {
        Vector3 pos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out Rigidbody rig))
            {
                rig.AddExplosionForce(explosionPower, pos, explosionRadius, upwardForce);
            }
            if (hit.TryGetComponent(out InteractionBase interaction))
            {
                if (interaction.name == "Lock")
                {
                    Debug.Log("Lock found");
                    interaction.OnInteracted();
                }
            }

            if (hit.TryGetComponent(out FollowAI AI))
            {
                this.tag = "Untagged";
                AI.OnExploded();
            }
        }

        Destroy(this.gameObject);
    }
}
