using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : InteractionBase
{

    public Vector3 distanceToMove;
    public float movementSpeed;
    private Vector3 startingPos;
    private Vector3 endingPos;
    private Transform parent;

    private void Awake()
    {
        parent = transform.parent;
        startingPos = parent.transform.position;
        endingPos = startingPos - distanceToMove;
       // print(endingPos);
    }

    public override void InteractSucessful()
    {
        base.InteractSucessful();
        StopAllCoroutines();
        StartCoroutine(MoveEvent(endingPos));
    }

    public override void InteractCancel()
    {
        base.InteractCancel();
        StopAllCoroutines();
        StartCoroutine(MoveEvent(startingPos));
    }

    private IEnumerator MoveEvent(Vector3 postion)
    {
        do
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, postion, movementSpeed);
            yield return null;
        } while (Vector3.Distance(parent.transform.position, postion) >= 1);

    }
}
