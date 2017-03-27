using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour {

    private static LoadingManager instance;
    public static LoadingManager Instance { get { return instance; } }

    public CanvasGroup BG_Fade;

    private bool FadeStatus = false;

    public bool smoothStatus = false;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        FadeOut();
    }

    private void Update()
    {
        if(BG_Fade.alpha >= 1 && FadeStatus == true && smoothStatus == true)
        {
            FadeOut();
            smoothStatus = false;
        }
    }

    public void Fadein(bool t)
    {
        smoothStatus = t;
        FadeStatus = true;
        StartCoroutine(Fade());
    }

    public void FadeOut()
    {
        FadeStatus = false;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {

        BG_Fade.interactable = true;
        BG_Fade.blocksRaycasts = true;

        while(BG_Fade.alpha > 0 && FadeStatus == false)
        {
            BG_Fade.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        while(BG_Fade.alpha < 1 && FadeStatus == true)
        {
            BG_Fade.alpha += Time.deltaTime / 2;
            yield return null;

        }

        BG_Fade.interactable = false;
        BG_Fade.blocksRaycasts = false;


    }
}
