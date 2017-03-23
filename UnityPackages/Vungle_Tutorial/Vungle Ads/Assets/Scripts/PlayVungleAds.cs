using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayVungleAds : MonoBehaviour
{


    public Text DebugText;

    public string AppId = "58d3030b64b3bf8742000250";

    Dictionary<string, object> options;


    private void Awake()
    {
        Init(AppId);
        
    }

    private void Start()
    {
        Init(AppId);
    }
    int Adtimer = 0;
    private void Update()
    {
        Adtimer++;
        if(Adtimer > 100)
        {
            RequestAd();
            Adtimer = 0;
        }
    }

    private void Init(string AppId)
    {
        Vungle.init(AppId, null, null);
        RequestAd();
    }

    private void RequestAd()
    {
       
        InitializeEventHandlers();
        options = new Dictionary<string, object>();
        options["incentivized"] = true;
    }

    private void InitializeEventHandlers()
    {
        Vungle.onAdStartedEvent += () => {
            DebugText.text = "On Ad Started";
        };

        Vungle.onAdFinishedEvent += (args) =>
        {
            DebugText.text = "On Ad Finished: "+ args.ToString();
        };

        Vungle.adPlayableEvent += (adPlayable) => {
            DebugText.text = "This ad is playable: " + adPlayable.ToString();
        };

        Vungle.onLogEvent += (log) => {
            DebugText.text = "This log: "+log.ToString();
        };
    }

    public void PlayAd()
    {
        DebugText.text = "THE BUTTON WAS CLICKED PLAY AD";
        
        Vungle.playAdWithOptions(options);

    }

}
