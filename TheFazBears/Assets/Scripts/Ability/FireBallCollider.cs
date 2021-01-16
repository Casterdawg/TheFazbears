using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    //How long the fireball will last in seconds when it doesn't collide with anything
    public float fireBallLifeTime;


    //When the fireball first starts up, set up the invoke method for destorying it if it doesn't collide with anything
    void Start()
    {
        Invoke("Destroy", fireBallLifeTime);
    }

    //If the fireball collides with something, check the tags of the object it collides with to perform actions.
    private void OnCollisionEnter(Collision collision)
    {
        //This if statement will be used to check to see if the fireball has interacted with an enemy
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    Debug.Log("Fireball hit enemy");
        //}
        if(collision.gameObject.TryGetComponent(out FollowAI AI))
        {
            AI.OnFireBallHit();
        }
        //This if statement will check to see if the fireball has interated with an interactable object
        if (collision.gameObject.CompareTag("FireInteractable"))
        {
            collision.gameObject.GetComponent<InteractionBase>().OnInteracted();
        }

        //Cancel the invoked method mentioned earlier since it is no longer needed
        CancelInvoke();

        //Call the destory method manually
        Destroy();
    }

    //This method will remove the object from the scene
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
