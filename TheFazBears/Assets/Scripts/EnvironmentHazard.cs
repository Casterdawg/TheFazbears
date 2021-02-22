using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHazard : MonoBehaviour
{
    public PlayerStateController playerManager;
    public float damageVal = 9999;
    private void OnTriggerEnter(Collider other)
    {
        playerManager.DoDamage(damageVal, other.gameObject);
    }
}
