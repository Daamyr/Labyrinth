using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

//	public GameObject target;
//
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		transform.LookAt (target.transform);
//	}

	public Transform playerBody;
	public float mouseSensitivity = 2.0f;
	private float xAxisClamp = 0.0f;

	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Start ()
	{
		transform.position = playerBody.position;
	}

	void FixedUpdate () {
		doRotation ();
	}

	void doRotation(){
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		float rotAmountX = mouseX * mouseSensitivity;
		float rotAmountY = mouseY * mouseSensitivity;

//		xAxisClamp -= rotAmountY;

		Vector3 targetRotCam = transform.rotation.eulerAngles;
		Vector3 targetRotBody = playerBody.rotation.eulerAngles;

		targetRotCam.x -= rotAmountY;
//		targetRotCam.z = 0;
		targetRotBody.y += rotAmountX;

//		if(xAxisClamp > 90)
//		{
//			xAxisClamp = 90;
//			targetRotCam.x = 90;
//		}
//		else if(xAxisClamp < -90)
//		{
//			xAxisClamp = -90;
//			targetRotCam.x = 270;
//		}

//		print(mouseY);


		//transform.rotation = Quaternion.Euler(targetRotCam);
		transform.Rotate (0, rotAmountX, 0);
		transform.Rotate (rotAmountY, 0, 0);
		playerBody.rotation = Quaternion.Euler(targetRotBody);
	}
}
