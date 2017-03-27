using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysqlManager : MonoBehaviour {

    private static MysqlManager instance;
    public static MysqlManager Instance {get{return instance;}}

    private IEnumerator getthedata;
    private IEnumerator postthedata;

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
        WWW getData = new WWW("http://www.projectclickthrough.com/server/getusersdata.php?UserId="+UserId+"&UserAccessToken="+UserAccessToken);
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


}
