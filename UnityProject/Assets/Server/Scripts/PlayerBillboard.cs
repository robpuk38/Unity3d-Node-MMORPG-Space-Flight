using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBillboard : MonoBehaviour {


	void Update () {
		transform.LookAt (Camera.main.transform);
	}
}
