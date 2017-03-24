using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFix : MonoBehaviour {

    public float scaleX;
    public float scaleY;
    public float scaleZ;

    
    void Update ()
    {
        this.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
	}
}
