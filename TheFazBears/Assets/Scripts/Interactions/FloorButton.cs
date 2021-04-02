using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{

    public InteractionBase interactionController;
    public Material interactedColor;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        interactionController.OnInteracted();
        rend.material = interactedColor;
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    interactionController.OnInteractCancel();
    //}
}
