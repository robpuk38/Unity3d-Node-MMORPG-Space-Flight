using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_around : MonoBehaviour {


	public Transform target_to_rotate_around;
	public float x =0.0f;
	public float y =0.0f;
	public float z =0.0f;
	public float speed = 0.1f;
	 


	// Update is called once per frame
	void Update () {
		Vector3 angle = new Vector3(x,y,z);
		transform.RotateAround (target_to_rotate_around.position, angle,speed);
	}
}
