using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(LineRenderer))]
public class lazor : MonoBehaviour {

    private static lazor instance;
    public static lazor Instance { get { return instance; } }
    LineRenderer lr;
	Light lasorLight;
    public GameObject Shot1;
    public GameObject Wave;


    public float MaxDistance = 300f;
	bool canFire = false;
	public float lazorOffTime = 0.5f;
	public float fireDelay = 2.0f;

	void Awake()
	{
        instance = this;
        lr = GetComponent<LineRenderer> ();	
		lasorLight = GetComponent<Light> ();
	}
	void Start () {
		lr.enabled = false;
		lasorLight.enabled = false;
		canFire = true;
	}
	
	Vector3 castRay()
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward) * MaxDistance;
		if (Physics.Raycast (transform.position, fwd, out hit)) {
			Debug.Log ("We Hit " + hit.transform.name);

			//SpawnExplosion(hit.point, hit.transform);
			return hit.point;
		} 

			Debug.Log ("We Missed ");
		return transform.position + (transform.forward *  MaxDistance);

	}
    /*
	void SpawnExplosion(Vector3 hitTargetPos, Transform target)
	{
		explosions temp = target.GetComponent<explosions> ();
		if(temp != null)
		{
			temp.IceBeenHit (hitTargetPos);
			temp.Addforce(hitTargetPos,transform);
		}

	}*/


	public void FireLazor()
	{
		Vector3 pos = castRay ();
		FireLazor (pos);

	}

	public void FireLazor(Vector3 TargetPos,  Transform target = null)
	{


        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {



                    GameObject p = GameObject.Find(Data_Manager.Instance.GetUserId()) as GameObject;

                    if (p != null)
                    {

                        Transform ZeroDrone = p.transform.Find("ZeroDrone");

                       

                        NetworkManager.Instance.CommandShoot(true);
                    }

                }
            }
        }


         /*               if (canFire) {
			if (target != null) 
			{
				SpawnExplosion (TargetPos, target);
			}
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, castRay());
			lr.enabled = true;
			canFire = false;
			lasorLight.enabled = true;
            if (NetworkManager.Instance != null)
            {
                NetworkManager.Instance.CommandShoot(true);
            }

        //GameObject Bullet;
        // Bullet = Shot1;
        //Fire
            GameObject s1 = (GameObject)Instantiate(Shot1, this.transform.position, this.transform.rotation);
        s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

        GameObject wav = (GameObject)Instantiate(Wave, this.transform.position, this.transform.rotation);
        wav.transform.localScale *= 0.25f;
        wav.transform.Rotate(Vector3.left, 90.0f);
        wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;

        Invoke ("TurnoffLazor", lazorOffTime);
        Invoke ("CamFire", fireDelay);
        }
*/
      
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
