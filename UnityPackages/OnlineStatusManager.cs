using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

using System;
using System.Timers;
using System.Threading;

public class OnlineStatusManager : MonoBehaviour
{
    public GameObject LoadMainSystem;
    public Text Message;
    public GameObject BG;
    public string ServerUrl = "http://www.projectclickthrough.com";

    Thread NewTimeThread;
    private bool ServerStatus = false;
    private void Awake()
    {
        CheckServerStatus(ServerUrl);
        
    }
    

    private void Update()
    {
        if(ServerStatus == false)
        {
            Message.text = "Checking Service Offline";
            BG.SetActive(true);
            LoadMainSystem.SetActive(false);
        }
        else
        {
            Message.text = "Checking Service Online";
            BG.SetActive(false);
            LoadMainSystem.SetActive(true);
        }
    }

    private void SystemUpdate()
    {
        NewTimeThread = new Thread(Tick);
        NewTimeThread.Start();
    }

    void Tick()
    {
        
        Debug.Log("Tick: " );
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        for(int i = 0; i<1000; i++)
        {
            Thread.Sleep(4);
        }
        sw.Stop();
        Debug.Log("Tick: "+sw.ElapsedMilliseconds/1000f);
        CheckServerStatus(ServerUrl);
    }

    private bool CheckServerStatus(string url)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Timeout = 3000;
            request.AllowAutoRedirect = false;
            request.Method = "HEAD";

            using (var responce = request.GetResponse())
            {
                Debug.Log("WE ARE ONLINE EVERYTHING IS OK");
                SystemUpdate();
                ServerStatus = true;
                return true;
            }
                // if its online everything is good
               
        }
        catch
        {
            // this is not oneline
           
            Debug.Log("WE ARE NOT ONLINE NO INTERNET ??? SERVICE OFFLINE ???");
            SystemUpdate();
            ServerStatus = false;
            return false;
        }

        
    }



}
