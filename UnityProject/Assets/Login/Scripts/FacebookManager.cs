using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{
    private static FacebookManager instance;
    public static FacebookManager Instance { get { return instance; } }
    public GameObject Join_Game_Canvas;
    public GameObject Facebook_Canvas;
    public GameObject Collected_Data_Canvas;
    public Text UserId;
    public Text UserName;
    public Text UserLast;
    public Text UserToken;
    public Text ConnectionMessage;
    public Image ProfilePic;
    public GameObject profilePic;
    private IEnumerator getthepic;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
            Join_Game_Canvas.SetActive(true);
            Facebook_Canvas.SetActive(false);
            Collected_Data_Canvas.SetActive(false);
        }
        else
        {
            FB.ActivateApp();

        }
        instance = this;
    }



    public void Login()
    {
        List<string> perm = new List<string>();
        perm.Add("public_profile");
        FB.LogInWithReadPermissions(permissions: perm, callback: OnLogin);
        AuidoManager.Instance.ButtonClicked();
        ConnectionMessage.text = "Connecting";
    }

    IEnumerator loadUserPic(GameObject go, string url)
    {
       /*Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(url);
        yield return www;

        www.LoadImageIntoTexture(tex);
        go.GetComponent<Renderer>().material.mainTexture = tex;*/



        Texture2D temp = new Texture2D(0, 0);
        WWW www = new WWW(url);
        yield return www;

        temp = www.texture;
        Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
        Transform thumb = go.transform;
        thumb.GetComponent<Image>().sprite = sprite;
    }

    public void new2dpicture(GameObject go, string url)
    {
        getthepic = loadUserPic(go, url);
        StartCoroutine(getthepic);
    }

    public void GetUsersData(AccessToken token)
    {
        //Debug.Log ("Get User Data");
        UserToken.text = token.TokenString;
        FB.API("/me?fields=id", HttpMethod.GET, DisplayUserId);
        FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
        FB.API("/me?fields=last_name", HttpMethod.GET, DisplayUserLast);
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayUserPic);
    }
    public string GetUserAccess()
    {
        return UserToken.text;
    }
    private void DisplayUserId(IResult result)
    {
        if (result.Error == null)
        {
            //Debug.Log ("DisplayUserId: Setting The Users Id Name String");
            UserId.text = result.ResultDictionary["id"].ToString();
        }
        else
        {
            ConnectionMessage.text = "Error:" + result.Error;
        }

    }

    public void NoUserFound()
    {
        //Debug.Log ("No User Was Found So We Will insert A new one here");
        MysqlManager.Instance.PostUsersData(GetUserId(), GetUserName(), GetUserAccess(), GPS.Instance.GetGpsX(), GPS.Instance.GetGpsY(), GPS.Instance.GetGpsZ());
    }

    public string GetUserId()
    {
        //Debug.Log ("GetUserId()" +UserId.text);
        return UserId.text;
    }



    private void DisplayUserName(IResult result)
    {
        if (result.Error == null)
        {
            //Debug.Log ("DisplayUserName: Setting The Users First Name String");
            UserName.text = result.ResultDictionary["first_name"].ToString();
        }
        else
        {
            ConnectionMessage.text = "Error:" + result.Error;
        }

    }

    public string GetUserName()
    {
        return UserName.text;
    }

    private void DisplayUserLast(IResult result)
    {
        if (result.Error == null)
        {
            //Debug.Log ("DisplayUserLast: Setting The Users Last Name String");
            UserLast.text = result.ResultDictionary["last_name"].ToString();
        }
        else
        {
            ConnectionMessage.text = "Error:" + result.Error;
        }

    }





    private void DisplayUserPic(IGraphResult result)
    {
        if (result.Texture != null)
        {

            //Debug.Log ("DisplayUserPic: Setting The Users Picture");
            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

            //send to mysql the data
            MysqlManager.Instance.GetUsersData(GetUserId());
        }
        else
        {
            ConnectionMessage.text = "Error:" + result.Error;
        }

    }

    public Sprite GetUserPicimage()
    {
        return ProfilePic.sprite;
    }

    private void OnLogin(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken token = AccessToken.CurrentAccessToken;

            //Debug.Log ("Login Success "+token.TokenString);	
            ConnectionMessage.text = "Getting Users Data";
            GetUsersData(token);
        }
        else
        {
            ConnectionMessage.text = "Login Failed";
        }

    }







    /*
	  public void Share()
	{
		//FB.ShareLink (contentTitle:"PCTRS",
			//contentURL:new System.Uri("http://www.projectclickthrough.com?refid="+userIdText.text.ToString()),
			//contentDescription:"Disription",callback:OnShare);
	}
	  private void OnShare(IShareResult results)
	{
		if (results.Cancelled || !string.IsNullOrEmpty (results.Error)) 
		{
			Debug.Log ("Share Link error" + results.Error);
		} 
		else if (!string.IsNullOrEmpty (results.PostId)) 
		{
			Debug.Log ("Share Link  postID" + results.PostId);
		} 
		else 
		{
			Debug.Log ("Share Success");
		}
	}*/

}
