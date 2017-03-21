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

    private bool hasHit = false;
    public float MaxDistance = 300f;
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
		
	}
	
	Vector3 castRay()
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward) * MaxDistance;
		if (Physics.Raycast (transform.position, fwd, out hit))
        {
			Debug.Log ("We Hit " + hit.transform.name);
            hasHit = true;
            //SpawnExplosion(hit.point, hit.transform);

            NetworkManager.Instance.CommandShoot(true, hit.point, hit.transform);

            return hit.point;
		}
        hasHit = false;

            Debug.Log ("We Missed ");
		return transform.position + (transform.forward *  MaxDistance);

	}


    


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

                       
                            lr.SetPosition(0, transform.position);
                            lr.SetPosition(1, castRay());
                            lr.enabled = true;
                            lasorLight.enabled = true;
                            Invoke("TurnoffLazor", lazorOffTime);
                          
                        if(hasHit == false)
                        { 
                         NetworkManager.Instance.CommandShoot(true, TargetPos, target);
                        }
                    }

                }
            }
        }


         /*               
			if (target != null) 
			{
				
			}
			
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

	

}
