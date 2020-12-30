using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    private void OnEnable()
    {
        AbilityStart();
    }
    public virtual void AbilityStart(){}

    private void Update()
    {
        AbilityUpdate();
    }

    public virtual void AbilityUpdate(){}

    private void OnDisable()
    {
        AbilityEnd();
    }

    public virtual void AbilityEnd(){}
}
