using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {

    // To show an ad:
    // 1. Application must first configure AdColony
    // 2. Application requests an InterstitialAd for a Zone

    public class Ads : MonoBehaviour {

        public static event Action<List<Zone>> OnConfigurationCompleted;

        public static event Action<InterstitialAd> OnRequestInterstitial;
        public static event Action OnRequestInterstitialFailed;

        // Params:
        // - type
        // - message
        public static event Action<string, string> OnCustomMessageReceived;

        public static event Action<InterstitialAd> OnOpened;
        public static event Action<InterstitialAd> OnClosed;
        public static event Action<InterstitialAd> OnExpiring;
        public static event Action<InterstitialAd> OnAudioStarted;
        public static event Action<InterstitialAd> OnAudioStopped;

        // Params
        // - ad
        // - iapProductId
        // - engagement
        public static event Action<InterstitialAd, string, AdsIAPEngagementType> OnIAPOpportunity;

        // Params:
        // - zoneId
        // - success
        // - name
        // - amount
        public static event Action<string, bool, string, int> OnRewardGranted;

        // ---------------------------------------------------------------------------

        public static void Configure(string appId, AppOptions options, params string[] zoneIds) {
            // Using SharedInstance to make sure the MonoBehaviour is instantiated
            if (SharedInstance._instance == null) {
                Debug.LogWarning(Constants.AdsMessageSDKUnavailable);
                return;
            }

            Debug.Log("ADS AdColony Configure() " + appId);

            SharedInstance._instance.Configure(appId, options, zoneIds);
            _initialized = true;
        }

        public static string GetSDKVersion() {
            if (IsInitialized()) {
                return SharedInstance._instance.GetSDKVersion();
            }
            return null;
        }

        // Asynchronously request an Interstitial Ad
        // see OnRequestInterstitial, OnRequestInterstitialFailed
        public static void RequestInterstitialAd(string zoneId, AdOptions adOptions) {
            if (IsInitialized()) {
                SharedInstance._instance.RequestInterstitialAd(zoneId, adOptions);
            }
        }

        public static Zone GetZone(string zoneId) {
            if (IsInitialized()) {
                return SharedInstance._instance.GetZone(zoneId);
            }
            return null;
        }

        public static string GetUserID() {
            if (IsInitialized()) {
                return SharedInstance._instance.GetUserID();
            }
            return null;
        }

        public static void SetAppOptions(AppOptions options) {
            if (IsInitialized()) {
                SharedInstance._instance.SetAppOptions(options);
            }
        }

        public static AppOptions GetAppOptions() {
            if (IsInitialized()) {
                return SharedInstance._instance.GetAppOptions();
            }
            return null;
        }

        public static void SendCustomMessage(string type, string content) {
            if (IsInitialized()) {
                SharedInstance._instance.SendCustomMessage(type, content);
            }
        }

        /// <summary>
        /// Reports IAPs within your application.
        /// </summary>
        /// Note that this API can be used to report standard IAPs as well as those triggered by AdColony’s IAP Promo (IAPP) advertisements.
        /// Leveraging this API will improve overall ad targeting for your application.
        /// @param transactionID An NSString representing the unique SKPaymentTransaction identifier for the IAP. Must be 128 chars or less.
        /// @param productID An NSString identifying the purchased product. Must be 128 chars or less.
        /// @param price (optional) An NSNumber indicating the total price of the items purchased.
        /// @param currencyCode (optional) An NSString indicating the real-world, three-letter ISO 4217 (e.g. USD) currency code of the transaction.
        public static void LogInAppPurchase(string transactionId, string productId, int purchasePriceMicro, string currencyCode) {
            if (IsInitialized()) {
                SharedInstance._instance.LogInAppPurchase(transactionId, productId, purchasePriceMicro, currencyCode);
            }
        }

        public static void ShowAd(InterstitialAd ad) {
            if (IsInitialized()) {
                SharedInstance._instance.ShowAd(ad);
            }
        }

        public static void CancelAd(InterstitialAd ad) {
            if (IsInitialized()) {
                SharedInstance._instance.CancelAd(ad);
            }
        }

        // ---------------------------------------------------------------------------

#region Internal Methods - do not call these

        private static bool IsSupportedOnCurrentPlatform() {
            // Using SharedInstance to make sure the MonoBehaviour is instantiated
            if (SharedInstance._instance == null) {
                return false;
            }
            return true;
        }

        private static bool IsInitialized() {
            if (!IsSupportedOnCurrentPlatform()) {
                return false;
            } else if (!_initialized) {
                Debug.LogError(Constants.AdsMessageNotInitialized);
                return false;
            }
            return true;
        }

        public static Ads SharedInstance {
            get {
                if (!_sharedInstance) {
                    _sharedInstance = (Ads)FindObjectOfType(typeof(Ads));
                }

                if (!_sharedInstance) {
                    GameObject singleton = new GameObject();
                    _sharedInstance = singleton.AddComponent<Ads>();
                    singleton.name = Constants.AdsManagerName;
                    DontDestroyOnLoad(singleton);

                    if (_sharedInstance._instance != null) {
                        Debug.LogWarning(Constants.AdsMessageAlreadyInitialized);
                        return null;
                    } else {
                        _sharedInstance._instance = null;
#if UNITY_EDITOR

#elif UNITY_ANDROID
                        _sharedInstance._instance = new AdsAndroid(singleton.name);
#elif UNITY_IOS
                        _sharedInstance._instance = new AdsIOS(singleton.name);
#elif UNITY_WP8

#elif UNITY_METRO

#endif
                    }
                }

                return _sharedInstance;
            }
        }

        void Awake() {
            if (gameObject == SharedInstance.gameObject) {
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        void Update() {
            if (_updateOnMainThreadActions.Count > 0) {
                System.Action action;
                do {
                    action = null;
                    lock (_updateOnMainThreadActionsLock) {
                        if (_updateOnMainThreadActions.Count > 0) {
                            action = _updateOnMainThreadActions.Dequeue();
                        }
                    }
                    if (action != null) {
                        action.Invoke();
                    }
                } while (action != null);
            }
        }

        public void EnqueueAction(System.Action action) {
            lock (_updateOnMainThreadActionsLock) {
                _updateOnMainThreadActions.Enqueue(action);
            }
        }

        public static void DestroyAd(String id) {
            if (IsInitialized()) {
                SharedInstance._instance.DestroyAd(id);
            }
        }

        public void _OnConfigure(string paramJson) {
            Debug.Log("_OnConfigure called");

            List<Zone> zoneList = new List<Zone>();
            ArrayList zoneJsonList = (AdColonyJson.Decode(paramJson) as ArrayList);
            foreach (string zoneJson in zoneJsonList) {
                Hashtable zoneValues = (AdColonyJson.Decode(zoneJson) as Hashtable);
                zoneList.Add(new Zone(zoneValues));
            }

            if (Ads.OnConfigurationCompleted != null) {
                Ads.OnConfigurationCompleted(zoneList);
            }
        }


        public void _OnRequestInterstitial(string paramJson) {
            Debug.Log("_OnRequestInterstitial called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnRequestInterstitial != null) {
                Ads.OnRequestInterstitial(ad);
            }
        }

        public void _OnRequestInterstitialFailed() {
            Debug.Log("_OnRequestInterstitialFailed called");
            if (Ads.OnRequestInterstitialFailed != null) {
                Ads.OnRequestInterstitialFailed();
            }
        }

        public void _OnOpened(string paramJson) {
            Debug.Log("_OnOpened called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnOpened != null) {
                Ads.OnOpened(ad);
            }
        }

        public void _OnClosed(string paramJson) {
            Debug.Log("_OnClosed called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnClosed != null) {
                Ads.OnClosed(ad);
            }
        }

        public void _OnExpiring(string paramJson) {
            Debug.Log("_OnExpiring called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnExpiring != null) {
                Ads.OnExpiring(ad);
            }
        }

        public void _OnAudioStarted(string paramJson) {
            Debug.Log("_OnAudioStarted called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnAudioStarted != null) {
                Ads.OnAudioStarted(ad);
            }
        }

        public void _OnAudioStopped(string paramJson) {
            Debug.Log("_OnAudioStopped called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            InterstitialAd ad = new InterstitialAd(values);

            if (Ads.OnAudioStopped != null) {
                Ads.OnAudioStopped(ad);
            }
        }

        public void _OnIAPOpportunity(string paramJson) {
            Debug.Log("_OnIAPOpportunity called");
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            Hashtable valuesAd = null;
            string iapProductId = null;
            AdsIAPEngagementType engagement = AdsIAPEngagementType.AdColonyIAPEngagementEndCard;

            if (values.ContainsKey(Constants.OnIAPOpportunityAdKey)) {
                valuesAd = values[Constants.OnIAPOpportunityAdKey] as Hashtable;
            }
            if (values.ContainsKey(Constants.OnIAPOpportunityEngagementKey)) {
                engagement = (AdsIAPEngagementType)Convert.ToInt32(values[Constants.OnIAPOpportunityEngagementKey]);
            }
            if (values.ContainsKey(Constants.OnIAPOpportunityIapProductIdKey)) {
                iapProductId = values[Constants.OnIAPOpportunityIapProductIdKey] as string;
            }

            InterstitialAd ad = new InterstitialAd(valuesAd);

            if (Ads.OnIAPOpportunity != null) {
                Ads.OnIAPOpportunity(ad, iapProductId, engagement);
            }
        }

        public void _OnRewardGranted(string paramJson) {
            Debug.Log("_OnRewardGranted called");

            string zoneId = null;
            bool success = false;
            string productId = null;
            int amount = 0;

            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            if (values != null) {
                if (values.ContainsKey(Constants.OnRewardGrantedZoneIdKey)) {
                    zoneId = values[Constants.OnRewardGrantedZoneIdKey] as string;
                }
                if (values.ContainsKey(Constants.OnRewardGrantedSuccessKey)) {
                    success = Convert.ToBoolean(Convert.ToInt32(values[Constants.OnRewardGrantedSuccessKey]));
                }
                if (values.ContainsKey(Constants.OnRewardGrantedNameKey)) {
                    productId = values[Constants.OnRewardGrantedNameKey] as string;
                }
                if (values.ContainsKey(Constants.OnRewardGrantedAmountKey)) {
                    amount = Convert.ToInt32(values[Constants.OnRewardGrantedAmountKey]);
                }
            }

            if (Ads.OnRewardGranted != null) {
                Ads.OnRewardGranted(zoneId, success, productId, amount);
            }
        }

        public void _OnCustomMessageReceived(string paramJson) {
            Debug.Log("_OnCustomMessageReceived called");

            string type = null;
            string message = null;

            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            if (values != null) {
                if (values.ContainsKey(Constants.OnCustomMessageReceivedTypeKey)) {
                    type = values[Constants.OnCustomMessageReceivedTypeKey] as string;
                }
                if (values.ContainsKey(Constants.OnCustomMessageReceivedMessageKey)) {
                    message = values[Constants.OnCustomMessageReceivedMessageKey] as string;
                }
            }

            if (Ads.OnCustomMessageReceived != null) {
                Ads.OnCustomMessageReceived(type, message);
            }
        }

        private static Ads _sharedInstance;
        private static bool _initialized = false;
        private IAds _instance = null;
        private System.Object _updateOnMainThreadActionsLock = new System.Object();
        private readonly Queue<System.Action> _updateOnMainThreadActions = new Queue<System.Action>();

#endregion

    }

    public interface IAds {
        void Configure(string appId, AppOptions options, params string[] zoneIds);
        string GetSDKVersion();
        void RequestInterstitialAd(string zoneId, AdOptions adOptions);
        Zone GetZone(string zoneId);
        string GetUserID();
        void SetAppOptions(AppOptions options);
        AppOptions GetAppOptions();
        void SendCustomMessage(string type, string content);
        void LogInAppPurchase(string transactionId, string productId, int purchasePriceMicro, string currencyCode);
        void ShowAd(InterstitialAd ad);
        void CancelAd(InterstitialAd ad);
        void DestroyAd(string id);
    }
}
