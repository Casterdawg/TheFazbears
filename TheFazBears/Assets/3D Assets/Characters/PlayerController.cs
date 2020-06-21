using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	[Range(0, 1)]
	public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;

	Animator animator;
	Transform cameraT;
	CharacterController controller;


	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;

	private bool isJumping;

	private int jumpCount = 0;

	//public Camera moveCamera;

	//public GravityPower toggle;

	//private bool aiming;

	void Start()
	{
		animator = GetComponent<Animator>();
		cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		//if (Input.GetButton("Fire2"))
		//{
		//	aiming = true;
		//}
		//else if(!Input.GetButton("Fire2"))
		//{
		//	aiming = false;
		//}


		if (controller.isGrounded)
		{
			jumpCount = 0;
		}

		if(this.gameObject.name == "FoxyPants")
		{
			jumpCount = 1;
		}


		// input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey(KeyCode.LeftShift);

		Move(inputDir, running);

		// animator
		float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
		animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

		JumpInput();


	}

	void Move(Vector2 inputDir, bool running)
	{
		if (inputDir != Vector2.zero)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded)
		{
			velocityY = 0;
		}

		

	}

	float GetModifiedSmoothTime(float smoothTime)
	{
		if (controller.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	private void JumpInput()
	{
		if (Input.GetButtonDown("Jump") && !isJumping || Input.GetButtonDown("Jump") && jumpCount <= 1) 
		{
			animator.SetTrigger("hasJumped");
			jumpCount++;
			isJumping = true;
			StopCoroutine(JumpEvent());
			velocityY = 0;
			StartCoroutine(JumpEvent());
		}

	}

	private IEnumerator JumpEvent()
	{
		controller.slopeLimit = 90.0f;
		float timeInAir = 0.0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			controller.Move((Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime) / jumpCount);
			timeInAir += Time.deltaTime;
			yield return null;
		} while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

		controller.slopeLimit = 45.0f;
		isJumping = false;
	}
}