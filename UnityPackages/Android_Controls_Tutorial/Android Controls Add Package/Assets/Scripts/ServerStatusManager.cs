using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Threading;

public class ServerStatusManager : MonoBehaviour {

    public GameObject LoadedSystem;
    public Text Message;
    public GameObject BG;

    public string ServerUrl = "http://www.projectclickthrough.com";

    Thread NewTimeThread;

    private bool ServerStatus = false;

    private void Awake()
    {
       

        SystemUpdate();
    }
    private void Update()
    {
        if(ServerStatus == false)
        {
            Message.text = "Server Offline";
            BG.SetActive(true);
            LoadedSystem.SetActive(false);
        }
        else
        {
            Message.text = "Server Online";
            BG.SetActive(false);
            LoadedSystem.SetActive(true);
        }
    }

    private void SystemUpdate()
    {
        NewTimeThread = new Thread(Tick);
        NewTimeThread.Start();
    }

    private void Tick()
    {
        Debug.Log("Tick Has Started");
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        for (int i= 0; i<1000; i++)
        {
            Thread.Sleep(10);
        }
        sw.Stop();
        Debug.Log("Tick Has Ended: "+sw.ElapsedMilliseconds/1000f);
        CheckServerStatus(ServerUrl);
    }

    private bool CheckServerStatus(string Url)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);
            request.Timeout = 3000;
            request.AllowAutoRedirect = false;
            request.Method = "HEAD";
            using (var responce = request.GetResponse())
            {
                ServerStatus = true;
                Debug.Log("The server is online and ok");
                SystemUpdate();
                return true;
            }
        }
        catch
        {
            ServerStatus = false;
            Debug.Log("The server is offline or no internet");
            SystemUpdate();
            return false;
        }

        
    }

}
