using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(LineRenderer))]
public class lazor : MonoBehaviour {
	LineRenderer lr;
	Light lasorLight;
	public float MaxDistance = 300f;
	bool canFire = false;
	public float lazorOffTime = 0.5f;
	public float fireDelay = 2.0f;

	void Awake()
	{
		lr = GetComponent<LineRenderer> ();	
		lasorLight = GetComponent<Light> ();
	}
	void Start () {
		lr.enabled = false;
		lasorLight.enabled = false;
		canFire = true;
	}
	/*void Update()
	{
		Debug.DrawRay (transform.position,transform.TransformDirection (Vector3.forward) * MaxDistance, Color.blue);
	}*/
	Vector3 castRay()
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward) * MaxDistance;
		if (Physics.Raycast (transform.position, fwd, out hit)) {
			Debug.Log ("We Hit " + hit.transform.name);

			SpawnExplosion(hit.point, hit.transform);
			return hit.point;
		} 

			Debug.Log ("We Missed ");
		return transform.position + (transform.forward *  MaxDistance);

	}
	void SpawnExplosion(Vector3 hitTargetPos, Transform target)
	{
		explosions temp = target.GetComponent<explosions> ();
		if(temp != null)
		{
			temp.IceBeenHit (hitTargetPos);
			temp.Addforce(hitTargetPos,transform);
		}

	}


	public void FireLazor()
	{
		Vector3 pos = castRay ();
		FireLazor (pos);

	}

	public void FireLazor(Vector3 TargetPos,  Transform target = null)
	{
		if (canFire) {
			if (target != null) 
			{
				SpawnExplosion (TargetPos, target);
			}
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, castRay());
			lr.enabled = true;
			canFire = false;
			lasorLight.enabled = true;
			Invoke ("TurnoffLazor", lazorOffTime);
			Invoke ("CamFire", fireDelay);
		}
	}

	void TurnoffLazor()
	{
		lr.enabled = false;
        lasorLight.enabled = false;
	}


	public float Distance
	{
		get{return MaxDistance; }
	}

	void CamFire()
	{
		canFire = true;
	}

}
