using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>
/// This will be by far the most complicated class to explain. Please let me know if you have questions about it after reviewing my comments.
/// A lot of these variables will become private in the future, but for now while we are designing mechanics, they are mostly public to be easier to work with between classes.
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    //Variable used for player input actions
    public PlayerControls masterInput;

    //All of these variables are specific to goldy, with his hown gravity, animator, jump limit and powers.
    //Further explination of the variables that both characters share will be below.
    [Header("Goldy Settings", order = 0)]
    public CharacterController characterControllGoldy;
    public Animator goldyAnimator;
    public GameObject goldy;
    public float goldyWalkSpeed = 2;
    public float goldyRunSpeed = 6;
    public float goldyGravity = -12;
    public float goldyJumpValue;
    public int goldyJumpLimit;
    public AbilityBase gravityPower;
    public AbilityBase electrictyPower;
    public float electricCoolDown = 1.5f;
    public bool goldyCanElectricute = true;
    public float currentGoldyHealth = 100;
    public GameObject goldyHealthBar;
    private float totalGoldyHealth;

    //Variables specific for when using foxy
    [Header("Foxy Settings", order = 1)]
    public CharacterController characterControllFoxy;
    public Animator foxyAnimator;
    public GameObject foxy;
    public float foxyWalkSpeedy = 4;
    public float foxyRunSpeed = 12;
    public float foxyGravity = -6;
    public float foxyJumpValue;
    public int foxyJumpLimit;
    public AbilityBase fireBallPower;
    public AbilityBase twirlPower;
    public Transform ledgeCheckX;
    public Transform ledgeCheckY;
    public float fireBallCoolDown = 1f;
    public float smackCoolDown = 2f;
    public bool foxyCanFireball = true;
    public bool foxyCanSmack = true;
    public float currentFoxyHealth = 150;
    public GameObject foxyHealthBar;
    private float totalFoxyHealth;

    //All of the variables below will be shared by both characters and will change depending on which character is active at the time
    [Header("Shared Variables", order = 2)]
    //The freelook camera that is usedfor orbiting around the active player
    public CinemachineFreeLook freeLookCam;
    //The look point for the camera when foxy is the active character
    public Transform foxyLookPoint;
    //The look point for when goldy is the active character
    public Transform goldyLookPoint;
    //The variable that will be populated with the current look point of the player
    public Transform currentLookPoint;
    private HealthBar changedHealthBar;

    //HideInInspector prevents public variables from being visable in the inspector, makes the inspector easier to navigate while keeping the variables easy to work with.
    //These two variables are populated with the abilities that foxy and goldy both have, they will switch when the characters switch.
    [HideInInspector]
    public AbilityBase currentAimAbility, currentAbility;

    //Connect to the state controller for both characters
    public State currentState;
    //Current controller is the controller that is actively being used by the player
    //Idle controller is the other controller, this variable is used to make sure it has gravity when switching characters.
    [HideInInspector]
    public CharacterController currentController;
    public CharacterController idleController;
    //These variables are used when using the aimed movement, checking to see what direction the player is facing and the current directional velocity of the player
    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector2 currentDir = Vector2.zero;

    //Current animator is the animator of the active character. When we set up the player, we preferably would want to make sure that the triggers and states are the same for the animator to make animating both characters easier.
    [HideInInspector]
    public Animator currentAnimator;
    //Current player will reference the gameobject of the active player, is used for particular parts of movement
    private GameObject currentPlayer;
    //These variables are changed when switching characters, they relate to character movement and gravity.
    [HideInInspector]
    public float walkSpeed, runSpeed, gravity, idleGravity;
    //Variable used for making the characters turn more naturly and not instantly turning around.
    public float turnSmoothTime = 0.2f;
    //The velocity of the turning motion
    public float turnSmoothVelocity;
    //Used to provide smooth motion when changing speed
    public float speedSmoothTime = 0.1f;
    //Velocity of speed smoothing
    [HideInInspector]
    public float speedSmoothVelocity;
    //The current speed of the active character
    [HideInInspector]
    public float currentSpeed;
    //The y velocity used when applying gravity to the character
    [HideInInspector]
    public float velocityY;
    public int midAirjumpLimit;

    //This animation curve is used to have the character start the jump faster at the start and gradually slow down as they reach the apex of their jump
    public AnimationCurve jumpFallOff;
    //A number that will determine how strong a characters jump is.
    public float jumpMultiplier;

    //These bools are used to check to see if you are a particular character, you can switch characters and if the character is performing various actions
    [HideInInspector]
    public bool isJumping, running, aiming;
    public bool isGoldy;
    public bool canSwitchChar = true;

    //The amount of jumps a character has done (one is done on the ground and then the other is midair)
    [HideInInspector]
    public int jumpCount = 0;

    //Getting the position of the playerCamera
    [HideInInspector]
    public Transform camTransform;

    //Raycast data obtained from ledge detection for foxy
    [HideInInspector]
    public RaycastHit hitX, hitY;

    [Header("Shared settings", order = 4)]
    //The reticle that is toggled when aiming
    public GameObject reticle;
    //The aim camera that is toggled when aiming
    public CinemachineVirtualCamera aimCam;
    //The mouse sensitivity multiplier for the y axis change
    public float mouseSense;

    //When the class starts, perform the following actions
    private void Awake()
    {
        totalFoxyHealth = currentFoxyHealth;
        totalGoldyHealth = currentGoldyHealth;


        //Populate variables for the camera, the input manager, and setting the default state of the character
        camTransform = Camera.main.transform;
        masterInput = new PlayerControls();
        SetState(new Grounded());

        //Input listeners for various actions in the game
        masterInput.Player.Sprint.performed += Running;
        masterInput.Player.Sprint.canceled += Stopped;
        masterInput.Player.Jump.performed += JumpInput;
        masterInput.Player.Ability.started += UseAbility;

        //Lock the cursor so that camera movement feels better and set the cursor to be invisable to remove screen clutter
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //If the player can switch characters, then add an input action for switching characters
        if (canSwitchChar)
            masterInput.Player.SwitchCharacter.performed += SwitchChar;
        //Link the variables for the active character
        LinkCharacter();
    }

    //These two methods are used to ensure that if the player controller is enabled and disabled, the input manaer is enabled/disabled respectively
    //This could be used well to disable input durring a pause screen
    private void OnEnable()
    {
        masterInput.Enable();
    }

    private void OnDisable()
    {
        masterInput.Disable();
    }

    //FixedUpdate is similar to update, but it is frame indipendant. This will ensure that users who experiance different framerate to have consistant movement mechanics
    //This update will enable the update function in the state machine. Like the airborn, grounded, and climbing state
    private void FixedUpdate()
    {
        currentState.Update(this);
    }

    public void DoDamage(float damageValue, GameObject currentChar)
    {
        if(currentChar == foxy)
        {
            currentFoxyHealth -= damageValue;
            print(currentFoxyHealth);
            changedHealthBar.UpdateValue(currentFoxyHealth, totalFoxyHealth);
        }
        else if(currentChar == goldy)
        {
            currentGoldyHealth -= damageValue;
            print(currentGoldyHealth);
            changedHealthBar.UpdateValue(currentGoldyHealth, totalGoldyHealth);
        }


        if(currentFoxyHealth <= 0 || currentGoldyHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        print("Game over");
    }

    //This method is used for changin the current state of the character
    public void SetState(State state)
    {
        //If the current state is not null. then exit the current state the player is in
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        //Set the current state you are going to switch to
        currentState = state;

        //With the newly populated state, enter that new state
        if (currentState != null)
        {
            currentState.Enter(this);
        }
    }

    //This method is used for seamless character switching. This is where those foxy and goldy variables come into play
    private void LinkCharacter()
    {
        //Make sure the aim camera and reticle is disabled when switching characters
        aimCam.gameObject.SetActive(false);
        reticle.SetActive(false);

        //If the current player is goldy, populate the shared vairables with those that goldy uses
        if (isGoldy)
        {
            //Sets the controller, animator, and idleController when switching
            currentAnimator = goldyAnimator;
            currentController = characterControllGoldy;
            idleController = characterControllFoxy;

            //These variables are all utilized for movement of the current character and for setting the gravity of the idle character
            walkSpeed = goldyWalkSpeed;
            gravity = goldyGravity;
            idleGravity = foxyGravity;
            runSpeed = goldyRunSpeed;
            currentPlayer = goldy;
            midAirjumpLimit = goldyJumpLimit;
            jumpMultiplier = goldyJumpValue;

            //This group of variables is utilized for the camera switching between the different characters
            freeLookCam.LookAt = goldyLookPoint;
            freeLookCam.Follow = goldyLookPoint;
            aimCam.Follow = goldyLookPoint;
            currentLookPoint = goldyLookPoint;

            //These variables are for setting the current abilities the player can use
            currentAbility = electrictyPower;
            currentAimAbility = gravityPower;

            goldyHealthBar.SetActive(true);
            foxyHealthBar.SetActive(false);
            changedHealthBar = goldyHealthBar.GetComponent<HealthBar>();

        }
        //If the current character is foxy, then switch the variables accordingly.
        else
        {
            currentAnimator = foxyAnimator;
            currentController = characterControllFoxy;
            idleController = characterControllGoldy;

            walkSpeed = foxyWalkSpeedy;
            gravity = foxyGravity;
            idleGravity = goldyGravity;
            runSpeed = foxyRunSpeed;
            currentPlayer = foxy;
            midAirjumpLimit = foxyJumpLimit;
            jumpMultiplier = foxyJumpValue;

            freeLookCam.LookAt = foxyLookPoint;
            freeLookCam.Follow = foxyLookPoint;
            aimCam.Follow = foxyLookPoint;
            currentLookPoint = foxyLookPoint;


            currentAbility = twirlPower;
            currentAimAbility = fireBallPower;

            goldyHealthBar.SetActive(false);
            foxyHealthBar.SetActive(true);
            changedHealthBar = foxyHealthBar.GetComponent<HealthBar>();
        }
    }

    //This is the base movement the player has when in the grounded and airborn state
    public void Move()
    {
        //Get the input direction of the character using the input manager
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        //if the input direction is not zero, then rotate the player depending on the values provided by the player
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            currentPlayer.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(currentPlayer.transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }
        
        //The speed of the player that is modified by if the player is running or not. Use the running speed of you are running, if not, then use walk speed.
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        //Smooth out the movement of the speed change
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        //Gravity applied to the player
        velocityY += Time.deltaTime * gravity;

        //The velocity of the player is the combination of the current speed from input and gravity
        Vector3 velocity = currentPlayer.transform.forward * currentSpeed + Vector3.up * velocityY;

        //Move the player based on the variables above
        currentController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(currentController.velocity.x, currentController.velocity.z).magnitude;

        //If the current controller is grounded, then reset the gravity of the player
        if (currentController.isGrounded)
        {
            velocityY = 0;
        }
    }

    //Play this method once when you switch characters
    private void OtherCharGravity()
    {
        float grav = 0;
        //Gravity of the idleCharacter
        grav += Time.deltaTime * idleGravity;
        Vector3 velocity = Vector3.up * grav;

        //A loop that will be performed while the idle character is not grounded.
        //Move the character while it is not grounded.
        while (!idleController.isGrounded)
        {
            idleController.Move(velocity);
        }
    }

    //Movement used for when the player is in the aiming state
    public void AimingMove()
    {
        //Utilized the same value from the input manager
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        //Normalize the values of the input manager. This is to make sure you don't move faster while moving diagonally.
        inputDir.Normalize();

        //The same targetSpeed used in the other movement fucntion
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        //These two variables are specific to this controller. These two variables ensure that movement is based on what direction the character is facing.
        currentDir = Vector2.SmoothDamp(currentDir, inputDir, ref currentDirVelocity, speedSmoothTime);

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        //Mostly the same as the other movement, but take into account the forward vector of the player to make sure movement feels good while aiming.
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = (currentController.transform.forward * currentDir.y + currentController.transform.right * currentDir.x) * targetSpeed + Vector3.up * velocityY;

        currentController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(currentController.velocity.x, currentController.velocity.z).magnitude;

        //Use custom mouse look while aiming
        UpdateMouseLook();

        if (currentController.isGrounded)
            velocityY = 0.0f;
    }

    private void UpdateMouseLook()
    {
        //Get the deta change of how the mouse moves across the screen
        Vector2 targetMouseDelta = masterInput.Player.Look.ReadValue<Vector2>();

        //Rotate the lookpoint of the active character based on the mouse sense and the mouse delta on the y axis
        currentLookPoint.transform.rotation *= Quaternion.AngleAxis(-targetMouseDelta.y * mouseSense, Vector3.right);

        //Rotate the current character based on the x value of the mouse delta
        currentPlayer.transform.eulerAngles += Vector3.up * targetMouseDelta.x;

        //Get the current angle value of lookpoint after it has been moved
        Vector3 angles = currentLookPoint.transform.localEulerAngles;
        //Reset the z value of the angles variable
        angles.z = 0;

        //Get the x angle from the currentLookPoint
        float angle = currentLookPoint.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation to prevent the player looking 360 degrees, this looking upside down and glitching the camera
        if (angle > 180 && angle < 280)
        {
            //If the if statement condition is met, then prevent the lookPoint from moving beyond the range
            angles.x = 280;
        }
        else if (angle < 180 && angle > 80)
        {
            angles.x = 80;
        }

        //Set the position of the lookpoint with the clamped values applied
        currentLookPoint.transform.localEulerAngles = angles;

    }
    
    //Get the smooth turning value. This is built to where the smooth time can be different based on if you are in the air currently. Could be removed in the future if smove turning is not different while airborn.
    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (currentController.isGrounded)
        {
            return smoothTime;
        }
        return smoothTime;
    }

    //The method called when the switch character button is pressed.
    private void SwitchChar(InputAction.CallbackContext context)
    {
        //Switch which character is currently active
        isGoldy = !isGoldy;
        //Relink the character after the active character is changed
        LinkCharacter();
        //Apply gravity to the other character that is not active
        OtherCharGravity();
    }

    //If the player pressed down on the shift key, set the running bool to true
    private void Running(InputAction.CallbackContext context)
    {
        running = true;
    }

    //When the player lets go of the shift key, set the running bool to false
    private void Stopped(InputAction.CallbackContext context)
    {
        running = false;
    }

    //When the player pressed space, then check to see if the player can jump.
    private void JumpInput(InputAction.CallbackContext context)
    {
        //If you can jump, stop the current Jump event, set is jumping to true, reset y velcity and restart the jumpEvent
        if (jumpCount <= midAirjumpLimit)
        {
            jumpCount++;
            isJumping = true;
            StopCoroutine(JumpEvent());
            velocityY = 0;
            StartCoroutine(JumpEvent());
        }

    }

    //When the player uses the ability button, then use the non aimed ability used by the specific character
    private void UseAbility(InputAction.CallbackContext context)
    {
        IEnumerator coroutine;
        if (currentController == characterControllFoxy && foxyCanSmack)
        {
            foxyCanSmack = false;
            coroutine = BoolToggle("smack", smackCoolDown);
        }
        else if (currentController == characterControllGoldy && goldyCanElectricute)
        {
            goldyCanElectricute = false;
            coroutine = BoolToggle("electricute", electricCoolDown);
        }
        else
        {
            return;
        }
        StartCoroutine(coroutine);
        currentAbility.enabled = true;
    }

    public IEnumerator BoolToggle(string ability, float time)
    {
       yield return new WaitForSeconds(time);

        switch (ability)
        {
            case "smack":
                foxyCanSmack = true;
            break;

            case "electricute":
                goldyCanElectricute = true;
            break;

            case "fireBall":
                foxyCanFireball = true;
            break;
        }
       print("Cooldown ended");
    }


   // private void AbilityCoolDownBool

    //Function called when the player jump
    private IEnumerator JumpEvent()
    {
        //If the current state of the player is climbing, then switch the state to Airborn when the player jumps
        if (currentState.ToString() == "Climbing")
            SetState(new Airborn());

        //Adjust the slope limit of the character so that they can navigate steep terrain more easily while jumping
        currentController.slopeLimit = 90.0f;
        //Reset the time spent in the air
        float timeInAir = 0.0f;

        //While the player is not grounded and isn't colliding with any object above them, raise the player based on various multiplies set beforehand.
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            currentController.Move((Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime) / jumpCount);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!currentController.isGrounded && currentController.collisionFlags != CollisionFlags.Above);

        //When the jump is over, reset the slopelimit of the character back to default so they can't walk up walls
        currentController.slopeLimit = 45.0f;

        //Set the jump bool back to false
        isJumping = false;
    }
}
