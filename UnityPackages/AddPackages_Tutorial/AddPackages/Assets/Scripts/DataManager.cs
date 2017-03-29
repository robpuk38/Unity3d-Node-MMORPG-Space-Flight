using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour {

    private static DataManager instance;
    public static DataManager Instance {get { return instance; } }

    public Text Id;
    public Text UserId;
    public Text UserName;
    public Text UserPic;
    public Text UserFirstName;
    public Text UserLastName;
    public Text UserAccessToken;
    public Text UserState;
    public Text UserAccess;
    public Text UserCredits;
    public Text UserLevel;
    public Text UserMana;
    public Text UserHealth;
    public Text UserExp;


    private void Awake()
    {
        instance = this;
    }
    public void SetId(string set)
    {
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
        UserId.text = PlayerPrefs.GetString("UserId");
        return UserId.text;
    }

    public void SetUserName(string set)
    {
       
        UserName.text = set;
    }

    public string GetUserName()
    {
        return UserName.text;
    }


    public void SetUserPic(string set)
    {
        
        UserPic.text = set;
    }

    public string GetUserPic()
    {
        return UserPic.text;
    }

    public void SetUserFirstName(string set)
    {
       
        UserFirstName.text = set;
    }

    public string GetUserFirstName()
    {
        return UserFirstName.text;
    }

    public void SetUserLastName(string set)
    {
        
        UserLastName.text = set;
    }

    public string GetUserLastName()
    {
        return UserLastName.text;
    }

    public void SetUserAccessToken(string set)
    {
        PlayerPrefs.SetString("UserAccessToken", set);
        UserAccessToken.text = set;
    }

    public string GetUserAccessToken()
    {
        UserAccessToken.text = PlayerPrefs.GetString("UserAccessToken");
        return UserAccessToken.text;
    }

    public void SetUserState(string set)
    {
        
        UserState.text = set;
    }

    public string GetUserState()
    {
        return UserState.text;
    }

    public void SetUserAccess(string set)
    {
       
        UserAccess.text = set;
    }

    public string GetUserAccess()
    {
        return UserAccess.text;
    }

    public void SetUserCredits(string set)
    {

        UserCredits.text = set;
        
    }

    public string GetUserCredits()
    {
        return UserCredits.text;
    }

    public void SetUserLevel(string set)
    {

        UserLevel.text = set;
       
    }

    public string GetUserLevel()
    {
        return UserLevel.text;
    }

    public void SetUserMana(string set)
    {

        UserMana.text = set;
       
    }

    public string GetUserMana()
    {
        return UserMana.text;
    }

    public void SetUserHealth(string set)
    {

        UserHealth.text = set;
       
    }

    public string GetUserHealth()
    {
        return UserHealth.text;
    }

    public void SetUserExp(string set)
    {

        UserExp.text = set;
    }

    public string GetUserExp()
    {
        return UserExp.text;
    }

    public void SaveUsersData()
    {
        MysqlManager.Instance.SaveUsersData(this.GetUserId(), 
            this.GetUserAccessToken(), 
            this.GetUserCredits(), 
            this.GetUserLevel(), 
            this.GetUserMana(), 
            this.GetUserHealth(), 
            this.GetUserExp(),
            this.GetUserState());
    }



}
