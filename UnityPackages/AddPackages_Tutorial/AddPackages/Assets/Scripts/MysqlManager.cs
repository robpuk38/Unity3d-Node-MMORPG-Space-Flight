using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysqlManager : MonoBehaviour {

    private static MysqlManager instance;
    public static MysqlManager Instance {get{return instance;}}

    private IEnumerator getthedata;
    private IEnumerator postthedata;
    private IEnumerator savethedata;
    private string AppKey = "appidkeyiswhatwesayitis";

    private void Awake()
    {
        instance = this;
    }

    public void GetUsersData(string UserId, string UserAccessToken)
    {
        getthedata = GetUserData(UserId, UserAccessToken);
        StartCoroutine(getthedata);
    }

    private IEnumerator GetUserData(string UserId, string UserAccessToken)
    {
        
        WWW getData = new WWW("http://www.projectclickthrough.com/server/getusersdata.php?UserId="+UserId+"&UserAccessToken="+UserAccessToken+ "&AppKey="+AppKey);
        yield return getData;

        if(getData.isDone)
        {
            string GetTheData = getData.text;

            string[] aData = GetTheData.Split('|');
            for (int i = 0; i < aData.Length - 1; i++)
            {
                if(aData[i] == "Id")
                {
                    DataManager.Instance.SetId(aData[i+1]);
                    Debug.Log("Id: " + aData[i + 1]);
                }
                if (aData[i] == "UserId")
                {
                    DataManager.Instance.SetUserId(aData[i + 1]);
                }
                if (aData[i] == "UserName")
                {
                    DataManager.Instance.SetUserName(aData[i + 1]);
                }
                if (aData[i] == "UserPic")
                {
                    DataManager.Instance.SetUserPic(aData[i + 1]);
                }
                if (aData[i] == "UserFirstName")
                {
                    DataManager.Instance.SetUserFirstName(aData[i + 1]);
                }
                if (aData[i] == "UserLastName")
                {
                    DataManager.Instance.SetUserLastName(aData[i + 1]);
                }
                if (aData[i] == "UserState")
                {
                    DataManager.Instance.SetUserState(aData[i + 1]);
                }
                if (aData[i] == "UserAccess")
                {
                    DataManager.Instance.SetUserAccess(aData[i + 1]);
                }

                if (aData[i] == "UserCredits")
                {
                    DataManager.Instance.SetUserCredits(aData[i + 1]);
                }
                if (aData[i] == "UserLevel")
                {
                    DataManager.Instance.SetUserLevel(aData[i + 1]);
                }
                if (aData[i] == "UserMana")
                {
                    DataManager.Instance.SetUserMana(aData[i + 1]);
                }
                if (aData[i] == "UserHealth")
                {
                    DataManager.Instance.SetUserHealth(aData[i + 1]);
                }
                if (aData[i] == "UserExp")
                {
                    DataManager.Instance.SetUserExp(aData[i + 1]);
                }

                if (aData[i] == "Error")
                {
                    // there is no user found lets create a new user
                    Debug.Log("Data Manager Error No User");
                   // FacebookManager.Instance.NoUserFound();
                    yield break;
                }
            }
        }

        FacebookManager.Instance.MemoryData();

        yield return null;
    }

    public void PostUsersData(string UserId, string UserPic, string UserAccessToken,string UserName, string UserFirstName, string UserLastName, string UserState)
    {
        postthedata = PostUserData(UserId, UserPic,  UserAccessToken,  UserName,  UserFirstName,  UserLastName, UserState);
        StartCoroutine(postthedata);
    }

    private IEnumerator PostUserData(string UserId, string UserPic, string UserAccessToken, string UserName, string UserFirstName, string UserLastName, string UserState)
    {

        
          WWWForm postData = new WWWForm();
          postData.AddField("UserId", UserId);
          postData.AddField("UserPic", UserPic);
          postData.AddField("UserAccessToken", UserAccessToken);
          postData.AddField("UserName", UserName);
          postData.AddField("UserFirstName", UserFirstName);
          postData.AddField("UserLastName", UserLastName);
          postData.AddField("UserState", UserState);

          WWW connection = new WWW("http://www.projectclickthrough.com/server/postusersdata.php", postData);
          yield return connection;

          if(connection.isDone)
          {

              Debug.Log("WE HAVE INSERTED THE DATA");
              GetUsersData(UserId, UserAccessToken);
          }

        yield return null;
    }



    public void SaveUsersData(string UserId, string UserAccessToken, string UserCredits, string UserLevel , string UserMana, string UserHealth, string UserExp,string UserState)
    {
        savethedata = SaveUserData(UserId, UserAccessToken, UserCredits,  UserLevel,  UserMana,  UserHealth, UserExp, UserState);
        StartCoroutine(savethedata);
    }

    private IEnumerator SaveUserData(string UserId, string UserAccessToken, string UserCredits, string UserLevel, string UserMana, string UserHealth, string UserExp,string UserState)
    {
       
        WWWForm postData = new WWWForm();
        postData.AddField("UserId", UserId);
        postData.AddField("UserAccessToken", UserAccessToken);
        postData.AddField("UserCredits", UserCredits);
        postData.AddField("UserLevel", UserLevel);
        postData.AddField("UserMana", UserMana);
        postData.AddField("UserHealth", UserHealth);
        postData.AddField("UserExp", UserExp);
        postData.AddField("UserState", UserState);

        WWW connection = new WWW("http://www.projectclickthrough.com/server/saveusersdata.php", postData);
        yield return connection;

        if (connection.isDone)
        {

           // Debug.Log("WE HAVE SAVED THE DATA");

            //todo is user is logging out dont do this.
          //  if (FacebookManager.Instance.hasLogout != true)
           // {
              //  GetUsersData(UserId, UserAccessToken);
          //  }

        }

        yield return null;

    }
}
