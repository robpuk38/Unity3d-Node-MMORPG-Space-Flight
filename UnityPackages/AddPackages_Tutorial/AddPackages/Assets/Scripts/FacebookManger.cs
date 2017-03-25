using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class FacebookManger : MonoBehaviour
{
    public GameObject FacebookLoginCanvas;
    public GameObject FacebookLogoutCanvas;
    public GameObject VungleCanvas;
    public GameObject AdcolonyCanvas;

    public Text UserId;
    public Text UserName;
    public Image UserPic;
    public GameObject UserPics;
    public Text ErrorMessage;
   

    private IEnumerator getuserspic;


    string fisrtname;
    string lastname;
    private bool isloaded = false;
    private bool hasloaded = false;



    private void Awake()
    {
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

        if (FB.IsLoggedIn)
        {
         Debug.Log("We are alreay login");
            
            
        

        if (PlayerPrefs.GetString("UserId") != null
        && PlayerPrefs.GetString("UserId") != ""
        && PlayerPrefs.GetString("UserPic") != null
        && PlayerPrefs.GetString("UserPic") != ""
        && PlayerPrefs.GetString("FirstName") != null
        && PlayerPrefs.GetString("FirstName") != ""
        && PlayerPrefs.GetString("LastName") != null
        && PlayerPrefs.GetString("LastName") != "")
        {
            // if we have already login before lets never do it again. 
            Debug.Log("We have already login before");
           
            MemoryData();

            return;
        }
        }



        if (!FB.IsLoggedIn)
        {
            List<string> perm = new List<string>();
            perm.Add("public_profile");
            FB.LogInWithReadPermissions(permissions: perm, callback: OnLogin);
        }
    }

    int loadingtime = 0;
    private void Update()
    {
        loadingtime++;
        if (loadingtime > 200 && isloaded == true && hasloaded == false)
        {
            FacebookLoginCanvas.SetActive(false);
            FacebookLogoutCanvas.SetActive(true);
            VungleCanvas.SetActive(true);
            AdcolonyCanvas.SetActive(true);
            hasloaded = true;
        }
    }

    private void MemoryData()
    {
        UserId.text = PlayerPrefs.GetString("UserId");
        UserName.text = PlayerPrefs.GetString("FirstName") + " " + PlayerPrefs.GetString("LastName");

        new2dpicture(UserPics, PlayerPrefs.GetString("UserPic"));

    }

    IEnumerator loadUsersPic(GameObject go, string url)
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
            ErrorMessage.text = "";
            string userPicture = "https://graph.facebook.com/" + UserId.text + "/picture?width=200";
            PlayerPrefs.SetString("UserPic", userPicture);
            isloaded = true;
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
            PlayerPrefs.SetString("FirstName", fisrtname);
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
            PlayerPrefs.SetString("LastName", lastname);
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
            PlayerPrefs.SetString("UserId", UserId.text);
        }
        else
        {
            //everything is not ok;

            ErrorMessage.text = "Error Facebook UserId!";
        }
    }

    public void FacebookLogout()
    {
        FacebookLoginCanvas.SetActive(true);
        FacebookLogoutCanvas.SetActive(false);
        VungleCanvas.SetActive(false);
        AdcolonyCanvas.SetActive(false);
        isloaded = false;
        hasloaded = false;
    }




}
