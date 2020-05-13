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
	public float _CameraDistance = 5f;
	public float rotationSmoothTime = .12f;
	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float yaw;
	float pitch;



	void Start()
	{
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void LateUpdate()
	{
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = currentRotation;

		transform.position = target.position - transform.forward * _CameraDistance;
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

			ScrollAmount *= (this._CameraDistance * 0.3f);

			this._CameraDistance += ScrollAmount * -1f;

			this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 4f);
		}
	}

}

