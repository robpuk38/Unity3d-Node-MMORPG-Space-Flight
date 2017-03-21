using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerinput : MonoBehaviour {

    private static playerinput instance;
    public static playerinput Instance { get { return instance; } }

    public lazor[] Lazor; 
	bool Fire = false;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
	{
		if (Fire == true) {
			foreach (lazor l in Lazor) {
				//Vector3 pos = transform.position + (transform.forward * l.Distance);
				l.FireLazor ();
				Fire = false;

				Debug.Log ("FIRED TRUE NOW BACK TO FALSE ");
			}
		}
		
	}

	public void FireLazors()
	{
		
			Debug.Log ("FIRE TRUE");
        lazor.Instance.FireLazor();
        Fire = true;
        if (AuidoManager.Instance != null)
        {
            AuidoManager.Instance.FireButton();
        }


    }
}
