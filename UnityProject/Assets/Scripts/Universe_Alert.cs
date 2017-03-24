using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe_Alert : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I AM " + other.name);


        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {


                if ("ZeroDrone_" + Data_Manager.Instance.GetUserId() == other.name)
                {

                    Debug.Log("WE ARE OUR SELFS NO NEED TO ALERT ");
                    //WE ARE OUR SELFS SO WE DO NOT NEED TO TRIGGER A ALERT BUT IF WE ARE NOT OURSELFS WE WANT TO TRIGGER ALERT
                    /*float posx;
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
                    NetworkManager.Instance.CommandMoveObject(other.name, posx, posy, posz, rotx, roty, rotz);*/
                }
                else
                {
                    Debug.Log("WE ARE A THREAT OR A ALLIE  " + other.name);
                }
            }

        }



    }
}
