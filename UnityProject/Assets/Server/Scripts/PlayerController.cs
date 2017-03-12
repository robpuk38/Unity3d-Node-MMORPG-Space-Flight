using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private static PlayerController instance;
	public static PlayerController Instance{get{return instance; }}

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public bool isLocalPlayer = false;
	//Vector3 oldPosition;
	//Vector3 currentPosition;
	//Quaternion oldRotation;
	//Quaternion currentRotation;
	// Use this for initialization
	void Awake () {
		instance = this;
		//oldPosition = transform.position;
		//currentPosition = oldPosition;
		//oldRotation = transform.rotation;
		//currentRotation = oldRotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!isLocalPlayer)
		{
			return;
		}

	/*	var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate (0,x,0);
		transform.Translate (0,0,z);

		currentPosition = transform.position;
		currentRotation = transform.rotation;

		if(currentPosition != oldPosition)
		{
			NetworkManager.Instance.CommandMove (transform.position);
			oldPosition = currentPosition;
		}

		if(currentRotation != oldRotation)
		{
			NetworkManager.Instance.CommandTurn (transform.rotation);
			oldRotation = currentRotation;
		}


		if(Input.GetKeyDown(KeyCode.Space))
		{
			NetworkManager.Instance.CommandShoot ();
		}*/
	}

	public void CmdFire()
	{
		var bullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;

		PlayersBullet b = bullet.GetComponent<PlayersBullet> ();
		b.playerFrom = this.gameObject;
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.up * 6;
		Destroy (bullet, 2.0f);
	}
}
