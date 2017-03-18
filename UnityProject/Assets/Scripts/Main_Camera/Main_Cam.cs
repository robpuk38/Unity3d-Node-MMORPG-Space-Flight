using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Cam : MonoBehaviour {

	//gyro
	private Gyroscope gyro;
	private GameObject cameraContainer;
	private Quaternion rotation;

	

	private bool arReady = false;

	

	// Use this for initialization
	void Start () 
	{

		//check if we support both services
		if (!SystemInfo.supportsGyroscope)
		{
			Debug.Log ("This Devices Dose Not Support Gyro");
			return;
		}

		
		cameraContainer = new GameObject("Camera Container");
		cameraContainer.transform.position = transform.position;
		transform.SetParent (cameraContainer.transform);

		gyro = Input.gyro;
		gyro.enabled = true;

		cameraContainer.transform.rotation = Quaternion.Euler (90f, 0, 0);

		rotation = new Quaternion (0,0,1,0);
		
		arReady = true;

		
	}
	
	// Update is called once per frame
	void Update () {

		if (arReady) {
			

			transform.localRotation = gyro.attitude * rotation;

			
		}
   }

};
