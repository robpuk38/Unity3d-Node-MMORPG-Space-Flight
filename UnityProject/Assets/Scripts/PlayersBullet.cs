using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersBullet : MonoBehaviour {
	[HideInInspector]
	public GameObject playerFrom;
	void OnCollisionEnter(Collision collision)
	{
		
		var hit = collision.gameObject;

		Debug.Log ("Hello I WAS HIT "+ hit.name);
		var health = hit.GetComponent<PlayersHealth> ();
		if(health != null)
		{
			health.TakeDamage (playerFrom, 10);
		}
		Destroy (gameObject);
	}
}
