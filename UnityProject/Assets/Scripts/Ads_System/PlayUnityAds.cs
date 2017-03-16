using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class PlayUnityAds : MonoBehaviour {

	private static PlayUnityAds instance;
	public static PlayUnityAds Instance{get{return instance; }}
	private void Awake()
	{
		instance = this;
	}
	public void PlayAd()
	{
		if (Advertisement.IsReady ()) 
		{
			Advertisement.Show ("rewardedVideo",new ShowOptions(){resultCallback = HandleAdResult});
		}
	}

	private void HandleAdResult(ShowResult result)
	{
		switch (result) 
		{
		case ShowResult.Finished:
			Debug.Log ("Player Gain + 5 gems");
			break;
		case ShowResult.Skipped:
			Debug.Log ("Player Skipped The ad");
			break;
		case ShowResult.Failed:
			Debug.Log ("Player Failed To Luanch The ad ");
			break;

			
		}
	}
}
