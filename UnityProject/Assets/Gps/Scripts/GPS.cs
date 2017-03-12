using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour {

	private static GPS instance;
	public static GPS Instance{get{return instance; }}

	public float latitude;
	public float longitude;

	private void Awake()
	{
		instance = this;
		//DontDestroyOnLoad (gameObject);
		StartCoroutine (StartLocationServices());
	}

	private IEnumerator StartLocationServices()
	{
		if(!Input.location.isEnabledByUser)
		{
			//Debug.Log ("User Did NoT Allow GPS Service Create Fake Cords");
			float fake_cordsx = Random.Range (0, 99999);
			float fake_cordsy = Random.Range (0, 99999);
			float fake_cordsz = Random.Range (0, 99999);

			latitude = fake_cordsx * fake_cordsz / fake_cordsy;
			longitude =fake_cordsy - fake_cordsz + fake_cordsx;
			yield break;


		}

		Input.location.Start(10.0f,10.0f);
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) 
		{
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		if(maxWait <= 0)
		{
			Debug.Log ("Time Out");
			yield break;
		}

		if(Input.location.status == LocationServiceStatus.Failed)
		{
			Debug.Log ("Unable To Get devices Location");

			yield break;
		}

		latitude = Input.location.lastData.latitude;
		longitude = Input.location.lastData.longitude;

		yield break;
	}


	public float GetGpsX()
	{
		return latitude;
	}

	public float GetGpsY()
	{
		return latitude * longitude + longitude * latitude / 2;
	}

	public float GetGpsZ()
	{
		return longitude;
	}
}
