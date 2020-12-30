using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    private void OnEnable()
    {
        AbilityStart();
    }
    public virtual void AbilityStart()
    {
        //Debug.Log("Ability started");
    }

    private void Update()
    {
        AbilityUpdate();
    }

    public virtual void AbilityUpdate()
    {
       // Debug.Log("Update ability");
    }

    private void OnDisable()
    {
        AbilityEnd();
    }

    public virtual void AbilityEnd()
    {
        //Debug.Log("Ability Ended");
    }
}
