using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearSun_Explotion : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Near Sun Explotion Effect " + other.name);


        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {


                //todo explode object and or any object that is in trigger zone..
                // also send the results through server for now we just destory object;


                other.gameObject.SetActive(false);
                

            }

        }



    }
}
