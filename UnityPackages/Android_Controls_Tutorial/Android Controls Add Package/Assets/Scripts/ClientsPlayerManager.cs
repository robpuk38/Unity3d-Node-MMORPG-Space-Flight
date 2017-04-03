using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsPlayerManager : MonoBehaviour
{

    public GameObject MainCamera;
    public GameObject Clients_Camera;
    public ClientsControlManager CameraMovementJoystick;
    public ClientsControlManager PlayerMovementJoystick;
    public Transform Player;
    public Transform LookAtPoint;
    public Vector3 Camoffset;
    private Transform Character;

    public float DistanceDamp = 0.1f;
    public float rotationalDamp = 0.1f;
    public float ControllerRotDamp = 0.1f;
    public float MovementSpeed = 0.1f;

    Animator anim;
    private bool DisableAutoFollowCamera;

    string DirectionalName;
    float XAxis = 0;
    float ZAxis = 0;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isMoving = false;




    private void Start()
    {
        MainCamera.SetActive(false);
        Clients_Camera.SetActive(true);
        anim = Player.GetChild(0).GetComponent<Animator>();
        Character = Player.GetChild(0).GetComponent<Transform>();

    }

    private void Update()
    {
        RightJoysticCameraMovement();
        LeftJoystickPlayerMovment();

    }

    private void LateUpdate()
    {
        LookAtTarget();
    }
   
   
    private void LookAtTarget()
    {

       

          if (DisableAutoFollowCamera == false)
          {

              Vector3 toPos = LookAtPoint.position + (LookAtPoint.rotation * Camoffset);
              Vector3 curPos = Vector3.Lerp(Clients_Camera.transform.position, toPos, DistanceDamp);
              Clients_Camera.transform.position = curPos;

              Vector3 pos = new Vector3(LookAtPoint.position.x, LookAtPoint.position.y, LookAtPoint.position.z);
              Quaternion toRot = Quaternion.LookRotation(pos - Clients_Camera.transform.position, transform.up);
              Quaternion curRot = Quaternion.Slerp(Clients_Camera.transform.rotation, toRot, rotationalDamp);
              Clients_Camera.transform.rotation = curRot;

          }
          else
          {
              if (isMoving == false)
              {
                Clients_Camera.transform.rotation = LookAtPoint.transform.rotation;
                Clients_Camera.transform.position = LookAtPoint.transform.position;
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
            isRunning = false;


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
                MovementSpeed = 0.05f;
            }


           
            

            if (PlayerMovementJoystick.Direction.normalized.x < 0.0f && PlayerMovementJoystick.Direction.normalized.z > 0.0f)
            {
               
                if (PlayerMovementJoystick.Direction.normalized.z > 0.1f)
                {
                    DirectionalName = "TOP LEFT DIANGLE";

                    if (anim.GetFloat("ZAxis") < 0.9f)
                    {

                        anim.SetFloat("ZAxis", 0.9f);

                    }
                    Character.position += Character.transform.forward * MovementSpeed;
                    Character.position += -Character.transform.right * MovementSpeed / 2;

                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 0.1f)
                {
                    Character.position += -Character.transform.right * MovementSpeed;
                    DirectionalName = "LEFT";
                }
                
                
            }
            else if (PlayerMovementJoystick.Direction.normalized.x < 0.0f && PlayerMovementJoystick.Direction.normalized.z < 0.0f)
            {
                if (-PlayerMovementJoystick.Direction.normalized.z < 0.9f)
                {
                    DirectionalName = "BOTTOM LEFT DIANGLE";
                    if (anim.GetFloat("ZAxis") < -0.0f)
                    {

                        anim.SetFloat("ZAxis", -0.9f);

                    }

                    Character.position += -Character.transform.right * MovementSpeed / 2;
                    Character.position += -Character.transform.forward * MovementSpeed ;

                }
                else
                {


                    DirectionalName = "BOTTOM";
                    Character.position += -Character.transform.forward * MovementSpeed;


                }

            }
            else if (PlayerMovementJoystick.Direction.normalized.x > 0.0f && PlayerMovementJoystick.Direction.normalized.z > 0.0f)
            {
                if (PlayerMovementJoystick.Direction.normalized.x > 0.1f)
                {
                    DirectionalName = "FORWARD RIGHT DIANGALE";
                    if (anim.GetFloat("ZAxis") < 0.9f)
                    {

                        anim.SetFloat("ZAxis", 0.9f);

                    }


                    Character.position += Character.transform.right * MovementSpeed / 2;
                    Character.position += Character.transform.forward *MovementSpeed ;


                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 0.1f)
                {
                    DirectionalName = "FORWARD ";
                    if (anim.GetFloat("ZAxis") == 0 && anim.GetFloat("XAxis") == 0)
                    {
                        anim.SetFloat("ZAxis", 0.9f);
                    }

                    Character.position += Character.transform.forward * MovementSpeed;




                }

            }
            else if (PlayerMovementJoystick.Direction.normalized.x > 0.0f && PlayerMovementJoystick.Direction.normalized.z < 1.0f)
            {
                if (PlayerMovementJoystick.Direction.normalized.x > 0.9f)
                {


                    DirectionalName = "RIGHT";
                    Character.position += Character.transform.right * MovementSpeed;

                }
                else if (PlayerMovementJoystick.Direction.normalized.x < 1.0f)
                {
                    DirectionalName = "BOTTOM RIGHT DIANGLE";
                    if (anim.GetFloat("ZAxis") < 0.0f)
                    {

                        anim.SetFloat("ZAxis", -0.9f);

                    }

                    Character.position += Character.transform.right * MovementSpeed / 2;
                    Character.position += -Character.transform.forward * MovementSpeed;


                }



            }


            Debug.Log("JOYSTICK MOVEMENT: " + DirectionalName);

        }
    }

    public void CrouchBtn()
    {
        anim.SetBool("CrouhState", true);
    }
    public void RunBtn()
    {
        anim.SetBool("RunState", true);
    }
    public void PickUpBtn()
    {
        anim.SetBool("PickupState", true);
    }
    public void WeaponsBtn()
    {
        anim.SetBool("WeaponState", true);
    }


}
