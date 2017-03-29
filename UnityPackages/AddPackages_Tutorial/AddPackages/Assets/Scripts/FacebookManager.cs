using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour
{

    private static FacebookManager instance;
    public static FacebookManager Instance { get { return instance; } }

    public GameObject FacebookLoginCanvas;
    public GameObject FacebookLogoutCanvas;
    public GameObject VungleCanvas;
    public GameObject AdcolonyCanvas;

    public Text UserId;
    public Text UserName;
    public Image UserPic;
    public GameObject UserPics;
    public Text ErrorMessage;
    public Text UserCredits;
    public Text UserLevel;


    private IEnumerator getuserspic;


    string fisrtname;
    string lastname;
    private bool isloaded = false;
    private bool hasloaded = false;
    public bool hasLogout = false;



    private void Awake()
    {
        instance = this;
        FacebookLoginCanvas.SetActive(true);
        FacebookLogoutCanvas.SetActive(false);
        VungleCanvas.SetActive(false);
        AdcolonyCanvas.SetActive(false);

        if (!FB.IsInitialized)
        {
            FB.Init();
        }

        ErrorMessage.text = "";


    }

    public void FacebookLogin()
    {
        //PlayerPrefs.DeleteAll();

        LoadingManager.Instance.Fadein(false);
        loadingtime = 0;

        if (FB.IsLoggedIn)
        {
            Debug.Log("We are alreay login");
            LoginStatusMemory();
            return;
        }
        else
        {
            LoginStatusMemory();
            return;
        }

        
    }

    private void LoginStatusMemory()
    {
        if (DataManager.Instance.GetUserId() != null
        && DataManager.Instance.GetUserId() != ""
        && DataManager.Instance.GetUserId() != "USERID"
        && DataManager.Instance.GetUserAccessToken() != null
        && DataManager.Instance.GetUserAccessToken() != ""
        && DataManager.Instance.GetUserAccessToken() != "USERACCESSTOKEN"
       )
        {
            // if we have already login before lets never do it again. 
            Debug.Log("We have already login before");
            MysqlManager.Instance.GetUsersData(DataManager.Instance.GetUserId(), DataManager.Instance.GetUserAccessToken());



            return;
        }
        else
        {
            if (!FB.IsLoggedIn)
            {
                List<string> perm = new List<string>();
                perm.Add("public_profile");
                FB.LogInWithReadPermissions(permissions: perm, callback: OnLogin);
            }
        }
    }

    int loadingtime = 0;
    int deloadingtime = 0;
    private void Update()
    {
        if (isloaded == true)
        {
            loadingtime++;
        }
        if (isloaded == false)
        {
            deloadingtime++;
        }
        if (loadingtime > 200 && isloaded == true && hasloaded == false)
        {
            LoadingManager.Instance.FadeOut();
            FacebookLoginCanvas.SetActive(false);
            FacebookLogoutCanvas.SetActive(true);
            VungleCanvas.SetActive(true);
            AdcolonyCanvas.SetActive(true);
            hasloaded = true;
            loadingtime = 0;
        }

        if (deloadingtime > 200 && isloaded == false && hasloaded == false && hasLogout == true)
        {
            LoadingManager.Instance.FadeOut();
            FacebookLoginCanvas.SetActive(true);
            FacebookLogoutCanvas.SetActive(false);
            VungleCanvas.SetActive(false);
            AdcolonyCanvas.SetActive(false);
            deloadingtime = 0;
            hasLogout = false;
        }
    }

    public void MemoryData()
    {


        UserId.text = DataManager.Instance.GetUserId();

        UserName.text = DataManager.Instance.GetUserName();
        UserCredits.text = DataManager.Instance.GetUserCredits();
        UserLevel.text = DataManager.Instance.GetUserLevel();

        new2dpicture(UserPics, DataManager.Instance.GetUserPic());

        DataManager.Instance.SetUserState("1");
        DataManager.Instance.SaveUsersData();

    }

    public void NoUserFound()
    {
        if (DataManager.Instance.GetUserPic() != null && DataManager.Instance.GetUserPic() != "" && DataManager.Instance.GetUserPic() != "USERPIC")
        {


            MysqlManager.Instance.PostUsersData(DataManager.Instance.GetUserId(),
               DataManager.Instance.GetUserPic(),
               DataManager.Instance.GetUserAccessToken(),
               DataManager.Instance.GetUserName(),
               DataManager.Instance.GetUserFirstName(),
               DataManager.Instance.GetUserLastName(),
               DataManager.Instance.GetUserState());
            Debug.Log("No User Was Found Inserting Now");
            return;
        }
        else
        {
            Debug.Log("No User Was Found Else Error");
            return;
        }
    }

    IEnumerator loadUsersPic(GameObject go, string url)
    {
       

        if (url.ToString() != null && url.ToString() != "" && url.ToString() != "USERPIC")
        {
            Texture2D temp = new Texture2D(0, 0);
            WWW www = new WWW(url);
            yield return www;

            temp = www.texture;
            Sprite sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));
            Transform themb = go.transform;
            themb.GetComponent<Image>().sprite = sprite;
            isloaded = true;

        }

    }


    private void new2dpicture(GameObject go, string url)
    {
        getuserspic = loadUsersPic(go, url);
        StartCoroutine(getuserspic);
    }


    private void OnLogin(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Successful Login");
            //this is a success
            AccessToken token = AccessToken.CurrentAccessToken;



            DataManager.Instance.SetUserAccessToken(token.TokenString);
            DataManager.Instance.SetUserState("1");
            GetUsersData(token);
            ErrorMessage.text = "";

        }
        else
        {
            //we had some error

            Debug.Log("Failed Login");
            ErrorMessage.text = "Error Facebook Login!";
        }
    }

    private void GetUsersData(AccessToken token)
    {
        FB.API("/me?fields=id", HttpMethod.GET, DisplayUsersId);
        FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsersFirstName);
        FB.API("/me?fields=last_name", HttpMethod.GET, DisplayUsersLastName);
        FB.API("/me/picture?type=square&height=200&width=200", HttpMethod.GET, DisplayUsersPic);

    }

    private void DisplayUsersPic(IGraphResult results)
    {
        if (results.Texture != null)
        {
            // Successfull Picture grab
            UserPic.sprite = Sprite.Create(results.Texture, new Rect(0, 0, 200, 200), new Vector2());
            UserName.text = fisrtname + " " + lastname;
            DataManager.Instance.SetUserName(UserName.text);
            ErrorMessage.text = "";
            string userPicture = "https://graph.facebook.com/" + UserId.text + "/picture?width=200";

            DataManager.Instance.SetUserPic(userPicture);
            isloaded = true;
            NoUserFound();
        }
        else
        {
            //we had a picture error

            ErrorMessage.text = "Error Facebook Picture!";
        }
    }
    private void DisplayUsersFirstName(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
            fisrtname = results.ResultDictionary["first_name"].ToString();
            ErrorMessage.text = "";
            DataManager.Instance.SetUserFirstName(fisrtname);

        }
        else
        {
            //everything is not ok;

            ErrorMessage.text = "Error Facebook UserName!";
        }
    }

    private void DisplayUsersLastName(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
            lastname = results.ResultDictionary["last_name"].ToString();
            ErrorMessage.text = "";

            DataManager.Instance.SetUserLastName(lastname);
        }
        else
        {
            //everything is not ok;

            ErrorMessage.text = "Error Facebook UserName!";
        }
    }

    private void DisplayUsersId(IResult results)
    {
        if (results.Error == null)
        {
            //ever thing is ok 
            UserId.text = results.ResultDictionary["id"].ToString();
            ErrorMessage.text = "";
            DataManager.Instance.SetUserId(UserId.text);

        }
        else
        {
            //everything is not ok;

            ErrorMessage.text = "Error Facebook UserId!";
        }
    }

    public void FacebookLogout()
    {

        isloaded = false;
        hasloaded = false;
        hasLogout = true;
        deloadingtime = 0;
        LoadingManager.Instance.Fadein(false);
        DataManager.Instance.SetUserState("0");
        DataManager.Instance.SaveUsersData();
    }




}
