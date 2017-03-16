using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour 
{
	private static AdsManager instance;
	public static AdsManager Instance{get{return instance; }}
    public Text Debugtext;
    private void Start()
	{
		instance = this;
	}
    public void adcolonybtn()
	{
        Debugtext.text = "clicked adcolony";
        PlayAdcolonyAds.Instance.PlayAd();
	}

	public void vunglebtn()
	{
		Debug.Log ("clicked vungle");
		PlayVungleAds.Instance.PlayAd();
	}
	public void unitybtn()
	{
		Debug.Log ("clicked unity");
		PlayUnityAds.Instance.PlayAd();
	}
}
