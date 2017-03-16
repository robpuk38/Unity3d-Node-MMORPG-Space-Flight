using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayAdcolonyAds : MonoBehaviour {

    private static PlayAdcolonyAds instance;
    public static PlayAdcolonyAds Instance { get { return instance; } }

    public Text Debugtext;

        // AdColony values
       
        AdColony.InterstitialAd Ad = null;
        public string appId;
        public string zoneId;
       

        private int requestTime = 0;


    private void Start()
    {
        instance = this;

    
   
        

            // ----- AdColony Ads -----

            AdColony.Ads.OnConfigurationCompleted += (List<AdColony.Zone> zones_) => 
            {
                Debugtext.text = "AdColony.Ads.OnConfigurationCompleted called";

                if (zones_ == null || zones_.Count <= 0) {
                    // Show the configure asteroid again.
                   
                }
                else
                {
                 

                    // Successfully configured... show the request ad asteroid.
                 
                }
            };

            AdColony.Ads.OnRequestInterstitial += (AdColony.InterstitialAd ad_) => 
            {
                Debugtext.text = "AdColony.Ads.OnRequestInterstitial called";
                Ad = ad_;
            };

            AdColony.Ads.OnRequestInterstitialFailed += () => 
            {
                Debugtext.text = "AdColony.Ads.OnRequestInterstitialFailed called";
            };

            AdColony.Ads.OnOpened += (AdColony.InterstitialAd ad_) => 
            {
                Debugtext.text = "AdColony.Ads.OnOpened called";
                RequestAd();
            };

            AdColony.Ads.OnClosed += (AdColony.InterstitialAd ad_) => 
            {
                Debugtext.text = "AdColony.Ads.OnClosed called, expired: " + ad_.Expired;
                
            };

            AdColony.Ads.OnExpiring += (AdColony.InterstitialAd ad_) => 
            {
                Debugtext.text = "AdColony.Ads.OnExpiring called";
                Ad = null;
            };

            AdColony.Ads.OnIAPOpportunity += (AdColony.InterstitialAd ad_, string iapProductId_, AdColony.AdsIAPEngagementType engagement_) => 
            {
                Debugtext.text = "AdColony.Ads.OnIAPOpportunity called";
            };

            AdColony.Ads.OnRewardGranted += (string zoneId, bool success, string name, int amount) => 
            {
                Debugtext.text = string.Format("AdColony.Ads.OnRewardGranted called\n\tzoneId: {0}\n\tsuccess: {1}\n\tname: {2}\n\tamount: {3}", zoneId, success, name, amount);
            };

            AdColony.Ads.OnCustomMessageReceived += (string type, string message) => 
            {
                Debugtext.text = string.Format("AdColony.Ads.OnCustomMessageReceived called\n\ttype: {0}\n\tmessage: {1}", type, message);
            };

        ConfigureAds();

        RequestAd ();

        }


        void Update() 
        {

            requestTime++;

            if(requestTime > 100)
            {
                //RequestAd ();
                requestTime =0;
            }

          
        }

        void ConfigureAds()
        {
        // Configure the AdColony SDK
        Debugtext.text = "**** Configure ADC SDK ****";

            // Set some test app options with metadata.
            AdColony.AppOptions appOptions = new AdColony.AppOptions();
            AdColony.Ads.Configure(appId, appOptions, zoneId);
        }

        void RequestAd() {
        
        // Request an ad.
        Debugtext.text = "**** Request Ad ****";

            AdColony.AdOptions adOptions = new AdColony.AdOptions();
            adOptions.ShowPrePopup = false;
            adOptions.ShowPostPopup = false;

            AdColony.Ads.RequestInterstitialAd(zoneId, adOptions);
        }



       
        
        public void PlayAd()
        {

           if (Ad != null)
            {
            Debugtext.text = "Adcolony Ad Is now Playing!";
                AdColony.Ads.ShowAd (Ad);

               
            }
            else 
            {
            Debugtext.text = "Adcolony Ad Is Not Playing Request?";
                RequestAd ();
            }

        }
}
