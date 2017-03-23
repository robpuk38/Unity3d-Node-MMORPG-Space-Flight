using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {
#if UNITY_ANDROID
    public class PubServicesAndroid : IPubServices {

        private AndroidJavaClass _plugin;
        private AndroidJavaClass _pluginWrapper;

        public PubServicesAndroid(string managerName) {
            // This is the raw AdColony.jar
            _plugin = new AndroidJavaClass("com.adcolony.sdk.AdColonyPubServices");

            // This is a separate wrapper to manage type conversions and callbacks
            _pluginWrapper = new AndroidJavaClass("com.adcolony.unityplugin.UnityADCPubServices");
            _pluginWrapper.CallStatic("setManagerName", managerName);
        }

        public void Configure(Hashtable initParams) {
            // Need to use the YvolverUnity bridge so it can get the required context
            string initParamsJson = AdColonyJson.Encode(initParams);
            _pluginWrapper.CallStatic("configure", initParamsJson);
        }

        public void SetNotificationsAllowed(PubServicesNotificationType value) {
            _pluginWrapper.CallStatic("setNotificationsAllowed", (int)value);
        }

        public bool IsServiceAvailable() {
            return _plugin.CallStatic<bool>("isServiceAvailable");
        }

        public void ShowOverlay() {
            _plugin.CallStatic("showOverlay");
        }

        public void CloseOverlay() {
            _plugin.CallStatic("closeOverlay");
        }

        public bool IsOverlayVisible() {
            return _plugin.CallStatic<bool>("isOverlayVisible");
        }

        public Hashtable GetExperiments() {
            string experiments = _pluginWrapper.CallStatic<string>("getExperiments");
            try {
                Hashtable values = (AdColonyJson.Decode(experiments) as Hashtable);
                return values;
            } catch (Exception e) {
                UnityEngine.Debug.LogError("PubServicesAndroid::GetExperiments() An error occurred: " + e);
            }
            return null;
        }

        public PubServicesVIPInformation GetVIPInformation() {
            string vipInformationJson = _pluginWrapper.CallStatic<string>("getVIPInformation");
            Hashtable values = (AdColonyJson.Decode(vipInformationJson) as Hashtable);
            PubServicesVIPInformation userInformation = new PubServicesVIPInformation(values);
            return userInformation;
        }

        public void PurchaseProductGoogle(string googleReceiptJson, string googleSignature, string purchaseLocale, long purchasePriceMicro, int inGameCurrencyQuantityForProduct) {
            Hashtable values = new Hashtable();
            values["googleReceiptJson"] = googleReceiptJson;
            values["googleSignature"] = googleSignature;
            values["purchaseLocale"] = purchaseLocale;
            values["purchasePriceMicro"] = purchasePriceMicro;
            values["inGameCurrencyQuantityForProduct"] = inGameCurrencyQuantityForProduct;
            string json = AdColonyJson.Encode(values);
            _pluginWrapper.CallStatic("purchaseProductGoogle", json);
        }

        public Dictionary<string, long> GetStats() {
            Dictionary<string, long> stats = new Dictionary<string, long>();
            string data = _pluginWrapper.CallStatic<string>("getStats");
            ArrayList values = (AdColonyJson.Decode(data) as ArrayList);

            if (values != null) {
                foreach (Hashtable h in values) {
                    if (h.ContainsKey("name") && h.ContainsKey("value")) {
                        stats.Add((string)h["name"], Int64.Parse(h["value"].ToString()));
                    }
                }
            } else {
                Debug.Log("Unable to decode stats: " + data);
            }

            return stats;
        }

        public long GetStat(string name) {
            return _plugin.CallStatic<long>("getStatValue", name);
        }

        public bool SetStat(string name, long value) {
            return _plugin.CallStatic<bool>("setStat", name, value);
        }

        public bool IncrementStat(string name, long value) {
            return _plugin.CallStatic<bool>("incrementStat", name, value);
        }

        public void RefreshStats() {
            _plugin.CallStatic("refreshStats");
        }

        public void MarkStartRound() {
            _plugin.CallStatic("markStartRound");
        }

        public void MarkEndRound() {
            _plugin.CallStatic("markEndRound");
        }

        public void RegisterForPushNotifications(PubServicesPushNotificationType type) {
            Debug.Log("RegisterForPushNotifications called");
            string senderId = AdColony.AdColonyRuntimeConfig.GCMSenderId;
            if (string.IsNullOrEmpty(senderId)) {
                Debug.LogError("GCMSenderId not set in Tools->AdColony->Settings...");
                return;
            }
            _plugin.CallStatic("registerForPushNotifications", senderId);
        }

        public void UnregisterForPushNotifications() {
            Debug.Log("UnregisterForPushNotifications called");
            _plugin.CallStatic("unregisterForPushNotifications");
        }

        public void SetUnityInitialized() {
            Debug.Log("SetUnityInitialized called");
            _pluginWrapper.CallStatic("setInitialized");
        }
    }
#endif
}
