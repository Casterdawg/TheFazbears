using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{

	public bool lockCursor;
	public float mouseSensitivity = 10;
	public Transform target;
	
	public Vector2 pitchMinMax = new Vector2(-40, 85);
	public float ScrollSensitvity = 2f;
	public float ScrollDampening = 6f;
	public float _CameraDistance = 10f;
	public float rotationSmoothTime = .12f;
	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float yaw;
	float pitch;

	public bool aiming;

	public GameObject aimPoint;

	void Start()
	{

		//aimPoint = new Vector3(.5f, .6f, -1.3f);

		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	private void Update()
	{
		if (Input.GetButton("Fire2"))
		{
			aiming = true;
		}
		else if (!Input.GetButton("Fire2"))
		{
			aiming = false;
		}
	}

	void LateUpdate()
	{
		if (!aiming)
		{
			yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
			pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

			currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
			transform.eulerAngles = currentRotation;

			transform.position = target.position - transform.forward * _CameraDistance;
		}
		else if(aiming)
		{
			//Vector3 localAdjustment = aimPoint - transform.position;

			this.transform.position = Vector3.MoveTowards(transform.position, aimPoint.transform.position, .5f);
			this.transform.eulerAngles = new Vector3(
				Mathf.LerpAngle(transform.eulerAngles.x, 10, 1),
				Mathf.LerpAngle(transform.eulerAngles.y, -5, 1),
				Mathf.LerpAngle(transform.eulerAngles.z, 0, 1));
		}
		//if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		//{
		//	float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

		//	ScrollAmount *= (this._CameraDistance * 0.3f);

		//	this._CameraDistance += ScrollAmount * -1f;

		//	this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 6f);
		//}
	}

}

