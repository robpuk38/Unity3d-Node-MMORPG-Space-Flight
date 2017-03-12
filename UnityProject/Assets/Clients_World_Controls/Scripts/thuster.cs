using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(TrailRenderer))]
public class thuster : MonoBehaviour {
	TrailRenderer tr;
	Light thursterLight;
	void Awake()
	{
		tr = GetComponent<TrailRenderer> ();
		thursterLight = GetComponent<Light> ();
	}
	void Start () {
		tr.enabled = false;
		thursterLight.enabled = false;
		thursterLight.intensity = 0f;
	}
	



	public void Activate(bool activate = true)
	{
		if (activate) {
			tr.enabled = true;
			thursterLight.enabled = true;
		} 
		else 
		{
			tr.enabled = false;
			thursterLight.enabled = false;
		}
	}

	public void Intensity(float intent)
	{
		thursterLight.intensity = intent * 2f;
	}
}
