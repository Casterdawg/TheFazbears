using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public GameObject activatedObject;


    private void OnTriggerEnter(Collider other)
    {
        activatedObject.SetActive(false); 
    }

    private void OnTriggerExit(Collider other)
    {
        activatedObject.SetActive(true);
    }
}
