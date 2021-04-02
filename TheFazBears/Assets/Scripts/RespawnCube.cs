using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCube : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = spawnPoint.gameObject.transform.position;
    }
}
