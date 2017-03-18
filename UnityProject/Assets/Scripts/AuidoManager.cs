using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoManager : MonoBehaviour {

    private static AuidoManager instance;
    public static AuidoManager Instance { get { return instance; } }
    public AudioSource ButtonPress;
    public AudioSource Fire;
    public AudioSource ShipExplosion;
    public AudioSource PlanetExplosion;
    public AudioSource Speed;

    void Awake () {
        instance = this;

    }

    public void FireButton()
    {
        if (!Fire.isPlaying)
        {
            Fire.Play();
        }
        else
        {
            Fire.Play();
        }
    }
    private bool vol = false;
    private void Update()
    {
        if(vol == true)
        {
            Speed.volume = 1 - Time.time;
        }
        else
        {
            Speed.volume = 0 + Time.time;
        }
    }

    public void SpeedEffect(bool t,bool w)
    {
        if(w == true)
        {
            Speed.pitch = 0.03f;
        }
        else
        {
            Speed.pitch = 0.01f;
        }
        if (t == true)
        {
            vol = false;
            if (!Speed.isPlaying)
            {
                Speed.Play();
            }
        }
        else
        {
            vol = true;
            
        }

    }

    public void ShipExplosionEffect()
    {
        if (!ShipExplosion.isPlaying)
        {
            ShipExplosion.Play();
        }
        
    }

    public void PlanetExplosionEffect()
    {
        if (!PlanetExplosion.isPlaying)
        {
            PlanetExplosion.Play();
        }

    }

    public void ButtonClicked()
    {
        if (!ButtonPress.isPlaying)
        {
            ButtonPress.Play();
        }
        else
        {
            ButtonPress.Stop();
        }
        
    }
}
