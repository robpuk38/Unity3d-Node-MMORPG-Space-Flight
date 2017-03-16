using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {
#if UNITY_IOS
    public class PubServicesIOS : IPubServices {
        [DllImport ("__Internal")] private static extern void _AdcPubServicesConfigure(string initParams);
        [DllImport ("__Internal")] private static extern void _AdcSetNotificationsAllowed(int value);
        [DllImport ("__Internal")] private static extern bool _AdcIsServiceAvailable();
        [DllImport ("__Internal")] private static extern void _AdcShowOverlay();
        [DllImport ("__Internal")] private static extern void _AdcCloseOverlay();
        [DllImport ("__Internal")] private static extern bool _AdcIsOverlayVisible();
        [DllImport ("__Internal")] private static extern string _AdcGetExperiments();
        [DllImport ("__Internal")] private static extern string _AdcGetVipInformation();
        [DllImport ("__Internal")] private static extern bool _AdcPurchaseProduct(string productId, string base64ReceiptData, string transactionId, int quantity, int inGameCurrencyQuantityForProduct);
        [DllImport ("__Internal")] private static extern string _AdcGetStats();
        [DllImport ("__Internal")] private static extern long _AdcGetStat(string name);
        [DllImport ("__Internal")] private static extern bool _AdcSetStat(string name, long value);
        [DllImport ("__Internal")] private static extern bool _AdcIncrementStat(string name, long value);
        [DllImport ("__Internal")] private static extern void _AdcRefreshStats();
        [DllImport ("__Internal")] private static extern void _AdcMarkStartRound();
        [DllImport ("__Internal")] private static extern void _AdcMarkEndRound();
        [DllImport ("__Internal")] private static extern void _AdcSetManagerName(string managerName);
        [DllImport ("__Internal")] private static extern void _AdcRegsiterForPushNotifications(int value);
        [DllImport ("__Internal")] private static extern void _AdcUnregsiterForPushNotifications();
        [DllImport ("__Internal")] private static extern void _AdcSetUnityInitialized();

        public PubServicesIOS(string managerName) {
            _AdcSetManagerName(managerName);
        }

        public void Configure(Hashtable initParams) {
            string initParamsJson = "";
            if (initParams != null) {
                initParamsJson = AdColonyJson.Encode(initParams);
            }
            _AdcPubServicesConfigure(initParamsJson);
        }

        public void SetNotificationsAllowed(PubServicesNotificationType value) {
            _AdcSetNotificationsAllowed((int)value);
        }

        public void PurchaseProductIOS(string productId, string base64ReceiptData, string transactionId, int quantity, int inGameCurrencyQuantityForProduct) {
            _AdcPurchaseProduct(productId, base64ReceiptData, transactionId, quantity, inGameCurrencyQuantityForProduct);
        }

        public void ShowOverlay() {
            _AdcShowOverlay();
        }

        public void CloseOverlay() {
            _AdcCloseOverlay();
        }

        public bool IsOverlayVisible() {
            return _AdcIsOverlayVisible();
        }

        public bool IsServiceAvailable() {
            return _AdcIsServiceAvailable();
        }

        public Hashtable GetExperiments() {
            string data = _AdcGetExperiments();
            Hashtable values = (AdColonyJson.Decode(data) as Hashtable);
            return values;
        }

        public PubServicesVIPInformation GetVIPInformation() {
            string data = _AdcGetVipInformation();
            Hashtable values = (AdColonyJson.Decode(data) as Hashtable);
            return new PubServicesVIPInformation(values);
        }

        public Dictionary<string, long> GetStats() {
            string data = _AdcGetStats();
            Console.WriteLine(data);
            ArrayList values = (AdColonyJson.Decode(data) as ArrayList);

            Dictionary<string, long> stats = new Dictionary<string, long>();
            foreach (Hashtable h in values) {
                if (h.ContainsKey("name") && h.ContainsKey("value")) {
                    stats.Add((string)h["name"], Int64.Parse(h["value"].ToString()));
                }
            }

            return stats;
        }

        public long GetStat(string name) {
            return _AdcGetStat(name);
        }

        public bool SetStat(string name, long value) {
            return _AdcSetStat(name, value);
        }

        public bool IncrementStat(string name, long value) {
            return _AdcIncrementStat(name, value);
        }

        public void RefreshStats() {
            _AdcRefreshStats();
        }

        public void MarkStartRound() {
            _AdcMarkStartRound();
        }

        public void MarkEndRound() {
            _AdcMarkEndRound();
        }

        public void RegisterForPushNotifications(PubServicesPushNotificationType type) {
            _AdcRegsiterForPushNotifications((int)type);
        }

        public void UnregisterForPushNotifications() {
            _AdcUnregsiterForPushNotifications();
        }

        public void SetUnityInitialized() {
            _AdcSetUnityInitialized();
        }
    }
#endif
}
