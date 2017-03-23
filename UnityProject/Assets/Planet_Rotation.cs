using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rotation : MonoBehaviour {


    // Simple rotation 
    public float RotateX = 5.0f;
    public float RotateY = 0.1f;
    public float RotateZ = 0.1f;
    
    void Update()
    {
        transform.Rotate(RotateX, RotateY, RotateZ);
    }
}
