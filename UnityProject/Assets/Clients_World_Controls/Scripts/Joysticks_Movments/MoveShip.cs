using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{

    private static MoveShip instance;
    public static MoveShip Instance { get { return instance; } }

    public VirturalJoyStick joyStick;

    public VirturalJoyStick camjoyStick;

    

    Rigidbody ourdrone;
    GameObject player;
    GameObject Thusterlight1;
    GameObject Thusterlight2;
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
    public bool isMoving = false;
    public float DistanceDamp = 0.1f;
    public float rotationalDamp = 0.1f;
    public bool Fire = false;
    private float cooldownTime = 0.0f;
    public bool isBoost = false;


    private void Start()
    {
        instance = this;
      

    }
  
    private void Update()
    {
        

        

        
        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {

                    NetworkManager.Instance.CommandMove(isMoving, isBoost);
                }
            }

            player = GameObject.Find("ZeroDrone_" + Data_Manager.Instance.GetUserId()) as GameObject;
            if (player != null)
            {
                ourdrone = player.GetComponent<Rigidbody>();

                Thusterlight2 = player.transform.GetChild(0).GetChild(0).gameObject;

                Thusterlight1 = player.transform.GetChild(1).GetChild(0).gameObject;




            }
        }
        

       
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            if (Data_Manager.Instance != null)
            {
                if (NetworkManager.Instance != null)
                {
                    if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                    {



                        GameObject ZeroDrone = GameObject.Find("ZeroDrone_" + Data_Manager.Instance.GetUserId()) as GameObject;

                        if (ZeroDrone != null)
                        {
                            //Debug.Log("WE ARE NO LONGER NULL BUT WE WAS WHEN IT CALLED FIRST");


                            Data_Manager.Instance.SetUserPosX(ZeroDrone.GetComponent<Transform>().position.x.ToString());
                            Data_Manager.Instance.SetUserPosY(ZeroDrone.GetComponent<Transform>().position.y.ToString());
                            Data_Manager.Instance.SetUserPosZ(ZeroDrone.GetComponent<Transform>().position.z.ToString());

                            Data_Manager.Instance.SetUserRotX(ZeroDrone.GetComponent<Transform>().rotation.eulerAngles.x.ToString());
                            Data_Manager.Instance.SetUserRotY(ZeroDrone.GetComponent<Transform>().rotation.eulerAngles.y.ToString());
                            Data_Manager.Instance.SetUserRotZ(ZeroDrone.GetComponent<Transform>().rotation.eulerAngles.z.ToString());



                        }




                        if (player.GetComponent<MoveShip>().isBoost == true)
                        {

                            warp();
                        }
                        cooldownTime++;
                        SmoothFollow();
                        JoyStick_Controls();



                    }
                }
            }
        }

    }




    private void warp()
    {

        liftup = +SpeedForce * 20;
        Thrust();
        Thusterlight1.SetActive(true);
        Thusterlight2.SetActive(true);
          

       
    }
    private void Thrust()
    {
        if (player.GetComponent<MoveShip>().isBoost == true)
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
                AuidoManager.Instance.SpeedEffect(true, false);
            }
        }
        isMoving = true;


       

       player.transform.position += player.transform.forward * movementSpeed * liftup;
        // Debug.Log("DID WE MAKE IT IN FOR MOVMENT?");




    }
   
  





    private void SmoothFollow()
    {
        if (Camera != null && player.transform != null)
        {

            Vector3 toPos = player.transform.position + (player.transform.rotation * Camoffset);
            Vector3 curPos = Vector3.Lerp(Camera.transform.position, toPos, DistanceDamp);
            Camera.transform.position = curPos;
            Quaternion toRot = Quaternion.LookRotation(player.transform.position - Camera.transform.position, player.transform.up);
            Quaternion curRot = Quaternion.Slerp(Camera.transform.rotation, toRot, rotationalDamp);
            Camera.transform.rotation = curRot;
        }
    }


    float liftup = 0;
    float pullleft = 0;
   
    private void JoyStick_Controls()
    {
        
        
        if (joyStick != null && joyStick.InputDicection == Vector3.zero && camjoyStick != null && camjoyStick.InputDicection == Vector3.zero)
        {

            if (player.GetComponent<MoveShip>().isBoost == false)
            {

                isMoving = false;
                if (ourdrone != null)
                {
                    ourdrone.isKinematic = false;
                }

                if (AuidoManager.Instance != null)
                {
                    AuidoManager.Instance.SpeedEffect(false, false);
                }

            }

        }
        if (joyStick != null && joyStick.InputDicection != Vector3.zero)
        {




            if (player.GetComponent<MoveShip>().isBoost == false)
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
                player.transform.Rotate(0, pullleft, -pullleft);



            }

            if (joyStick.InputDicection.normalized.x < -0.8f && joyStick.InputDicection.normalized.z > -0.5f)
            {

                pullleft = -RollSpeed;
                player.transform.Rotate(0, pullleft, -pullleft);

            }

            if (joyStick.InputDicection.normalized.x > -0.8f && joyStick.InputDicection.normalized.z < -0.5f)
            {



                player.transform.Rotate(-TiltSpeed, 0, 0);

            }



        }


        if (camjoyStick != null && camjoyStick.InputDicection != Vector3.zero)
        {



            if (player.GetComponent<MoveShip>().isBoost == false)
            {
                liftup = +SpeedForce * 1;
                Thrust();
            }
            //	Debug.Log ("X: "+camjoyStick.InputDicection.normalized.x);
            //Debug.Log ("Z: "+camjoyStick.InputDicection.normalized.z);

            if (camjoyStick.InputDicection.normalized.x < 0.8f && camjoyStick.InputDicection.normalized.z > 0.5f)
            {
                //Debug.Log ("OK GOT IT NEW TOP");


                player.transform.Rotate(TiltSpeed, 0, 0);


            }

            if (camjoyStick.InputDicection.normalized.x > 0.8f && camjoyStick.InputDicection.normalized.z < 0.5f)
            {
                //Debug.Log ("OK GOT IT NEW RIGHT");
                pullleft = RollSpeed;
                player.transform.Rotate(0, 0, -pullleft);


            }

            if (camjoyStick.InputDicection.normalized.x < -0.8f && camjoyStick.InputDicection.normalized.z > -0.5f)
            {
                //	Debug.Log ("OK GOT IT NEW LEFT");

                //transform.Rotate(0,-PitchSpeed,0);
                pullleft = -RollSpeed;
                player.transform.Rotate(0, 0, -pullleft);

            }

            if (camjoyStick.InputDicection.normalized.x > -0.8f && camjoyStick.InputDicection.normalized.z < -0.5f)
            {
                //Debug.Log ("OK GOT IT NEW BOTTOM");

                player.transform.Rotate(-TiltSpeed, 0, 0);

            }


        }

    }



   
    public void ButtonA()
    {


        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {
                    if (AuidoManager.Instance != null)
                    {
                        AuidoManager.Instance.ButtonClicked();
                    }

                    if (player.GetComponent<MoveShip>().isBoost == false)
                    {
                        // Debug.Log("GOING IN WARP");
                        player.GetComponent<MoveShip>().isBoost = true;

                        if (ourdrone != null)
                        {
                            ourdrone.isKinematic = true;
                        }
                        if (Thusterlight2 != null & Thusterlight1 != null)
                        {
                            Thusterlight1.SetActive(true);
                            Thusterlight2.SetActive(true);
                        }

                    }
                    else
                    {
                        // Debug.Log("STOPING WARP");
                        player.GetComponent<MoveShip>().isBoost = false;

                        if (ourdrone != null)
                        {
                            ourdrone.isKinematic = false;
                        }
                        if (Thusterlight2 != null & Thusterlight1 != null)
                        {
                            Thusterlight1.SetActive(false);
                            Thusterlight2.SetActive(false);
                        }
                    }
                }
            }
        }
       
        
    }

    public void ButtonB()
    {
       
        //Debug.Log("B CLICKED");
    }
    public void ButtonX()
    {
        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {
                    // Debug.Log("X CLICKED");
                    Fire = true;
                    if (AuidoManager.Instance != null)
                    {
                        AuidoManager.Instance.FireButton();
                    }

                    // lazor.Instance.FireLazor();
                    NetworkManager.Instance.CommandShoot();

                }
            }
        }

    }

    public void ButtonY()
    {
        
       // Debug.Log("Y CLICKED");
    }

   

}
