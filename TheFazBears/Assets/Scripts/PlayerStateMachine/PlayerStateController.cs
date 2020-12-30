using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerStateController : MonoBehaviour
{
    public PlayerControls masterInput;

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

    [Header("Foxy Settings", order = 1)]
    public CharacterController characterControllFoxy;
    public Animator foxyAnimator;
    public GameObject foxy;
    public float foxyWalkSpeedy = 4;
    public float foxyRunSpeed = 12;
    public float foxyGravity = -6;
    public float foxyJumpValue;
    public AbilityBase fireBallPower;
    public AbilityBase twirlPower;
    public Transform ledgeCheckX;
    public Transform ledgeCheckY;

    [Header("Shared Variables", order = 2)]
    public CinemachineFreeLook freeLookCam;
    public Transform foxyLookPoint;
    public Transform goldyLookPoint;

    [HideInInspector]
    public AbilityBase currentAimAbility, currentAbility;

    public State currentState;
    [HideInInspector]
    public CharacterController currentController;
    public CharacterController idleController;
    private Vector2 currentDirVelocity = Vector2.zero;
    private Vector2 currentDir = Vector2.zero;

    [HideInInspector]
    public Animator currentAnimator;
    private GameObject currentPlayer;
    [HideInInspector]
    public float walkSpeed, runSpeed, gravity, idleGravity;
    public float turnSmoothTime = 0.2f;
    public float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    [HideInInspector]
    public float speedSmoothVelocity;
    [HideInInspector]
    public float currentSpeed;
    [HideInInspector]
    public float velocityY;

    public AnimationCurve jumpFallOff;
    public float jumpMultiplier;

    [HideInInspector]
    public bool isJumping, running, aiming;
    public bool isGoldy;
    public bool canSwitchChar = true;

    [HideInInspector]
    public int jumpCount = 0;

    [HideInInspector]
    public Transform camTransform;

    [HideInInspector]
    public RaycastHit hitX, hitY;

    [Header("Shared settings", order = 4)]
    public GameObject reticle;
    public CinemachineVirtualCamera aimCam;
    public float mouseSense;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        masterInput = new PlayerControls();
        SetState(new Grounded());

        masterInput.Player.Sprint.performed += Running;
        masterInput.Player.Sprint.canceled += Stopped;
        masterInput.Player.Jump.performed += JumpInput;
        masterInput.Player.Ability.performed += UseAbility;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (canSwitchChar)
            masterInput.Player.SwitchCharacter.performed += SwitchChar;

        LinkCharacter();
    }

    private void OnEnable()
    {
        masterInput.Enable();
    }

    private void OnDisable()
    {
        masterInput.Disable();
    }

    private void FixedUpdate()
    {
        currentState.Update(this);
    }

    public void SetState(State state)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = state;

        if (currentState != null)
        {
            currentState.Enter(this);
        }
    }

    private void LinkCharacter()
    {
        aimCam.gameObject.SetActive(false);
        reticle.SetActive(false);

        if (isGoldy)
        {
            currentAnimator = goldyAnimator;
            currentController = characterControllGoldy;
            idleController = characterControllFoxy;

            walkSpeed = goldyWalkSpeed;
            gravity = goldyGravity;
            idleGravity = foxyGravity;
            runSpeed = goldyRunSpeed;
            currentPlayer = goldy;

            freeLookCam.LookAt = goldyLookPoint;
            freeLookCam.Follow = goldyLookPoint;
            aimCam.Follow = goldyLookPoint;

            currentAbility = electrictyPower;
            currentAimAbility = gravityPower;

        }
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

            freeLookCam.LookAt = foxyLookPoint;
            freeLookCam.Follow = foxyLookPoint;
            aimCam.Follow = foxyLookPoint;


            currentAbility = twirlPower;
            currentAimAbility = fireBallPower;
        }
    }

    public void Move()
    {
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            currentPlayer.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(currentPlayer.transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = currentPlayer.transform.forward * currentSpeed + Vector3.up * velocityY;

        currentController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(currentController.velocity.x, currentController.velocity.z).magnitude;

        if (currentController.isGrounded)
        {
            velocityY = 0;
        }
    }

    private void OtherCharGravity()
    {
        float grav = 0;

        grav += Time.deltaTime * idleGravity;
        Vector3 velocity = Vector3.up * grav;
        while (!idleController.isGrounded)
        {
            idleController.Move(velocity);
        }
    }

    public void AimingMove()
    {
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        inputDir.Normalize();

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        currentDir = Vector2.SmoothDamp(currentDir, inputDir, ref currentDirVelocity, speedSmoothTime);

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = (currentController.transform.forward * currentDir.y + currentController.transform.right * currentDir.x) * targetSpeed + Vector3.up * velocityY;

        currentController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(currentController.velocity.x, currentController.velocity.z).magnitude;

        UpdateMouseLook();
        
        if (currentController.isGrounded)
            velocityY = 0.0f;
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = masterInput.Player.Look.ReadValue<Vector2>();

        currentPlayer.transform.eulerAngles += Vector3.up * targetMouseDelta.x;
    }

    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (currentController.isGrounded)
        {
            return smoothTime;
        }
        return smoothTime;
    }

    private void SwitchChar(InputAction.CallbackContext context)
    {
        isGoldy = !isGoldy;
        LinkCharacter();
        OtherCharGravity();
    }

    private void Running(InputAction.CallbackContext context)
    {
        running = true;
    }

    private void Stopped(InputAction.CallbackContext context)
    {
        running = false;
    }

    private void JumpInput(InputAction.CallbackContext context)
    {
        if (jumpCount <= 1)
        {
            jumpCount++;
            isJumping = true;
            StopCoroutine(JumpEvent());
            velocityY = 0;
            StartCoroutine(JumpEvent());
        }

    }

    private void UseAbility(InputAction.CallbackContext context)
    {
        currentAbility.enabled = true;
    }

    private IEnumerator JumpEvent()
    {
        Debug.Log(currentState);

        if(currentState.ToString() == "Climbing")
        SetState(new Airborn());

        currentController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            currentController.Move((Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime) / jumpCount);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!currentController.isGrounded && currentController.collisionFlags != CollisionFlags.Above);

        currentController.slopeLimit = 45.0f;
        isJumping = false;
       // Debug.Log(isJumping);
    }


}
