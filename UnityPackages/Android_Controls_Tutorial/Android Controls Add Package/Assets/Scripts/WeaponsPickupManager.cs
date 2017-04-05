using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPickupManager : MonoBehaviour {

    public GameObject WeaponContainer;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Other Name: "+other.name);
       // Debug.Log("This Name: "+ this.name);

       
            if (this.transform.childCount > 0)
            {
                CheckPlayerHasWeapon(this.transform.GetChild(0).gameObject,this.transform.GetChild(0).name);
            }


            
        
    }

    private void CheckPlayerHasWeapon(GameObject go ,string name)
    {
        Debug.Log("This name is: " + name);

        //to do check if the player is already carring a weapon if so drop the weapon and pick up the new weapon
        if (ClientsPlayerManager.Instance.WeaponsState == false)
        {
            Debug.Log("We Have no Weapon");
          

                // we have no weapon and this is the weapon we hit lets despawn the weapon container box and spawn the weapon in the playes hand and change the weapons stance to true
                ClientsPlayerManager.Instance.WeaponsState = true;
                ClientsPlayerManager.Instance.WeaponContatiner(name);

            Destroy(go);
        }
        else if (ClientsPlayerManager.Instance.WeaponsState == true)
        {
            Debug.Log("We Have a Weapon "+ WeaponContainer.name);
            // If item name is the item in the players had no need to do anything at all
            // but if the item is a different item then we need to drop the weapon from the players hand change the weapons state for a second and then 
            // place the new weapon in the players hand.



            /* if (WeaponContainer.transform.GetChild(0).GetComponent<Transform>().gameObject.activeSelf == true)
             {
                 WeaponContainer.transform.GetChild(0).GetComponent<Transform>().gameObject.SetActive(false);



             }
             if (WeaponContainer.transform.GetChild(1).GetComponent<Transform>().gameObject.activeSelf == true)
             {
                WeaponContainer.transform.GetChild(1).GetComponent<Transform>().gameObject.SetActive(false);
                WeaponContainer.transform.GetChild(0).GetComponent<Transform>().gameObject.SetActive(false);


            }
             if (WeaponContainer.transform.GetChild(2).GetComponent<Transform>().gameObject.activeSelf == true)
             {
                WeaponContainer.transform.GetChild(2).GetComponent<Transform>().gameObject.SetActive(false);
                WeaponContainer.transform.GetChild(0).GetComponent<Transform>().gameObject.SetActive(false);
                WeaponContainer.transform.GetChild(1).GetComponent<Transform>().gameObject.SetActive(false);

            }
             if (WeaponContainer.transform.GetChild(3).GetComponent<Transform>().gameObject.activeSelf == true)
             {
                 

            }
             if (WeaponContainer.transform.GetChild(4).GetComponent<Transform>().gameObject.activeSelf == true)
             {
               

            }*/
            DeactivateAll();

            Debug.Log("Yes We are this fire_sleet re-pickup ");
               ClientsPlayerManager.Instance.WeaponsState = true;
            ClientsPlayerManager.Instance.WeaponContatiner(name);

            Destroy(go);
        }
    }

    private void DeactivateAll()
    {
        WeaponContainer.transform.GetChild(0).GetComponent<Transform>().gameObject.SetActive(false);
        WeaponContainer.transform.GetChild(1).GetComponent<Transform>().gameObject.SetActive(false);
        WeaponContainer.transform.GetChild(2).GetComponent<Transform>().gameObject.SetActive(false);
        WeaponContainer.transform.GetChild(3).GetComponent<Transform>().gameObject.SetActive(false);
        WeaponContainer.transform.GetChild(4).GetComponent<Transform>().gameObject.SetActive(false);
    }
}
