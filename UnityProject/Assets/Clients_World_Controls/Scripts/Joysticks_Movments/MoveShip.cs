using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour {


	public VirturalJoyStick joyStick;
	public VirturalJoyStick camjoyStick;
	public Rigidbody ourdrone;
	public Transform lookAtPoint;
	public thuster[] Thusters;
	public Transform Camera;
	public float SmoothCamera = 0.5f;
	public float CameraOffsetX = 0.0f;
	public float CameraOffsetY = 0.0f;
	public float CameraOffsetZ = 0.0f;
	public float SpeedForce = 50.0f;
	public float RollSpeed = 5.0f;
	public float PitchSpeed = 5.0f;
	public float TiltSpeed = 5.0f;
	public float GameArea = 900.0f;

	public float movementSpeed = 50f;
	public float turnSpeed = 50f;

//	private float speed =0.0f;
//	private bool isThrottle = false;
//	private bool isThrottle_Boost = false;
	public Vector3 Camoffset;
	public float DistanceDamp = 0.1f;
	public float rotationalDamp = 0.1f;
	private float warpTime = 0.0f;
	private float cooldownTime = 0.0f;

	Vector3 oldPosition;
	Vector3 currentPosition;
	Quaternion oldRotation;
	Quaternion currentRotation;

	private void Start()
	{
		

		ourdrone = GetComponent<Rigidbody> ();

		oldPosition = transform.position;
		currentPosition = oldPosition;
		oldRotation = transform.rotation;
		currentRotation = oldRotation;


	}
	private void LateUpdate()
	{

		if(PlayerController.Instance !=null && !PlayerController.Instance.isLocalPlayer)
		{
			return;
		}

		currentPosition = transform.position;
		currentRotation = transform.rotation;

		if(currentPosition != oldPosition)
		{
			NetworkManager.Instance.CommandMove (transform.position);
			Data_Manager.Instance.SetUserPosX (transform.position.x.ToString());
			Data_Manager.Instance.SetUserPosY (transform.position.y.ToString());
			Data_Manager.Instance.SetUserPosZ (transform.position.z.ToString());
			oldPosition = currentPosition;
		}

		if(currentRotation != oldRotation)
		{
			NetworkManager.Instance.CommandTurn (transform.rotation);
			oldRotation = currentRotation;
		}


		warpTime++;
		cooldownTime++;
		warp ();

		//Camflow ();

		SmoothFollow ();
		JoyStick_Controls ();
		//InGameRange ();
		SwitchThusters ();

	}




	private void warp()
	{

		if (warpTime > 1000) {
			//Debug.Log ("OK WARPTIME IS OVER: = " + warpTime);
			isBoost = false;
			warpTime = 0;
		} 
		else if (warpTime < 1000)
		{
			if (isBoost == true) 
			{
				liftup = +SpeedForce * 10;
				Thrust ();
				//Debug.Log ("WARPTIME: = "+warpTime);
			}

		}
	}
	private void Thrust ()
	{
		if (isBoost == true) 
		{
			//we want to go through objects at warp speed
			ourdrone.isKinematic = true;
		} 
		else 
		{
			ourdrone.isKinematic = false;
		}
		isMoving = true;

		transform.position += transform.forward * movementSpeed * liftup;





	}
	bool isMoving = false;
	private void SwitchThusters()
	{
		if (isMoving == true && isBoost == true) {
			foreach (thuster t in Thusters) {
				t.Activate (true);
			}
		} else if (isMoving == false) {
			foreach (thuster t in Thusters) {
				t.Activate (false);
			}
		}
	}



	/*public void Camflow()
	{
		Vector3 toPos = transform.position + (transform.rotation * Camoffset);
		Vector3 curPos = Vector3.Lerp (Camera.transform.position, toPos, DistanceDamp);
		Camera.transform.position = curPos;
		Quaternion toRot = Quaternion.LookRotation (transform.position - Camera.transform.position, transform.up);
		Quaternion curRot = Quaternion.Slerp (Camera.transform.rotation,toRot, rotationalDamp);
		Camera.transform.rotation = curRot;
	}*/
	//public Vector3 velocity = Vector3.zero;

	private void SmoothFollow()
	{
		//Vector3 toPos = transform.position + (transform.rotation * Camoffset);
		//Vector3 curPos = Vector3.SmoothDamp (Camera.transform.position, toPos, ref velocity,DistanceDamp);
		//Vector3 curPos_F = Vector3.SmoothDamp (Camera.transform.position, curPos, ref velocity,DistanceDamp);
		Vector3 toPos = transform.position + (transform.rotation * Camoffset);
		Vector3 curPos = Vector3.Lerp (Camera.transform.position, toPos, DistanceDamp);
		Camera.transform.position = curPos;
		Quaternion toRot = Quaternion.LookRotation (transform.position - Camera.transform.position, transform.up);
		Quaternion curRot = Quaternion.Slerp (Camera.transform.rotation,toRot, rotationalDamp);
		Camera.transform.rotation = curRot;

	}
	float liftup =0;
	float pullleft =0;
	private void JoyStick_Controls()
	{



		if (joyStick != null && joyStick.InputDicection == Vector3.zero && camjoyStick !=null && camjoyStick.InputDicection == Vector3.zero) {
			//isThrottle = false;
			if (isBoost == false) {
				
				isMoving = false;
				ourdrone.isKinematic = false;
				//velocity = Vector3.zero;
				//ourdrone.velocity = Vector3.zero;
				//ourdrone.drag = 0.0f;
				//ourdrone.mass = 0.0f;
			//Thrust ();
			}

		}
		if (joyStick !=null && joyStick.InputDicection != Vector3.zero)
		{
			if (isBoost == false) {
				liftup = +SpeedForce * 1;
				Thrust ();
			}

			Debug.Log ("X: "+joyStick.InputDicection.normalized.x);
			Debug.Log ("Z: "+joyStick.InputDicection.normalized.z);
			if (joyStick.InputDicection.normalized.x < 0.8f && joyStick.InputDicection.normalized.z > 0.5f) {



				//transform.Rotate(0,TiltSpeed,0); 



			}

			if (joyStick.InputDicection.normalized.x > 0.8f && joyStick.InputDicection.normalized.z < 0.5f) {

				pullleft=RollSpeed;
				transform.Rotate(0,pullleft,-pullleft);



			}

			if (joyStick.InputDicection.normalized.x < -0.8f && joyStick.InputDicection.normalized.z > -0.5f) {

				pullleft =-RollSpeed;
				transform.Rotate(0,pullleft,-pullleft);

			}

			if (joyStick.InputDicection.normalized.x > -0.8f && joyStick.InputDicection.normalized.z < -0.5f) {


				//transform.Rotate(0,-TiltSpeed,0);
				transform.Rotate(-TiltSpeed,0,0); 

			}



		}


		if (camjoyStick !=null && camjoyStick.InputDicection != Vector3.zero)
		{
			
			if (isBoost == false) {
				liftup = +SpeedForce * 1;
				Thrust ();
			}
		//	Debug.Log ("X: "+camjoyStick.InputDicection.normalized.x);
			//Debug.Log ("Z: "+camjoyStick.InputDicection.normalized.z);

			if (camjoyStick.InputDicection.normalized.x < 0.8f && camjoyStick.InputDicection.normalized.z > 0.5f) {
				//Debug.Log ("OK GOT IT NEW TOP");


				transform.Rotate(TiltSpeed,0,0); 


			}

			if (camjoyStick.InputDicection.normalized.x > 0.8f && camjoyStick.InputDicection.normalized.z < 0.5f) {
				//Debug.Log ("OK GOT IT NEW RIGHT");
				pullleft=RollSpeed;
				transform.Rotate(0,0,-pullleft);


			}

			if (camjoyStick.InputDicection.normalized.x < -0.8f && camjoyStick.InputDicection.normalized.z > -0.5f) {
			//	Debug.Log ("OK GOT IT NEW LEFT");

				//transform.Rotate(0,-PitchSpeed,0);
				pullleft=-RollSpeed;
				transform.Rotate(0,0,-pullleft);

			}

			if (camjoyStick.InputDicection.normalized.x > -0.8f && camjoyStick.InputDicection.normalized.z < -0.5f) {
				//Debug.Log ("OK GOT IT NEW BOTTOM");

                transform.Rotate(-TiltSpeed,0,0); 

			}



		}

	}

	public bool isBoost = false;
	public void Throttle_Boost()
	{
		
		if (isBoost == false) {
			
			isBoost = true;


		} else {
			isBoost = false;
			ourdrone.isKinematic = false;
		}






	}
	/*
	private void InGameRange()
	{
		if (transform.position.x >= GameArea) {
			transform.position = new Vector3 (0, transform.position.y, transform.position.z);
			//Debug.Log ("36");
		} else if (transform.position.x <= -GameArea) {
			transform.position = new Vector3 (GameArea, transform.position.y, transform.position.z);
			//Debug.Log ("37");
		} else if (transform.position.z >= GameArea) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
			//Debug.Log ("38");
		} else if (transform.position.z <= -GameArea) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, GameArea);
			//Debug.Log ("39");
		}


		if (transform.position.y > GameArea) {
			transform.position = new Vector3 (transform.position.x, GameArea, transform.position.z);
			//Debug.Log ("40");
		}
	}*/
}
