using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astroid_Manager : MonoBehaviour {
	public astriods Astorids;
	public astriods Astorids_1;
	//public astriods Astorids_2;
	//public astriods Astorids_3;

	public int AstoridAmount = 10;
	public int GridSpacing = 100;

	void Start()
	{
		PlaceAstorids ();
	}

	void PlaceAstorids()
	{
		
			
		
			for (int x = 0; x < AstoridAmount; x++) {
				for (int y = 0; y < AstoridAmount; y++) {
					for (int z = 0; z < AstoridAmount; z++) {
						InstantiaeAstroids (x, y, z);
					}
				}

			}

	}

	void InstantiaeAstroids(int x, int y, int z)
	{
		Instantiate (Astorids, 
			new Vector3(transform.position.x+(x*GridSpacing)+ AstoridOffest(),
				transform.position.y + (y*GridSpacing) + AstoridOffest(),
				transform.position.z+(z* GridSpacing)+AstoridOffest()), 
			Quaternion.identity, 
			transform);

		Instantiate (Astorids_1, 
			new Vector3(transform.position.x+(x*GridSpacing)+ AstoridOffest(),
				transform.position.y + (y*GridSpacing) + AstoridOffest(),
				transform.position.z+(z* GridSpacing)+AstoridOffest()), 
			Quaternion.identity, 
			transform);
	}

	float AstoridOffest()
	{
		return Random.Range (-GridSpacing / 2f, GridSpacing / 2f);
	}
}
