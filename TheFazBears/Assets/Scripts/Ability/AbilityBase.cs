using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    //OnEnable is a built in funciton called when the script or gameobject the script is attached to is enabled.
    //This method will also apply to scripts that inhearit from this script
    private void OnEnable()
    {
        //When the script is enabled, start the ability
        AbilityStart();
    }
    public virtual void AbilityStart(){}

    //Update functions call actions every frame.
    //One reason that I have set up the scripts to be disabled and enabled is to save on performace, so that the update functions aren't constantly called. This saves on CPU useage and RAM.
    private void Update()
    {
        AbilityUpdate();
    }

    public virtual void AbilityUpdate(){}

    //This is like OnEnable, but is called when the script is disabled.
    private void OnDisable()
    {
        AbilityEnd();
    }

    public virtual void AbilityEnd(){}
}
