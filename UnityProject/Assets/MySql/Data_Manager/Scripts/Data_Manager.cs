using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Manager : MonoBehaviour
{
    private static Data_Manager instance;
    public static Data_Manager Instance { get { return instance; } }
    public Text Id;
    public Text UserId;
    public Text UserName;
    public Text UserPic;
    public Text UserToken;
    public Text UserPosX;
    public Text UserPosY;
    public Text UserPosZ;
    public Text UserLevel;
    public Text UserCurrency;
    public Text UserExpierance;
    public Text UserHealth;
    public Text UserPower;
    public Text UserGpsX;
    public Text UserGpsY;
    public Text UserGpsZ;
    public Text UserVungleApi;
    public Text UserAdcolonyApi;
    public Text UserAdcolonyZone;
    public Text UserRotX;
    public Text UserRotY;
    public Text UserRotZ;


    private void Awake()
    {
        instance = this;
    }


    public void SetId(string set)
    {
        PlayerPrefs.SetString("Id", set);
        Id.text = set;
    }

    public string GetId()
    {
        return Id.text;
    }

    public void SetUserId(string set)
    {
        PlayerPrefs.SetString("UserId", set);
        UserId.text = set;
    }

    public string GetUserId()
    {
        return UserId.text;
    }


    public void SetUserName(string set)
    {
        PlayerPrefs.SetString("UserName", set);
        UserName.text = set;
    }

    public string GetUserName()
    {
        return UserName.text;
    }

    public void SetUserPic(string set)
    {
        PlayerPrefs.SetString("UserPic", set);
        UserPic.text = set;
    }

    public string GetUserPic()
    {
        return UserPic.text;
    }


    public void SetUserToken(string set)
    {
        UserToken.text = set;
    }


    public string GetUserToken()
    {
        return UserToken.text;
    }


    public void SetUserPosX(string set)
    {
        UserPosX.text = set;
    }

    public string GetUserPosX()
    {
        return UserPosX.text;
    }

    public void SetUserPosY(string set)
    {
        UserPosY.text = set;
    }

    public string GetUserPosY()
    {
        return UserPosY.text;
    }

    public void SetUserPosZ(string set)
    {
        UserPosZ.text = set;
    }

    public string GetUserPosZ()
    {
        return UserPosZ.text;
    }

    public void SetUserLevel(string set)
    {

        UserLevel.text = set;
    }

    public string GetUserLevel()
    {
        return UserLevel.text;
    }

    public void SetUserCurrency(string set)
    {

        UserCurrency.text = set;
    }

    public string GetUserCurrency()
    {
        return UserCurrency.text;
    }

    public void SetUserExpierance(string set)
    {

        UserExpierance.text = set;
    }

    public string GetUserExpierance()
    {
        return UserExpierance.text;
    }

    public void SetUserHealth(string set)
    {
        UserHealth.text = set;
    }

    public string GetUserHealth()
    {
        return UserHealth.text;
    }

    public void SetUserPower(string set)
    {
        UserPower.text = set;
    }

    public string GetUserPower()
    {
        return UserPower.text;
    }

    public void SetUserGpsX(string set)
    {
        UserGpsX.text = set;
    }

    public string GetUserGpsX()
    {
        return UserGpsX.text;
    }

    public void SetUserGpsY(string set)
    {
        UserGpsY.text = set;
    }

    public string GetUserGpsY()
    {
        return UserGpsY.text;
    }

    public void SetUserGpsZ(string set)
    {
        UserGpsZ.text = set;
    }

    public string GetUserGpsZ()
    {
        return UserGpsZ.text;
    }

    public void SetUserVungleApi(string set)
    {
        PlayerPrefs.SetString("VungleApi", set);
        UserVungleApi.text = set;
    }

    public string GetUserVungleApi()
    {
        return UserVungleApi.text;
    }

    public void SetUserAdcolonyApi(string set)
    {
        PlayerPrefs.SetString("AdcolonyApi", set);
        UserAdcolonyApi.text = set;
    }

    public string GetUserAdcolonyApi()
    {
        return UserAdcolonyApi.text;
    }

    public void SetUserAdcolonyZone(string set)
    {
        PlayerPrefs.SetString("AdcolonyZone", set);
        UserAdcolonyZone.text = set;
    }


    public string GetUserAdcolonyZone()
    {
        return UserAdcolonyZone.text;
    }

    public void SetUserRotX(string set)
    {
        UserRotX.text = set;
    }

    public string GetUserRotX()
    {
        return UserRotX.text;
    }

    public void SetUserRotY(string set)
    {
        UserRotY.text = set;
    }

    public string GetUserRotY()
    {
        return UserRotY.text;
    }

    public void SetUserRotZ(string set)
    {
        UserRotZ.text = set;
    }

    public string GetUserRotZ()
    {
        return UserRotZ.text;
    }

}
