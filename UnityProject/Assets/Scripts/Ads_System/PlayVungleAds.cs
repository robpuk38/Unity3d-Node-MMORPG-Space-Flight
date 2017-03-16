using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVungleAds : MonoBehaviour {

	private static PlayVungleAds instance;
	public static PlayVungleAds Instance{get{return instance; }}
	private void Start()
	{
		instance = this;
	}

	public void PlayAd()
	{
		Debug.Log ("Vungle Ad Is now Playing!");
	}
}
