using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosions : MonoBehaviour {
	public GameObject explosion;
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


	public void IceBeenHit(Vector3 pos)
	{
        GameObject go = Instantiate (explosion, pos, Quaternion.identity, transform) as GameObject;
		Destroy (go,6f);
	}

	void OnCollisionEnter(Collision col)
	{
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
	}
}
