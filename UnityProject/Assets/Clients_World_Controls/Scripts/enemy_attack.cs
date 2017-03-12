using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_attack : MonoBehaviour {

	public Transform target;
	public lazor Lazor;
	Vector3 HitPos;
	void Update()
	{
		InFront ();
		HasLineofSight ();

	}
	void Awake()
	{
		if (!target) 
		{
			Debug.Log ("The Player Ship Is Not Attached To The Enemys Attack Script.");
			return;
		}
	}

	bool InFront()
	{
		Vector3 directionToTarget = transform.position - target.position;
        float angle = Vector3.Angle (transform.forward, directionToTarget);
        if (Mathf.Abs (angle) > 90 && Mathf.Abs (angle) < 270) {
			Debug.DrawLine (transform.position, target.position, Color.green);
			Debug.Log ("Ship Is inFront");

			return true;
		} 
		Debug.Log ("Ship Is Not in Front");
        Debug.DrawLine(transform.position, target.position, Color.yellow);
		return false;
	}

	bool HasLineofSight()
	{
		RaycastHit hit;
		Vector3 direction = target.position-transform.position;


		if (Physics.Raycast (Lazor.transform.position, direction, out hit, Lazor.Distance)) 
		{
			//Debug.DrawRay (Lazor.transform.position, hit.point,Color.cyan);
			Debug.Log (hit.transform.name);
			if (hit.transform.CompareTag ("Player")) 
			{
				Debug.DrawRay (Lazor.transform.position, direction,Color.red);
				Debug.Log ("TAG:=Player == "+ hit.transform.name);


				if (InFront ()) {
					HitPos = hit.transform.position;
				Debug.Log ("Fire Lazors" + HitPos);
					Firelazors (HitPos);
				}
				////else
				//{
				//	Debug.Log ("Fire Lazors Ended");
				//}

			}
		}
		return  false;
	}
	void Firelazors(Vector3 hit)
	{
		Lazor.FireLazor (hit,target);
	}
}
