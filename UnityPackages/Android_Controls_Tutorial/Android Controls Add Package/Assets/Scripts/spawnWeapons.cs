using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnWeapons : MonoBehaviour
{

    public GameObject[] Weapon;

    float randomWeapon = 0;
    float MinVal = 0;
    float Max = 0;

    public float MaxVal
    {
        get
        {
            return Max;
        }

        set
        {
            Max = Weapon.Length;
        }
    }

    private void Start()
    {
        Randomize();
    }
    private void Randomize()
    {
        MaxVal = Weapon.Length;
        randomWeapon = Random.Range(MinVal, MaxVal);
        SpawnRandomWeapon(randomWeapon);
    }
    private void SpawnRandomWeapon(float randomWeapon)
    {
        Vector3 Pos = this.transform.position;
        Quaternion Rot = Weapon[(int)randomWeapon].transform.rotation;
        GameObject NewWeapon = Instantiate(Weapon[(int)randomWeapon], Pos, Rot) as GameObject;
        NewWeapon.name = Weapon[(int)randomWeapon].name;
        NewWeapon.SetActive(true);

        NewWeapon.transform.parent = this.transform;

    }
    int spawnTime = 0;
    private void Update()
    {

      
        if (this.transform.childCount < 1)
        {
            spawnTime++;
            if (spawnTime > 2000)
            {
                // lets respawn a new random Weapon
                Randomize();
                spawnTime = 0;
            }
            Debug.Log("We are a weapon that is deactivated ");
        }
    }
}
