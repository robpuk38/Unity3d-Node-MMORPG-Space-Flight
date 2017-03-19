using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;


public class LoadingManger : MonoBehaviour
{
    private static LoadingManger instance;
    public static LoadingManger Instance { get { return instance; } }
    public Image LoadBarFront;
    public Text Percentage;
    public Text Loading;
    public GameObject NetworkManager;
    public GameObject FacebookMaster;
    public Canvas JoinGameCanvas;
    public string GetIPAddress = "73.146.71.203";
    public int GetPort = 3000;


    public CanvasGroup fade;
    private float loadTime;
    private float minTime = 10.0f;
    private string UserName = "";
    private string ServerStatus = "";

    private bool serverCheck = false;
    private bool fadeout = false;
    private bool fadein = false;

    private void Awake()
    {
        instance = this;
        JoinGameCanvas.gameObject.SetActive(false);

    }

    



    private void Start()
    {
        LoadBarFront.fillAmount = 0;

        fade.alpha = 1;
        if(Time.time < minTime)
        {
            loadTime = minTime;
        }
        else
        {
            loadTime = Time.time;
        }



        if (PlayerPrefs.GetString("Id") != null
            && PlayerPrefs.GetString("UserId") != null
            && PlayerPrefs.GetString("UserName") != null
            && PlayerPrefs.GetString("UserPic") != null
            && PlayerPrefs.GetString("VungleApi") != null
            && PlayerPrefs.GetString("AdcolonyApi") != null
            && PlayerPrefs.GetString("AdcolonyZone") != null)
        {
            Debug.Log("TRUE THE ID IS: " + PlayerPrefs.GetString("Id"));
            Debug.Log("TRUE THE USERID IS: " + PlayerPrefs.GetString("UserId"));
            Debug.Log("TRUE THE USERNAME IS: " + PlayerPrefs.GetString("UserName"));
            Debug.Log("TRUE THE USERPIC IS: " + PlayerPrefs.GetString("UserPic"));
            Debug.Log("TRUE THE VUNGLEAPI IS: " + PlayerPrefs.GetString("VungleApi"));
            Debug.Log("TRUE THE ADCOLONYAPI IS: " + PlayerPrefs.GetString("AdcolonyApi"));
            Debug.Log("TRUE THE ADCOLONYZONE IS: " + PlayerPrefs.GetString("AdcolonyZone"));
            UserName = PlayerPrefs.GetString("UserName");

        }
        else
        {
            Debug.Log("The user is not found never login before.");
            UserName = "Guest";
        }

       


    }

    private void Update()
    {
        

        Fadein();

        if (fade.alpha < 1)
        {
            PercentageOf();
        }

        PreloadFadeOut();

        FadeOut();

    }

    private void CheckServerStatus()
    {

        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect(GetIPAddress, GetPort);
            ServerStatus = "Online";
            
            Debug.Log("Port open");
           
        }
        catch (Exception)
        {
            ServerStatus = "Offline";
            Debug.Log("Port closed");
            NetworkManager.SetActive(false);


        }
        

    }

    private void PercentageOf()
    {
        if (LoadBarFront.fillAmount <= 1)
        {
            LoadBarFront.fillAmount += 1.0f / loadTime * Time.deltaTime;
        }

        float percent = LoadBarFront.fillAmount * 100;
        Percentage.text = (percent).ToString("f0")+"%";

        if((percent).ToString("f0") == "0")
        {

            Loading.text = "Welcome " + UserName ;
            if(serverCheck == false)
            {
                CheckServerStatus();
                serverCheck = true;
            }
        }

        if ((percent).ToString("f0") == "5")
        {

            Loading.text = "One Momment Please";
            if (serverCheck == false)
            {
                CheckServerStatus();
                serverCheck = true;
            }
        }

        if ((percent).ToString("f0") == "10")
        {

            Loading.text = "Gathering Profile Information For " + UserName;
            if (serverCheck == false)
            {
                CheckServerStatus();
                serverCheck = true;
            }
        }

        if ((percent).ToString("f0") == "15")
        {

            Loading.text = "Gathering Server Status " + ServerStatus;
            
        }

        if ((percent).ToString("f0") == "20" && ServerStatus =="Offline")
        {

            Loading.text = "Activating Local Play";

        }

        if ((percent).ToString("f0") == "20" && ServerStatus == "Online")
        {

            Loading.text = "Activating Global Play";

        }

        

    }

    private void PreloadFadeOut()
    {
        
        if (LoadBarFront.fillAmount >= 1)
        {
            if (fadein == false)
            {
                fadeout = true;
            }
            
        }
    }



    private void Fadein()
    {
        
        if (fadein == true)
        {
            Debug.Log("fadein ??:" + fadein);

            if(fade.alpha >= 0)
            {
                fade.alpha = 1 - Time.time;
                if (fade.alpha == 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
            
        }
        else
        {
            Debug.Log("fadein ?:" + fadein);
            if (Time.time < minTime)
            {
                fade.alpha = 1 - Time.time;
            }
        }

    }

    private void FadeOut()
    {

        


        if (Time.time > minTime & loadTime != 0 && fadeout == true)
        {
            Debug.Log("fadeout :" + fadeout);
            fade.alpha = Time.time - minTime;
            if (fade.alpha >= 1)
            {
                Debug.Log("ok change");
                fadein = true;
                fadeout = false;
                if (ServerStatus == "Online")
                {
                    NetworkManager.SetActive(true);
                }
                FacebookMaster.SetActive(true);
                JoinGameCanvas.gameObject.SetActive(true);
                

            }
        }
    }

}
