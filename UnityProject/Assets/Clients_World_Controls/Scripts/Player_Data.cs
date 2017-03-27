using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{



    public GameObject userID;
    public GameObject userName;
    public GameObject userLocation;

    public GameObject UserPic;
    public GameObject userGameposX;
    public GameObject userGameposY;
    public GameObject userGameposZ;

    private IEnumerator getthepic;

    public bool loadedonce = false;



    IEnumerator loadUserPic(GameObject go, string url)
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(url);
        yield return www;

        www.LoadImageIntoTexture(tex);
        go.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void new3dpicture(GameObject go, string url)
    {
        getthepic = loadUserPic(go, url);
        StartCoroutine(getthepic);
    }

    public void new3dtext(GameObject go, string t)
    {
        TextMesh textObject = go.GetComponent<TextMesh>();
        textObject.text = t;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("Player Data: " + Client.Instance.clientUserId);
        if (Data_Manager.Instance.GetUserId() != "" && loadedonce == false)
        {
            Debug.Log("Player Data: GOT IN" + Data_Manager.Instance.GetUserId());

            new3dtext(userID, Data_Manager.Instance.GetUserId());
            new3dtext(userName, Data_Manager.Instance.GetUserName());
            new3dtext(userLocation, Data_Manager.Instance.GetUserGpsX() + "," + Data_Manager.Instance.GetUserGpsY() + "," + Data_Manager.Instance.GetUserGpsZ());
            new3dpicture(UserPic, Data_Manager.Instance.GetUserPic());
            new3dtext(userGameposX, Data_Manager.Instance.GetUserPosX());
            new3dtext(userGameposY, Data_Manager.Instance.GetUserPosY());
            new3dtext(userGameposZ, Data_Manager.Instance.GetUserPosZ());
            loadedonce = true;

        }
    }
}
