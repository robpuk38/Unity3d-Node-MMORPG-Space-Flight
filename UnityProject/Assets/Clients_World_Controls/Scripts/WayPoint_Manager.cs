using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint_Manager : MonoBehaviour {

	public GameObject WayPoints;


	public int TotalWaypoints = 10;
	public int GridSpacing = 100;


	void Start()
	{
		PlaceWayPoints ();
	}

	void PlaceWayPoints()
	{



		for (int x = 0; x < TotalWaypoints; x++) {
			for (int y = 0; y < TotalWaypoints; y++) {
				for (int z = 0; z < TotalWaypoints; z++) {
					InstantiaeWayPoints (x, y, z);
				}
			}

		}

	}


	void InstantiaeWayPoints(int x, int y, int z)
	{
		GameObject Ourname = (GameObject)Instantiate (WayPoints, 
			new Vector3(transform.position.x+(x*GridSpacing)+ Offest(),
				transform.position.y + (y*GridSpacing) + Offest(),
				transform.position.z+(z* GridSpacing)+Offest()), 
			Quaternion.identity, 
			transform);

		Ourname.name = "WayPoint_" + x + "_" + y + "_" + z;


	}

	float Offest()
	{
		return Random.Range (-GridSpacing / 2f, GridSpacing / 2f);
	}
}
