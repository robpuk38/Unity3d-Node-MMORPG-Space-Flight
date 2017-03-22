using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MysqlManager : MonoBehaviour
{

    private static MysqlManager instance;
    public static MysqlManager Instance { get { return instance; } }
    private IEnumerator getthedata;
    private IEnumerator postthedata;
    public string ServerDomainAddress = "www.projectclickthrough.com";


    private void Start()
    {
        instance = this;
    }


    public void GetUsersData(string UserId)
    {
        getthedata = GetUserData(UserId);
        StartCoroutine(getthedata);
    }



    private IEnumerator GetUserData(string UserId)
    {
        WWW getData = new WWW("http://" + ServerDomainAddress + "/server/get.php?UserId=" + UserId);
        yield return getData;



        if (getData.isDone)
        {
            string GetTheData = getData.text;

            string[] aData = GetTheData.Split('|');
            for (int i = 0; i < aData.Length - 1; i++)
            {

                if (aData[i] == "Id")
                {
                    //Debug.Log ("Id: " + aData [i + 1]);
                    Data_Manager.Instance.SetId(aData[i + 1]);
                }

                if (aData[i] == "UserId")
                {
                    //Debug.Log ("UserId: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserId(aData[i + 1]);
                }

                if (aData[i] == "UserName")
                {
                    //Debug.Log ("UserName: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserName(aData[i + 1]);
                }

                if (aData[i] == "UserPic")
                {
                    //Debug.Log ("UserPic: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserPic(aData[i + 1]);

                    FacebookManager.Instance.new2dpicture(FacebookManager.Instance.profilePic, aData[i + 1]);

                }

                if (aData[i] == "UserToken")
                {
                    //Debug.Log ("UserToken: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserToken(aData[i + 1]);
                }

                if (aData[i] == "UserPosX")
                {
                    //Debug.Log ("UserPosX: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserPosX(aData[i + 1]);
                }

                if (aData[i] == "UserPosY")
                {
                    //Debug.Log ("UserPosY: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserPosY(aData[i + 1]);
                }

                if (aData[i] == "UserPosZ")
                {
                    //Debug.Log ("UserPosZ: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserPosZ(aData[i + 1]);
                }

                if (aData[i] == "UserLevel")
                {
                    //Debug.Log ("UserLevel: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserLevel(aData[i + 1]);
                }

                if (aData[i] == "UserCurrency")
                {
                    //Debug.Log ("UserCurrency: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserCurrency(aData[i + 1]);
                }

                if (aData[i] == "UserExpierance")
                {
                    //Debug.Log ("UserExpierance: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserExpierance(aData[i + 1]);
                }

                if (aData[i] == "UserHealth")
                {
                    //Debug.Log ("UserHealth: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserHealth(aData[i + 1]);
                }

                if (aData[i] == "UserPower")
                {
                    //Debug.Log ("UserPower: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserPower(aData[i + 1]);
                }

                if (aData[i] == "UserGpsX")
                {
                    //Debug.Log ("UserGpsX: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserGpsX(aData[i + 1]);
                }

                if (aData[i] == "UserGpsY")
                {
                    //Debug.Log ("UserGpsY: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserGpsY(aData[i + 1]);
                }

                if (aData[i] == "UserGpsZ")
                {
                    //Debug.Log ("UserGpsZ: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserGpsZ(aData[i + 1]);
                }

                if (aData[i] == "UserVungleApi")
                {
                    //Debug.Log ("UserVungleApi: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserVungleApi(aData[i + 1]);
                }

                if (aData[i] == "UserAdcolonyApi")
                {
                    //Debug.Log ("UserAdcolonyApi: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserAdcolonyApi(aData[i + 1]);
                }

                if (aData[i] == "UserAdcolonyZone")
                {
                    //Debug.Log ("UserAdcolonyZone: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserAdcolonyZone(aData[i + 1]);
                }

                if (aData[i] == "UserRotX")
                {
                   // Debug.Log ("UserRotX: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserRotX(aData[i + 1]);
                }

                if (aData[i] == "UserRotY")
                {
                  //  Debug.Log ("UserRotY: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserRotY(aData[i + 1]);
                }

                if (aData[i] == "UserRotZ")
                {
                   // Debug.Log ("UserRotZ: " + aData [i + 1]);
                    Data_Manager.Instance.SetUserRotZ(aData[i + 1]);
                }

                if (aData[i] == "Error")
                {
                    //Debug.Log ("Error: " + aData [i + 1]);
                    FacebookManager.Instance.NoUserFound();
                    yield break;
                }

            }
        }
        else
        {
            Debug.Log("GetUserData Connection Error");
        }



        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.JoinGame();
        }

    }



    public void PostUsersData(string UserId, string UserName, string UserToken, float UserPosX, float UserPosY, float UserPosZ)
    {
       // Debug.Log("Posting Users Data: " + UserId);
       // Debug.Log("Posting Users Data: " + UserName);
       // Debug.Log("Posting Users Data: " + UserToken);
      //  Debug.Log("Posting Users Data: " + UserPosX);
      //  Debug.Log("Posting Users Data: " + UserPosY);
       // Debug.Log("Posting Users Data: " + UserPosZ);
        postthedata = PostUserData(UserId, UserName, UserToken, UserPosX, UserPosY, UserPosZ);
        StartCoroutine(postthedata);
    }


    private IEnumerator PostUserData(string UserId, string UserName, string UserToken, float UserPosX, float UserPosY, float UserPosZ)
    {

        WWWForm postdata = new WWWForm();
        string MatchedAppKey = "jZuHiJtYS";
        postdata.AddField("AppKey", MatchedAppKey);
        postdata.AddField("UserId", UserId);
        postdata.AddField("UserName", UserName);
        postdata.AddField("UserToken", UserToken);
        postdata.AddField("UserPosX", UserPosX.ToString());
        postdata.AddField("UserPosY", UserPosY.ToString());
        postdata.AddField("UserPosZ", UserPosZ.ToString());


        WWW connection = new WWW("http://" + ServerDomainAddress + "/server/post.php", postdata);
        yield return connection;


        if (connection.isDone)
        {
           // Debug.Log("Insertation Has Finished");
            GetUsersData(UserId);


        }




    }



    public void SaveUsersData()
    {
        Debug.Log("SAVEING PLAYERS DETALS TO THE DATABASE NOW");
    }


}
