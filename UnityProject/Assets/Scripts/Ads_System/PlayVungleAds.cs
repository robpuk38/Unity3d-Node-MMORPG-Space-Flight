using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayVungleAds : MonoBehaviour {

	private static PlayVungleAds instance;
	public static PlayVungleAds Instance{get{return instance; }}
    public Text Debugtext;
    public string appId;
    private void Awake()
	{
		instance = this;
        Vungle.init(appId, null,null);
        initializeEventHandlers();
    }


    private void initializeEventHandlers()
    {

        
        Vungle.onAdStartedEvent += () => {
            Debugtext.text = "Ad Started";

            if (Data_Manager.Instance != null)
            {
                int newcurrency;
                int.TryParse(Data_Manager.Instance.GetUserCurrency(), out newcurrency);
                Debug.Log(newcurrency);

                newcurrency = 20 + newcurrency;
                Data_Manager.Instance.SetUserCurrency(newcurrency.ToString());
                Debug.Log(newcurrency);
                Debug.Log(Data_Manager.Instance.GetUserCurrency());

            }
        };

     
        Vungle.onAdFinishedEvent += (args) => {
            Debugtext.text = "Ad Finished";
        };

     
        Vungle.adPlayableEvent += (adPlayable) => {
            Debugtext.text = "Ad's playable state has been changed! Now: " + adPlayable;
        };

        //Fired log event from sdk
        Vungle.onLogEvent += (log) => {
            Debugtext.text = "Log: " + log;
        };


    }

   

    public void PlayAd()
	{
        Debugtext.text = "Vungle Ad Is now Playing!";
        Dictionary<string, object> options = new Dictionary<string, object>();
        options["incentivized"] = true;
        Vungle.playAdWithOptions(options);
    }
}
