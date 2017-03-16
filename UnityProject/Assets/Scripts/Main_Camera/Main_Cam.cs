using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Cam : MonoBehaviour {

	//gyro
	private Gyroscope gyro;
	private GameObject cameraContainer;
	private Quaternion rotation;

	//cam
	//private WebCamTexture cam;
	public  RawImage background;
	public  AspectRatioFitter fit;

	private bool arReady = false;

	public Text debugText;

	// Use this for initialization
	void Start () 
	{

		//check if we support both services
		if (!SystemInfo.supportsGyroscope)
		{
			Debug.Log ("This Devices Dose Not Support Gyro");
			return;
		}

		//for(int i = 0; i < WebCamTexture.devices.Length; i++)
		//{
		//	if(!WebCamTexture.devices[i].isFrontFacing)
		//	{
			//	cam = new WebCamTexture (WebCamTexture.devices [i].name, Screen.width, Screen.height);
			//	break;
			//}

		//}

		//if we did not find a back cam lets exit
		//if(cam == null)
		//{
		//	Debug.Log ("Device has no back cam");
		//	return;
		//}

		// both services are suppport lets do it
		cameraContainer = new GameObject("Camera Container");
		cameraContainer.transform.position = transform.position;
		transform.SetParent (cameraContainer.transform);

		gyro = Input.gyro;
		gyro.enabled = true;

		cameraContainer.transform.rotation = Quaternion.Euler (90f, 0, 0);

		rotation = new Quaternion (0,0,1,0);
		//cam.Play ();
		//background.texture = cam;
		arReady = true;

		
	}
	
	// Update is called once per frame
	void Update () {

		if (arReady) {
			//float ratio = (float)cam.width / (float)cam.height;
			//fit.aspectRatio = ratio;

			//float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
			//background.rectTransform.localScale = new Vector3 (1f,scaleY,1f);

			//int orient = -cam.videoRotationAngle;
			//background.rectTransform.localEulerAngles = new Vector3 (0,0,orient);

			transform.localRotation = gyro.attitude * rotation;

			//debugText.text = "transform.localRotation: "+transform.localRotation;
			//transform.Translate (Input.acceleration.x, 0, -Input.acceleration.z);
		}
   }

};
