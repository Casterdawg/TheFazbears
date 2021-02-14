using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//We inherit from the other script when we put the name of it right next to the name of this script.
//Since the script we are inheriting from is a monobehavior, this script will inherit that trait too.
//Monobehavior allows us to attach the scripts onto gameobjects and makes it easy for us to change public variables in the inspector instead of solely though code.
public class GravPower : AbilityBase
{
    //The float point is where the gravity object will float when you pick it up
    public Transform floatPoint;
    //This determines how fast you will shoot the gravity object
    public float launchSpeed;
    //This will determine how far you can pick up the gravity object
    public float abilityRange;
    //Establish connection to the playercontroller, this is used for enabling/disabling input listeners
    public PlayerStateController player;
    //The playercamers current position, used to determine what direction you shoot the gravity object
    public Transform camPos;

    public Camera cam;

    //The gravity object being effected
    private GameObject target;
    //The rigidbody of the target
    private Rigidbody targetRig;
    //The racast data used to check to see if the user is aiming at the gravity object to pick it up
    private RaycastHit hit;
    //The forward vector used to ensure that the raycast detects the object in the right direction
    private Vector3 forward;

    public AudioSource audioSource;
    public AudioClip pickUpSound;
    public AudioClip throwSound;


    public override void AbilityStart()
    {
        base.AbilityStart();
        //This invoke was used to prevent a bug where ShootObject and AbilityUsed were used on the same frame. When you set a delay, then you ensure the actions aren't performed on the same frame.
        //This line starts the GravSetUp() function after a tenth of a second.
        Invoke("GravSetUp", .1f);

    }

    //This method is called when the player first uses the gravity ability
    private void GravSetUp()
    {
        //Obtain the forward vector for that frame to get the direction the raycast will go.
        forward = camPos.TransformDirection(Vector3.forward);

        //Debug.DrawRay(cam.transform.position, forward * abilityRange, Color.red, 1000);

        //Test to see if the raycast hit any object while also testing to see if the object that was hit has the propper tag
        if (Physics.Raycast(camPos.transform.position, forward, out hit, abilityRange)
            && hit.transform.CompareTag("Throwable"))
        {
            //If the gravity object can be interacted with, then activate the interaction
            if (hit.collider.TryGetComponent(out InteractionBase interaction))
            {
                interaction.OnInteracted();
                Debug.Log("Interactable found");
                hit.rigidbody.useGravity = false;
            }

            //If all of these things return true in the if statement, then populate variables for future use, reset the objects velocity, make sure the object isn't impacted by gravity anymore.
            target = hit.transform.gameObject;
            targetRig = target.GetComponent<Rigidbody>();
            targetRig.useGravity = false;
            targetRig.velocity = Vector3.zero;

            //This line adds an event listener for when the player presses left click, it will call the shoot object function
            player.masterInput.Player.AimAbility.started += ShootObject;

            audioSource.clip = pickUpSound;
            audioSource.Play();
        }
        else
        {
            //If the raycast test returns false or the tag of the object is not throwable, then disable this script
            enabled = false;
        }
    }

    public override void AbilityUpdate()
    {
            base.AbilityUpdate();
        //If the target isn't null, then suspend the object at the float point
        if (target != null)
            target.transform.position = Vector3.MoveTowards(target.transform.position, floatPoint.position, 0.3f);
    }

    public override void AbilityEnd()
    {
        base.AbilityEnd();
        //When the script is disabled, ensure that the event listener is off to save on performance and prevent issues where multiple actions are called accidentilly.
        player.masterInput.Player.AimAbility.started -= ShootObject;
        //If the target isn't null, then enable gravity on it again and set the variable to null again for future use.
        if (target != null)
        {
            targetRig.useGravity = true;
            target = null;
        }

    }

    
    private void Throw()
    {
        //As long as the target rigidbody isn't null, then add force to the object in the direction you are facing to throw it.
        if (targetRig != null)
        {
            targetRig.AddForce(floatPoint.transform.forward * launchSpeed, ForceMode.Impulse);
            audioSource.clip = throwSound;
            audioSource.Play();
        }
    }

    private void ShootObject(InputAction.CallbackContext context)
    {
        //When the user pressed left click, throw the object and disable this script
        Throw();
        enabled = false;
    }
}
