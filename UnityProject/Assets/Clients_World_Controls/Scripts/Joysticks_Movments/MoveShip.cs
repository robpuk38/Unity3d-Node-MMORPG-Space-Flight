using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{


    public VirturalJoyStick joyStick;
    public VirturalJoyStick camjoyStick;
    public Rigidbody ourdrone;
    public Transform lookAtPoint;
    public thuster[] Thusters;
    public Transform Camera;
    public float SmoothCamera = 0.5f;
    public float CameraOffsetX = 0.0f;
    public float CameraOffsetY = 0.0f;
    public float CameraOffsetZ = 0.0f;
    public float SpeedForce = 50.0f;
    public float RollSpeed = 5.0f;
    public float PitchSpeed = 5.0f;
    public float TiltSpeed = 5.0f;
    public float GameArea = 900.0f;

    public float movementSpeed = 50f;
    public float turnSpeed = 50f;
    public Vector3 Camoffset;
    public float DistanceDamp = 0.1f;
    public float rotationalDamp = 0.1f;
    private float warpTime = 0.0f;
    private float cooldownTime = 0.0f;



    private void Start()
    {
        ourdrone = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {

                    Debug.Log("WHO AM ID" + Data_Manager.Instance.GetUserId());

                    GameObject p = GameObject.Find(Data_Manager.Instance.GetUserId()) as GameObject;

                    if (p != null)
                    {

                        Transform ZeroDrone = p.transform.Find("ZeroDrone");
                        // Debug.Log("WHAT " + ZeroDrone.name);
                       

                        Data_Manager.Instance.SetUserPosX(ZeroDrone.GetComponent<Transform>().position.x.ToString());
                        Data_Manager.Instance.SetUserPosY(ZeroDrone.GetComponent<Transform>().position.y.ToString());
                        Data_Manager.Instance.SetUserPosZ(ZeroDrone.GetComponent<Transform>().position.z.ToString());
                        Data_Manager.Instance.SetUserRotX(ZeroDrone.GetComponent<Transform>().rotation.x.ToString());
                        Data_Manager.Instance.SetUserRotY(ZeroDrone.GetComponent<Transform>().rotation.y.ToString());
                        Data_Manager.Instance.SetUserRotZ(ZeroDrone.GetComponent<Transform>().rotation.z.ToString());

                    }

                        
                   // if (isMoving == true)
                  //  {
                        
                        NetworkManager.Instance.CommandMove();
                 //   }

                    if (isBoost == true)
                    {
                        warpTime++;
                        warp();
                    }
                    cooldownTime++;
                    SmoothFollow();
                    JoyStick_Controls();

                    SwitchThusters();

                }
            }
        }

    }




    private void warp()
    {

        if (warpTime > 1000)
        {
            //Debug.Log ("OK WARPTIME IS OVER: = " + warpTime);
            isBoost = false;

            foreach (thuster t in Thusters)
            {
                t.Activate(false);
            }

            warpTime = 0;
        }
        else if (warpTime < 1000)
        {
            if (isBoost == true)
            {
                liftup = +SpeedForce * 10;
                Thrust();
                foreach (thuster t in Thusters)
                {
                    t.Activate(true);
                }
            }

        }
    }
    private void Thrust()
    {
        if (isBoost == true)
        {
            //we want to go through objects at warp speed
            ourdrone.isKinematic = true;
            if (AuidoManager.Instance != null)
            {
                AuidoManager.Instance.SpeedEffect(true, true);
            }
        }
        else
        {
            ourdrone.isKinematic = false;
            if (AuidoManager.Instance != null)
            {
                AuidoManager.Instance.SpeedEffect(true,false);
            }
        }
        isMoving = true;


        
        transform.position += transform.forward * movementSpeed * liftup;
        // Debug.Log("DID WE MAKE IT IN FOR MOVMENT?");




    }
    bool isMoving = false;
    private void SwitchThusters()
    {
        if (isMoving == true && isBoost == true)
        {
            foreach (thuster t in Thusters)
            {
                t.Activate(true);
            }
        }
        else if (isMoving == false)
        {
            foreach (thuster t in Thusters)
            {
                t.Activate(false);
            }
        }
    }





    private void SmoothFollow()
    {

        Vector3 toPos = transform.position + (transform.rotation * Camoffset);
        Vector3 curPos = Vector3.Lerp(Camera.transform.position, toPos, DistanceDamp);
        Camera.transform.position = curPos;
        Quaternion toRot = Quaternion.LookRotation(transform.position - Camera.transform.position, transform.up);
        Quaternion curRot = Quaternion.Slerp(Camera.transform.rotation, toRot, rotationalDamp);
        Camera.transform.rotation = curRot;
    }


    float liftup = 0;
    float pullleft = 0;
    private void JoyStick_Controls()
    {



        if (joyStick != null && joyStick.InputDicection == Vector3.zero && camjoyStick != null && camjoyStick.InputDicection == Vector3.zero)
        {

            if (isBoost == false)
            {

                isMoving = false;
                ourdrone.isKinematic = false;
               
                if (AuidoManager.Instance != null)
                {
                    AuidoManager.Instance.SpeedEffect(false,false);
                }

            }

        }
        if (joyStick != null && joyStick.InputDicection != Vector3.zero)
        {




            if (isBoost == false)
            {
                liftup = +SpeedForce * 1;
                Thrust();
            }

            //Debug.Log ("X: "+joyStick.InputDicection.normalized.x);
            //Debug.Log ("Z: "+joyStick.InputDicection.normalized.z);
            if (joyStick.InputDicection.normalized.x < 0.8f && joyStick.InputDicection.normalized.z > 0.5f)
            {

                liftup = +SpeedForce * 2;





            }

            if (joyStick.InputDicection.normalized.x > 0.8f && joyStick.InputDicection.normalized.z < 0.5f)
            {

                pullleft = RollSpeed;
                transform.Rotate(0, pullleft, -pullleft);



            }

            if (joyStick.InputDicection.normalized.x < -0.8f && joyStick.InputDicection.normalized.z > -0.5f)
            {

                pullleft = -RollSpeed;
                transform.Rotate(0, pullleft, -pullleft);

            }

            if (joyStick.InputDicection.normalized.x > -0.8f && joyStick.InputDicection.normalized.z < -0.5f)
            {



                transform.Rotate(-TiltSpeed, 0, 0);

            }



        }


        if (camjoyStick != null && camjoyStick.InputDicection != Vector3.zero)
        {



            if (isBoost == false)
            {
                liftup = +SpeedForce * 1;
                Thrust();
            }
            //	Debug.Log ("X: "+camjoyStick.InputDicection.normalized.x);
            //Debug.Log ("Z: "+camjoyStick.InputDicection.normalized.z);

            if (camjoyStick.InputDicection.normalized.x < 0.8f && camjoyStick.InputDicection.normalized.z > 0.5f)
            {
                //Debug.Log ("OK GOT IT NEW TOP");


                transform.Rotate(TiltSpeed, 0, 0);


            }

            if (camjoyStick.InputDicection.normalized.x > 0.8f && camjoyStick.InputDicection.normalized.z < 0.5f)
            {
                //Debug.Log ("OK GOT IT NEW RIGHT");
                pullleft = RollSpeed;
                transform.Rotate(0, 0, -pullleft);


            }

            if (camjoyStick.InputDicection.normalized.x < -0.8f && camjoyStick.InputDicection.normalized.z > -0.5f)
            {
                //	Debug.Log ("OK GOT IT NEW LEFT");

                //transform.Rotate(0,-PitchSpeed,0);
                pullleft = -RollSpeed;
                transform.Rotate(0, 0, -pullleft);

            }

            if (camjoyStick.InputDicection.normalized.x > -0.8f && camjoyStick.InputDicection.normalized.z < -0.5f)
            {
                //Debug.Log ("OK GOT IT NEW BOTTOM");

                transform.Rotate(-TiltSpeed, 0, 0);

            }


        }

    }

    public bool isBoost = false;
    public void Throttle_Boost()
    {
        InMovement();
        if (isBoost == false)
        {

            isBoost = true;
          


        }
        else
        {
            isBoost = false;
            ourdrone.isKinematic = false;
        }






    }

    private void InMovement()
    {
        if (AuidoManager.Instance != null)
        {
            AuidoManager.Instance.ButtonClicked();
        }
    }

}
