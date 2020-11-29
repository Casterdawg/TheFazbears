using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoldySC : MonoBehaviour
{
    public PlayerControls masterInput;
    public CharacterController player;
    public State currentState;
    public Animator animator;

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    public float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    public float speedSmoothVelocity;
    public float currentSpeed;
    public float velocityY;

    public AnimationCurve jumpFallOff;
    public float jumpMultiplier;

    [HideInInspector]
    public bool isJumping, running, aiming;

    [HideInInspector]
    public int jumpCount = 0;

    [HideInInspector]
    public Transform camTransform;

    public GameObject reticle;
    public GameObject aimCam;

    public float mouseSense;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        masterInput = new PlayerControls();
        SetState(new Grounded());

        masterInput.Player.Sprint.performed += Running;
        masterInput.Player.Sprint.canceled += Stopped;
        masterInput.Player.Jump.performed += JumpInput;
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

    public void Move()
    {
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        player.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(player.velocity.x, player.velocity.z).magnitude;

        if (player.isGrounded)
        {
            velocityY = 0;
        }
    }

    public void AimingMove()
    {
        /*
        Vector2 inputDir = masterInput.Player.Movement.ReadValue<Vector2>();
        Vector2 camDir = masterInput.Player.Look.ReadValue<Vector2>();
        Vector2 currentDir = Vector2.zero;
        inputDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, inputDir, ref currentSpeed, GetModifiedSmoothTime(speedSmoothTime);

        if (player.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        player.Move(velocity * Time.deltaTime);
        */
    }

    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (player.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
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

    private IEnumerator JumpEvent()
    {
        player.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            player.Move((Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime) / jumpCount);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!player.isGrounded && player.collisionFlags != CollisionFlags.Above);

        player.slopeLimit = 45.0f;
        isJumping = false;
    }
}
