using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 distanceToMove;
    public float movementSpeed;
    private Vector3 startingPos;
    private Vector3 endingPos;

    private void Awake()
    {
        startingPos = transform.position;
        endingPos = startingPos - distanceToMove;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        MoveEvent(endingPos);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        MoveEvent(startingPos);
    }

    private IEnumerator MoveEvent(Vector3 postion)
    {
        do
        {
            transform.position = Vector3.MoveTowards(transform.position, postion, movementSpeed);
            yield return null;
        } while (Vector3.Distance(transform.position, postion) >= 1);

    }
}
