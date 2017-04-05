using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsPlayerManager : MonoBehaviour
{
    private static ClientsPlayerManager instance;
    public static ClientsPlayerManager Instance { get { return instance; } }
    public GameObject MainCamera;
    public GameObject Clients_Camera;
    public ClientsControlManager CameraMovementJoystick;
    public ClientsControlManager PlayerMovementJoystick;
    public Transform Player;
    public Transform LookAtPoint;
    public Transform WeaponsCamerLookAtRot;
    private Transform Character;
    public GameObject WeaponContainer;

    public Vector3 Camoffset;
    public float DistanceDamp = 0.1f;
    public float rotationalDamp = 0.1f;
    public float ControllerRotDamp = 0.1f;
    public float MovementSpeed = 0.1f;


    public bool WeaponsState = false;
    Animator anim;
    private bool DisableAutoFollowCamera;


    float XAxis = 0;
    float ZAxis = 0;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isMoving = false;

    public float Zadjustment = 0;
    public float Xadjustment = 0;
    public float Yadjustment = 0;

    private bool forward = false;
    private bool backward = false;
    private bool left = false;
    private bool right = false;
    private bool forwardleft = false;
    private bool forwardright = false;
    private bool backwardleft = false;
    private bool backwardright = false;



    private void Start()
    {
        instance = this;
        MainCamera.SetActive(false);
        Clients_Camera.SetActive(true);
        anim = Player.GetChild(0).GetComponent<Animator>();
        Character = Player.GetChild(0).GetComponent<Transform>();

    }

    private void Update()
    {
        RightJoysticCameraMovement();
        LeftJoystickPlayerMovment();


        //to do we need to save the players weapons state so the system remebers if the player has a weapon or not
        if (WeaponsState == false)
        {
            anim.SetBool("WeaponState", false);
            if (WeaponsState == false && isRunning == false)
            {
                DistanceDamp = 0.07f;
                rotationalDamp = 0.15f;
                Zadjustment = -2f;
                Xadjustment = 0;
                Yadjustment = -0.5f;

            }
            else if (WeaponsState == false && isRunning == true)
            {
                DistanceDamp = 0.2f;
                rotationalDamp = 0.3f;
                Zadjustment = -1.5f;
                Xadjustment = 0;
                Yadjustment = 0f;
            }


        }
        else if (WeaponsState == true)
        {
            if (WeaponsState == true && isRunning == false)
            {
                DistanceDamp = 0.1f;
                rotationalDamp = 1f;
                Zadjustment = -1.5f;
                Xadjustment = 0;
                Yadjustment = -0.5f;

            }
            else if (WeaponsState == true && isRunning == true)
            {
                DistanceDamp = 0.3f;
                rotationalDamp = 0.3f;
                Zadjustment = -1.5f;
                Xadjustment = 0;
                Yadjustment = -0.5f;
            }
            Debug.Log("THE PLAYER HAS A WEAPON");
            anim.SetBool("WeaponState", true);


        }

        ForwardLeft();
        Left();
        BackwardLeft();
        Backward();
        ForwardRight();
        Forward();
        Right();
        BackwardRight();
    }



    private void LateUpdate()
    {
        LookAtTarget();
    }

    public Vector3 WeaponCamOffest;
    private void LookAtTarget()
    {



        if (DisableAutoFollowCamera == false)
        {



            Vector3 toPos = Vector3.zero;
            Vector3 pos = Vector3.zero;
            Vector3 curPos = Vector3.zero;
            Quaternion toRot = Quaternion.identity;
            Quaternion curRot = Quaternion.identity;
            if (WeaponsState == false)
            {
                Camoffset = new Vector3(Xadjustment, Yadjustment, Zadjustment);
                toPos = LookAtPoint.position + (LookAtPoint.rotation * Camoffset);
                pos = new Vector3(LookAtPoint.position.x, LookAtPoint.position.y, LookAtPoint.position.z);


            }
            else if (WeaponsState == true)
            {
                Camoffset = new Vector3(Xadjustment, Yadjustment, Zadjustment);
                toPos = WeaponsCamerLookAtRot.position + (WeaponsCamerLookAtRot.rotation * Camoffset);

                pos = new Vector3(WeaponsCamerLookAtRot.position.x, WeaponsCamerLookAtRot.position.y, WeaponsCamerLookAtRot.position.z);

            }
            curPos = Vector3.Lerp(Clients_Camera.transform.position, toPos, DistanceDamp);
            Clients_Camera.transform.position = curPos;
            toRot = Quaternion.LookRotation(pos - Clients_Camera.transform.position, transform.up);
            curRot = Quaternion.Slerp(Clients_Camera.transform.rotation, toRot, rotationalDamp);
            Clients_Camera.transform.rotation = curRot;


        }
        else
        {
            if (isMoving == false)
            {

                if (WeaponsState == false)
                {
                    Clients_Camera.transform.rotation = LookAtPoint.transform.rotation;
                    Clients_Camera.transform.position = LookAtPoint.transform.position;
                }
                else if (WeaponsState == true)
                {
                    Quaternion tonewRot = Quaternion.LookRotation(WeaponCamOffest - Clients_Camera.transform.position, transform.up);
                    Clients_Camera.transform.rotation = Quaternion.Slerp(WeaponsCamerLookAtRot.transform.rotation, tonewRot, rotationalDamp);



                    Clients_Camera.transform.position = WeaponsCamerLookAtRot.transform.position;
                }
            }

        }


    }
    float currentRotation = 0.0f;

    private void RightJoysticCameraMovement()
    {


        if (CameraMovementJoystick != null && CameraMovementJoystick.Direction == Vector3.zero)
        {
            DisableAutoFollowCamera = false;

            LookAtPoint.transform.rotation = Character.transform.rotation;



        }

        if (CameraMovementJoystick != null && CameraMovementJoystick.Direction != Vector3.zero)
        {
            DisableAutoFollowCamera = true;
            currentRotation += CameraMovementJoystick.Direction.normalized.x;


            LookAtPoint.rotation = Quaternion.identity * Quaternion.AngleAxis(currentRotation, Vector3.up);
            Character.rotation = Quaternion.identity * Quaternion.AngleAxis(currentRotation, Vector3.up);



            Vector3 pos = new Vector3(LookAtPoint.position.x, LookAtPoint.position.y, LookAtPoint.position.z);
            Quaternion toRot = Quaternion.LookRotation(pos, transform.up);
            Quaternion curRot = Quaternion.Slerp(Clients_Camera.transform.rotation, toRot, ControllerRotDamp);
            Clients_Camera.transform.rotation = curRot;




        }


    }

    private void LeftJoystickPlayerMovment()
    {
        if (PlayerMovementJoystick != null && PlayerMovementJoystick.Direction == Vector3.zero)
        {

            // We are not moving the joystick
            anim.SetFloat("ZAxis", 0);
            anim.SetFloat("XAxis", 0);

            XAxis = 0;
            ZAxis = 0;


            isWalking = false;
            isMoving = false;

          

        }

        if (PlayerMovementJoystick != null && PlayerMovementJoystick.Direction != Vector3.zero)
        {
            // We are moving the joystick
            DisableAutoFollowCamera = false;

            XAxis = PlayerMovementJoystick.Direction.normalized.x;
            ZAxis = PlayerMovementJoystick.Direction.normalized.z;

            anim.SetFloat("ZAxis", ZAxis);
            anim.SetFloat("XAxis", XAxis);
            isMoving = true;

            if (isWalking == true && isRunning == false)
            {
                MovementSpeed = 0.01f;
            }
            else if (isWalking == false && isRunning == true)
            {
                MovementSpeed = 0.015f;
            }


            Character.Translate(XAxis * ZAxis * Character.transform.forward * MovementSpeed);

            XAxis += PlayerMovementJoystick.Direction.normalized.x;


            currentRotation += PlayerMovementJoystick.Direction.normalized.x;


            LookAtPoint.rotation = Quaternion.identity * Quaternion.AngleAxis(currentRotation, Vector3.up);
            Character.rotation = Quaternion.identity * Quaternion.AngleAxis(currentRotation, Vector3.up);



            Vector3 pos = new Vector3(LookAtPoint.position.x, LookAtPoint.position.y, LookAtPoint.position.z);
            Quaternion toRot = Quaternion.LookRotation(pos, transform.up);
            Quaternion curRot = Quaternion.Slerp(Clients_Camera.transform.rotation, toRot, ControllerRotDamp);
            Clients_Camera.transform.rotation = curRot;







            if (PlayerMovementJoystick.Direction.normalized.x < 0.0f && PlayerMovementJoystick.Direction.normalized.z > 0.0f)
            {

                if (PlayerMovementJoystick.Direction.normalized.z > 0.1f)
                {


                    if (anim.GetFloat("ZAxis") < 0.9f)
                    {

                        anim.SetFloat("ZAxis", 0.9f);

                    }
                   // ForwardLeft();
                   

                    forward = false;
                    backward = false;
                    left = false;
                    right = false;
                    forwardleft = true;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = false;



                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 0.1f)
                {


                   // Left();
                  
                    forward = false;
                    backward = false;
                    left = true;
                    right = false;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = false;

                }



            }
            else if (PlayerMovementJoystick.Direction.normalized.x < 0.0f && PlayerMovementJoystick.Direction.normalized.z < 0.0f)
            {
                if (-PlayerMovementJoystick.Direction.normalized.z < 0.9f)
                {

                    if (anim.GetFloat("ZAxis") < -0.0f)
                    {

                        anim.SetFloat("ZAxis", -0.9f);

                    }
                   // BackwardLeft();
                    
                    forward = false;
                    backward = false;
                    left = false;
                    right = false;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = true;
                    backwardright = false;


                }
                else
                {

                    //Backward();
                   
                    forward = false;
                    backward = true;
                    left = false;
                    right = false;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = false;
                }

            }
            else if (PlayerMovementJoystick.Direction.normalized.x > 0.0f && PlayerMovementJoystick.Direction.normalized.z > 0.0f)
            {
                if (PlayerMovementJoystick.Direction.normalized.x > 0.1f)
                {

                    if (anim.GetFloat("ZAxis") < 0.9f)
                    {

                        anim.SetFloat("ZAxis", 0.9f);

                    }

                   // ForwardRight();
                    
                    forward = false;
                    backward = false;
                    left = false;
                    right = false;
                    forwardleft = false;
                    forwardright = true;
                    backwardleft = false;
                    backwardright = false;



                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 0.1f)
                {

                    if (anim.GetFloat("ZAxis") == 0 && anim.GetFloat("XAxis") == 0)
                    {
                        anim.SetFloat("ZAxis", 0.9f);
                    }

                  //  Forward();
                   
                    forward = true;
                    backward = false;
                    left = false;
                    right = false;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = false;



                }

            }
            else if (PlayerMovementJoystick.Direction.normalized.x > 0.0f && PlayerMovementJoystick.Direction.normalized.z < 1.0f)
            {
                if (PlayerMovementJoystick.Direction.normalized.x > 0.9f)
                {



                    //Right();
                    
                    forward = false;
                    backward = false;
                    left = false;
                    right = true;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = false;



                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 1.0f)
                {

                    if (anim.GetFloat("ZAxis") < 0.0f)
                    {

                        anim.SetFloat("ZAxis", -0.9f);

                    }


                    //BackwardRight();
                    forward = false;
                    backward = false;
                    left = false;
                    right = false;
                    forwardleft = false;
                    forwardright = false;
                    backwardleft = false;
                    backwardright = true;

                }



            }




        }
    }

    public void CrouchBtn()
    {
        anim.SetBool("CrouhState", true);
    }
    public void RunBtn()
    {
        if (anim.GetBool("RunState") == true)
        {
            anim.SetBool("RunState", false);
            isRunning = false;

        }
        else
        {

            anim.SetBool("RunState", true);
            isRunning = true;
        }


    }
    public void PickUpBtn()
    {
        anim.SetBool("PickupState", true);
    }
    public void WeaponsBtn()
    {
        anim.SetBool("WeaponState", true);
    }


    public void WeaponContatiner(string WeaponName)
    {

        Debug.Log("WEAPON NAME IS " + WeaponName);
        if (WeaponName == "fire_sleet")
        {
            WeaponContainer.GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
            Debug.Log("WE ARE  " + WeaponName);


        }
        if (WeaponName == "archtronic")
        {
            WeaponContainer.GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            Debug.Log("WE ARE  " + WeaponName);


        }
        if (WeaponName == "grimbrand")
        {
            WeaponContainer.GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            Debug.Log("WE ARE  " + WeaponName);


        }
        if (WeaponName == "hellwailer")
        {
            WeaponContainer.GetComponent<Transform>().GetChild(3).gameObject.SetActive(true);
            Debug.Log("WE ARE  " + WeaponName);


        }
        if (WeaponName == "mauler")
        {
            WeaponContainer.GetComponent<Transform>().GetChild(4).gameObject.SetActive(true);
            Debug.Log("WE ARE  " + WeaponName);


        }

    }

    private void Forward()
    {
        anim.SetBool("Forward_Idle_Walking", forward);
        anim.SetBool("isMoving", isMoving);
    }
    private void Backward()
    {

    }
    private void Left()
    {
        anim.SetBool("Left_Idle_Walking", left);
        anim.SetBool("isMoving", isMoving);
    }
    private void Right()
    {
        anim.SetBool("Right_Idle_Walking", right);
        anim.SetBool("isMoving", isMoving);
    }
    private void ForwardLeft()
    {
        anim.SetBool("Forward_Left_Idle_Walking", forwardleft);
        anim.SetBool("isMoving", isMoving);
    }
    private void ForwardRight()
    {
        anim.SetBool("Forward_Right_Idle_Walking", forwardright);
        anim.SetBool("isMoving", isMoving);
    }
    private void BackwardLeft()
    {

    }
    private void BackwardRight()
    {

    }

}
