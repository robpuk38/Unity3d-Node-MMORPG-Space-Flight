using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {

    public class PubServices : MonoBehaviour {
        // This is called when the service availability has changed
        // either due to an initialization or network conditions.  The developer
        // might want to listen to this notification to determine whether or not to
        // show a button to the Yvolver overlay.
        // Params:
        // - PubServicesStatusType
        // - error
        public static event Action<PubServicesStatusType, string> OnServiceAvailabilityChanged;

        // This is called when the overlay view either appears or
        // disappears.  This notification gives the developer an opportunity to
        // pause their app when shown.
        // Params:
        // - isVisible
        public static event Action OnOverlayVisibilityChanged;

        // This is called after a product has been requested for redemption
        // by the user.  When this is called, it is the obligation to ensure
        // that the user is granted for the given productId.
        public static event Action<PubServicesDigitalItem> OnGrantDigitalProductItem;

        // This is called after a user has been given a reward based on an
        // IAP purchase from within the app.  It should be treated as informational only.
        // Params:
        // - productId
        // - inGameCurrencyBonus
        public static event Action<string, int> OnInAppPurchaseReward;

        // This is called after a user has been given a reward based on an
        // IAP purchase from within the app.  It should be treated as informational
        // only.  Errors will typically be handled within the overlay.
        // Params:
        // - productId
        // - error
        public static event Action<string, string> OnInAppPurchaseRewardFailed;

        // This is called after new stats have been downloaded
        public static event Action OnStatsUpdated;

        // This is called after a user's VIP information has changed
        public static event Action OnVIPInformationUpdated;

        // This is called after a push notification has been received. If a
        // specific action was set on the developer portal that requires the
        // PubServices service to be active, the action will be processed when
        // the service becomes available.
        public static event Action<PubServicesPushNotification> OnPushNotificationReceived;

        // Your application has been successfully registered for push
        // notifications.
        // Parameters:
        // 1 - device token
        public static event Action<string> OnRegisteredForPushNotifications;

        // The application attempted registration for push notifications but
        // failed. Usually this is because the application isn't setup for push
        // notifications or the user has disabled push for the application.
        // Parameters:
        // 1 - error message
        public static event Action<string> OnRegisteredForPushNotificationsFailed;

        // This is called after detecting the application has been launched from
        // opening a URL with the appropriate URI scheme for this application.
        // Parameters:
        // 1 - full URI as string
        // 2 - dictionary of request parameters
        // 3 - was it handled by AdColony (e.g., an adcXXXX:// URL)
        public static event Action<string, Hashtable, bool> OnURLOpened;

        // Main initialization method.  Based on result of initialization, certain events may be fired off.
        // This service uses the API from the main AdColony API, that must be set first
        public static void Configure(string initParams = null) {
            // Using SharedInstance to make sure the MonoBehaviour is instantiated
            if (SharedInstance._instance == null) {
                Debug.LogWarning(Constants.PubServicesMessageSdkUnavailable);
                return;
            }

            Debug.Log(Constants.PubServicesManagerName + " SDK init, initParams: " + AdColonyJson.Encode(initParams));

            Hashtable h = null;
            try {
                if (initParams != null) {
                    h = (Hashtable)AdColonyJson.Decode(initParams);
                }
            } catch {
            }

            SharedInstance._instance.Configure(h);
            _initialized = true;
            SharedInstance._instance.SetUnityInitialized();
        }

        // Can use enum as flag:
        //      SetNotificationsAllowed(PubServicesNotificationType.Toast | PubServicesNotificationType.Modal);
        public static void SetNotificationsAllowed(PubServicesNotificationType value) {
            if (IsInitialized()) {
                SharedInstance._instance.SetNotificationsAllowed(value);
            }
        }

        // Returns whether or not the service currently available.
        public static bool IsServiceAvailable() {
            if (IsInitialized()) {
                return SharedInstance._instance.IsServiceAvailable();
            }
            return false;
        }

        // Shows the overlay, this will be added to the main window.
        public static void ShowOverlay() {
            if (IsInitialized()) {
                SharedInstance._instance.ShowOverlay();
            }
        }

        // Closes the overlay.
        // The developer may want to call this after redeeming a digital item.
        public static void CloseOverlay() {
            if (IsInitialized()) {
                SharedInstance._instance.CloseOverlay();
            }
        }

        // Returns TRUE when the overlay is currently open.
        public static bool IsOverlayVisible() {
            if (IsInitialized()) {
                return SharedInstance._instance.IsOverlayVisible();
            }
            return false;
        }

        public static Hashtable GetExperiments() {
            UnityEngine.Debug.Log("GetExperiments()");
            if (IsInitialized()) {
                return SharedInstance._instance.GetExperiments();
            }
            return null;
        }

        public static object GetExperimentValue(string key) {
            Hashtable o = GetExperiments();
            if (o != null) {
                return o[key];
            }
            return null;
        }

        public static PubServicesVIPInformation GetVIPInformation() {
            if (IsInitialized()) {
                return SharedInstance._instance.GetVIPInformation();
            }
            return null;
        }

#if UNITY_IOS

        // Use this method to grant a particular a reward from the given IAP receipt.
        public static void PurchaseProductIOS(string productId, string base64ReceiptData, string transactionId, int quantity, int inGameCurrencyQuantityForProduct) {
            if (IsInitialized()) {
                SharedInstance._instance.PurchaseProductIOS(productId, base64ReceiptData, transactionId, quantity, inGameCurrencyQuantityForProduct);
            }
        }

#elif UNITY_ANDROID

        // Use this method to grant a particular a reward from the given IAP receipt.
        public static void PurchaseProductGoogle(string googleReceiptJson, string googleSignature, string purchaseLocale, long purchasePriceMicro, int inGameCurrencyQuantityForProduct) {
            Debug.Log("PurchaseProductGoogle: " + purchaseLocale + ", purchasePriceMicro" + purchasePriceMicro + ", inGameCurrencyQuantityForProduct: " + inGameCurrencyQuantityForProduct);
            if (IsInitialized()) {
                Debug.Log("PurchaseProductGoogle IsInitialized");
                SharedInstance._instance.PurchaseProductGoogle(googleReceiptJson, googleSignature, purchaseLocale, purchasePriceMicro, inGameCurrencyQuantityForProduct);
            }
        }

#endif

        // Returns all stats already downloaded from the server
        public static Dictionary<string, long> GetStats() {
            if (IsInitialized()) {
                return SharedInstance._instance.GetStats();
            }
            return null;
        }

        // Retrieves a stat that has already been downloaded from the server
        public static long? GetStat(string name) {
            if (IsInitialized()) {
                return SharedInstance._instance.GetStat(name);
            }
            return null;
        }

        // Sets a stat locally and caches for transmission to the server
        public static bool SetStat(string name, long value) {
            if (IsInitialized()) {
                return SharedInstance._instance.SetStat(name, value);
            }
            return false;
        }

        // Increments a stat locally and caches for transmission to the server
        public static bool IncrementStat(string name, long value) {
            if (IsInitialized()) {
                return SharedInstance._instance.IncrementStat(name, value);
            }
            return false;
        }

        // Retrieves the latest stats from the server
        public static void RefreshStats() {
            if (IsInitialized()) {
                SharedInstance._instance.RefreshStats();
            }
        }

        //  Marks the start of a round for stat aggregation per round.
        public static void MarkStartRound() {
            if (IsInitialized()) {
                SharedInstance._instance.MarkStartRound();
            }
        }

        // Marks the end of a round for stat aggregation per round.
        public static void MarkEndRound() {
            if (IsInitialized()) {
                SharedInstance._instance.MarkEndRound();
            }
        }

        public static void RegisterForPushNotifications(PubServicesPushNotificationType type) {
            if (IsInitialized()) {
                SharedInstance._instance.RegisterForPushNotifications(type);
            }
        }

        public static void UnregisterForPushNotifications() {
            if (IsInitialized()) {
                SharedInstance._instance.UnregisterForPushNotifications();
            }
        }

        // --------------------------------------------------------------------

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
                Debug.LogError(Constants.PubServicesMessageNotInitialized);
                return false;
            }
            return true;
        }

        public static PubServices SharedInstance {
            get {
                if (!_sharedInstance) {
                    _sharedInstance = (PubServices)FindObjectOfType(typeof(PubServices));
                }

                if (!_sharedInstance) {
                    GameObject singleton = new GameObject();
                    _sharedInstance = singleton.AddComponent<PubServices>();
                    singleton.name = Constants.PubServicesManagerName;
                    DontDestroyOnLoad(singleton);

                    if (_sharedInstance._instance != null) {
                        Debug.LogWarning(Constants.PubServicesMessageAlreadyInitialized);
                        return null;
                    } else {
                        _sharedInstance._instance = null;
#if UNITY_EDITOR

#elif UNITY_ANDROID
                        _sharedInstance._instance = new PubServicesAndroid(singleton.name);
#elif UNITY_IOS
                        _sharedInstance._instance = new PubServicesIOS(singleton.name);
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

        public void _OnServiceAvailabilityChanged(string value) {
            UnityEngine.Debug.Log("_OnServiceAvailabilityChanged called");
            string error = "";
            PubServicesStatusType status = PubServicesStatusType.Unknown;

            Hashtable values = (AdColonyJson.Decode(value) as Hashtable);

            if (values.ContainsKey("error")) {
                error = values["error"] as string;
            }

            if (values.ContainsKey("status")) {
                int temp = Convert.ToInt32(values["status"]);
                status = (PubServicesStatusType)temp;
            }

            if (PubServices.OnServiceAvailabilityChanged != null) {
                PubServices.OnServiceAvailabilityChanged(status, error);
            }
        }

        public void _OnOverlayVisibilityChanged() {
            if (PubServices.OnOverlayVisibilityChanged != null) {
                PubServices.OnOverlayVisibilityChanged();
            }
        }

        public void _OnGrantDigitalProductItem(string digitalItemJson) {
            UnityEngine.Debug.Log("GrantDigitalProductItem(" + digitalItemJson + ")");
            try {
                Hashtable values = (AdColonyJson.Decode(digitalItemJson) as Hashtable);
                if (values != null) {
                    PubServicesDigitalItem digitalItem = new PubServicesDigitalItem(values);
                    if (digitalItem != null) {
                        if (PubServices.OnGrantDigitalProductItem != null) {
                            PubServices.OnGrantDigitalProductItem(digitalItem);
                        }
                    }
                }
            } catch (Exception e) {
                UnityEngine.Debug.Log("Exception caught in CanGrantDigitalProductItem, " + e);
            }
        }

        // Multiple items returned as JSON string
        public void _OnIAPProductPurchased(string paramJson) {
            int inGameCurrencyBonus = 0;
            string productId = "";

            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);

            if (values.ContainsKey("product_id")) {
                productId = values["product_id"] as string;
            }
            if (values.ContainsKey("in_game_currency_bonus")) {
                inGameCurrencyBonus = Convert.ToInt32(values["in_game_currency_bonus"]);
            }

            if (PubServices.OnInAppPurchaseReward != null) {
                PubServices.OnInAppPurchaseReward(productId, inGameCurrencyBonus);
            }
        }

        public void _OnIAPProductPurchaseFailed(string paramJson) {
            string productId = "";
            string error = "";

            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);

            if (values.ContainsKey("product_id")) {
                productId = values["product_id"] as string;
            }
            if (values.ContainsKey("error")) {
                error = values["error"] as string;
            }

            if (PubServices.OnInAppPurchaseRewardFailed != null) {
                PubServices.OnInAppPurchaseRewardFailed(productId, error);
            }
        }

        public void _OnStatsUpdated(string unused) {
            if (PubServices.OnStatsUpdated != null) {
                PubServices.OnStatsUpdated();
            }
        }

        public void _OnVIPInformationUpdated(string unused) {
            if (PubServices.OnVIPInformationUpdated != null) {
                PubServices.OnVIPInformationUpdated();
            }
        }

        public void _OnPushNotificationReceived(string paramJson) {
            UnityEngine.Debug.Log("_OnPushNotificationReceived: " + paramJson);
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);
            AdColony.PubServicesPushNotification pushNotification = new PubServicesPushNotification(values);
            if (PubServices.OnPushNotificationReceived != null) {
                PubServices.OnPushNotificationReceived(pushNotification);
            }
        }

        public void _OnRegisteredForPushNotifications(string deviceToken) {
            UnityEngine.Debug.Log("_OnRegisteredForPushNotifications: " + deviceToken);
            if (PubServices.OnRegisteredForPushNotifications != null) {
                PubServices.OnRegisteredForPushNotifications(deviceToken);
            }
        }

        public void _OnRegisteredForPushNotificationsFailed(string error) {
            UnityEngine.Debug.Log("_OnRegisteredForPushNotificationsFailed: " + error);
            if (PubServices.OnRegisteredForPushNotificationsFailed != null) {
                PubServices.OnRegisteredForPushNotificationsFailed(error);
            }
        }

        public void _OnURLOpened(string paramJson) {
            Hashtable values = (AdColonyJson.Decode(paramJson) as Hashtable);

            string url = "";
            Hashtable urlParams = null;
            bool handled = false;

            if (values.ContainsKey("url")) {
                url = values["url"] as string;
            }
            if (values.ContainsKey("url_params")) {
                urlParams = (AdColonyJson.Decode(values["url_params"] as string) as Hashtable);
            }
            if (values.ContainsKey("handled")) {
                handled = (bool)values["handled"];
            }

            if (PubServices.OnURLOpened != null) {
                PubServices.OnURLOpened(url, urlParams, handled);
            }
        }

        private static PubServices _sharedInstance;
        private IPubServices _instance = null;
        private static bool _initialized = false;

#endregion
    }

    public interface IPubServices {
        void Configure(Hashtable initParams);
        void SetNotificationsAllowed(PubServicesNotificationType value);
        bool IsServiceAvailable();
        void ShowOverlay();
        void CloseOverlay();
        bool IsOverlayVisible();
        Hashtable GetExperiments();
        PubServicesVIPInformation GetVIPInformation();
#if UNITY_IOS
        void PurchaseProductIOS(string productId, string base64ReceiptData, string transactionId, int quantity, int inGameCurrencyQuantityForProduct);
#elif UNITY_ANDROID
        void PurchaseProductGoogle(string googleReceiptJson, string googleSignature, string purchaseLocale, long purchasePriceMicro, int inGameCurrencyQuantityForProduct);
#endif
        Dictionary<string, long> GetStats();
        long GetStat(string name);
        bool SetStat(string name, long value);
        bool IncrementStat(string name, long value);
        void RefreshStats();
        void MarkStartRound();
        void MarkEndRound();
        // Type is ignored for Android
        void RegisterForPushNotifications(PubServicesPushNotificationType type);
        void UnregisterForPushNotifications();
        void SetUnityInitialized();
    }
}
