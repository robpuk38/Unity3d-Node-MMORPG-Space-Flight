using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePoint : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I AM ME "+other.name);


        if(Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {


                if ("ZeroDrone_"+Data_Manager.Instance.GetUserId() == other.name)
                {

                    Debug.Log("DID WE MAKE IT IN  " + other.name);
                    float posx;
                    float posy;
                    float posz;
                    float.TryParse(Data_Manager.Instance.GetUserGpsX(), out posx);
                    float.TryParse(Data_Manager.Instance.GetUserGpsY(), out posy);
                    float.TryParse(Data_Manager.Instance.GetUserGpsZ(), out posz);

                    float rotx;
                    float roty;
                    float rotz;
                    float.TryParse(Data_Manager.Instance.GetUserGpsX(), out rotx);
                    float.TryParse(Data_Manager.Instance.GetUserGpsY(), out roty);
                    float.TryParse(Data_Manager.Instance.GetUserGpsZ(), out rotz);
                    Vector3 Pos = new Vector3(posx, posy, posz);
                    Quaternion Rot = Quaternion.Euler(rotx, roty, rotz);
                    other.transform.position = Pos;
                    other.transform.rotation = Rot;
                    NetworkManager.Instance.CommandMoveObject(other.name, posx, posy, posz, rotx, roty, rotz);
                }
            }
            
        }
       

        
    }
}
