using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosions : MonoBehaviour {
	/*public GameObject explosion;
	public Rigidbody rb;
	public float Lazorhitmodifyer = 100.0f;

	void Start()
	{
		if (!rb) {
			Debug.Log ("Missing RigidBody On Explostion Script!");
			return;
		}
		
		if (!explosion ) {
			Debug.Log ("Missing Explosion On Explostion Script!");
			return;
		}
	}
    int UpdateTime = 0;
    private void Update()
    {
        if (NetworkManager.Instance != null)
        {
            if (Data_Manager.Instance != null)
            {
                if (this.transform.name == "ZeroDrone_"+Data_Manager.Instance.GetUserId())
                {
                    Debug.Log("WE ARE SENDING OR SELF TO THE SERVER AGAIN");
                    return;
                }
            }

            Debug.Log("WE ARE "+ this.transform.ToString());
            UpdateTime++;

            if(UpdateTime > 50)
            {
                NetworkManager.Instance.CommandMoveObject(this.transform.name,
                 this.transform.position.x,
                 this.transform.position.y,
                 this.transform.position.z,
                 this.transform.rotation.eulerAngles.x,
                 this.transform.rotation.eulerAngles.y,
                 this.transform.rotation.eulerAngles.z);
                UpdateTime = 0;
            }
             
        }
    }



    public void IceBeenHit(Vector3 pos)
	{
        GameObject go = Instantiate (explosion, pos, Quaternion.identity, transform) as GameObject;
		Destroy (go,6f);
	}

	void OnCollisionEnter(Collision col)
	{

        Debug.Log("I HAVE BEEN HIT WHAT AMD I? "+ this.transform.name);

        

        foreach (ContactPoint contact in col.contacts) {
			IceBeenHit (contact.point);
		}
	}

	public void Addforce(Vector3 hitPos,Transform hitfrom)
	{
		if (!rb) {
			Debug.Log ("Missing RigidBody On Explostion Script!");
			return;
		}	

		Vector3 direction = (hitfrom.position + hitPos).normalized;
		rb.AddForceAtPosition (direction * Lazorhitmodifyer ,hitPos,ForceMode.Impulse);
	}*/
}
