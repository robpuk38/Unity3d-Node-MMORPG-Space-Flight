using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {

    // -------------------------------------------------------------------------
    // Base Options Class
    // -------------------------------------------------------------------------
    public class Options {
        public UserMetadata Metadata;

        protected Hashtable _data = new Hashtable();

        public Options() {

        }

        public Options(Hashtable values) {
            _data = new Hashtable(values);

            if (values.ContainsKey(Constants.OptionsMetadataKey)) {
                Hashtable metadataValues = values[Constants.OptionsMetadataKey] as Hashtable;
                Metadata = new UserMetadata(metadataValues);
                _data.Remove(Constants.OptionsMetadataKey);
            }
        }

        public Hashtable ToHashtable() {
            Hashtable data = new Hashtable(_data);
            if (Metadata != null) {
                Hashtable metadataData = Metadata.ToHashtable();
                data.Add(Constants.OptionsMetadataKey, metadataData);
            }
            return data;
        }

        public string ToJsonString() {
            Hashtable data = ToHashtable();
            return AdColonyJson.Encode(data);
        }

        public void SetOption(string key, string value) {
            if (key == null) {
                Debug.Log("Invalid option.");
                return;
            }

            if (value == null) {
                Debug.Log("Invalid option value.");
                return;
            }

            _data[key] = value;
        }

        public void SetOption(string key, int value) {
            if (key == null) {
                Debug.Log("Invalid option key.");
                return;
            }

            _data[key] = value;
        }

        public void SetOption(string key, double value) {
            if (key == null) {
                Debug.Log("Invalid option.");
                return;
            }

            _data[key] = value;
        }

        public void SetOption(string key, bool value) {
            if (key == null) {
                Debug.Log("Invalid option.");
                return;
            }

            _data[key] = value;
        }

        public string GetStringOption(string key) {
            return _data.ContainsKey(key) ? _data[key] as string : null;
        }

        public int GetIntOption(string key) {
            return _data.ContainsKey(key) ? Convert.ToInt32(_data[key]) : 0;
        }

        public double GetDoubleOption(string key) {
            return _data.ContainsKey(key) ? Convert.ToDouble(_data[key]) : 0.0;
        }

        public bool GetBoolOption(string key) {
            return _data.ContainsKey(key) ? Convert.ToBoolean(_data[key]) : false;
        }
    }

    // -------------------------------------------------------------------------
    // Application Options Class
    // -------------------------------------------------------------------------
    public class AppOptions : Options {
        private bool _disableLogging;
        public bool DisableLogging {
            get {
                return _disableLogging;
            }
            set {
                _disableLogging = value;
                _data[Constants.AppOptionsDisableLoggingKey] = _disableLogging;
            }
        }

        private string _userId;
        public string UserId {
            get {
                return _userId;
            }
            set {
                _userId = value;
                _data[Constants.AppOptionsUserIdKey] = _userId;
            }
        }

        private AdOrientationType _adOrientation = AdOrientationType.AdColonyOrientationAll;
        public AdOrientationType AdOrientation {
            get {
                return _adOrientation;
            }
            set {
                _adOrientation = value;
                _data[Constants.AppOptionsOrientationKey] = Convert.ToInt32(_adOrientation);
            }
        }

        // Android only:
        private bool _multiWindowEnabled;
        public bool MultiWindowEnabled {
            get {
                return _multiWindowEnabled;
            }
            set {
                _multiWindowEnabled = value;
                _data[Constants.AppOptionsMultiWindowEnabledKey] = _multiWindowEnabled;
            }
        }

        // Android only:
        private string _originStore;
        public string OriginStore {
            get {
                return _originStore;
            }
            set {
                _originStore = value;
                _data[Constants.AppOptionsOriginStoreKey] = _originStore;
            }
        }

        public AppOptions() {

        }

        public AppOptions(Hashtable values) : base(values) {
            if (values != null) {
                _data = new Hashtable(values);

                if (values.ContainsKey(Constants.AppOptionsDisableLoggingKey)) {
                    _disableLogging = Convert.ToBoolean(values[Constants.AppOptionsDisableLoggingKey]);
                }
                if (values.ContainsKey(Constants.AppOptionsUserIdKey)) {
                    _userId = values[Constants.AppOptionsUserIdKey] as string;
                }
                if (values.ContainsKey(Constants.AppOptionsOrientationKey)) {
                    _adOrientation = (AdOrientationType)Convert.ToInt32(values[Constants.AppOptionsOrientationKey]);
                }
                if (values.ContainsKey(Constants.AppOptionsMultiWindowEnabledKey)) {
                    _multiWindowEnabled = Convert.ToBoolean(values[Constants.AppOptionsMultiWindowEnabledKey]);
                }
                if (values.ContainsKey(Constants.AppOptionsOriginStoreKey)) {
                    _originStore = values[Constants.AppOptionsOriginStoreKey] as string;
                }
            }
        }
    }

    // -------------------------------------------------------------------------
    // Ad Specific Options Class
    // -------------------------------------------------------------------------
    public class AdOptions : Options {
        private bool _showPrePopup;
        public bool ShowPrePopup {
            get {
                return _showPrePopup;
            }
            set {
                _showPrePopup = value;
                _data[Constants.AdOptionsPrePopupKey] = _showPrePopup;
            }
        }

        private bool _showPostPopup;
        public bool ShowPostPopup {
            get {
                return _showPostPopup;
            }
            set {
                _showPostPopup = value;
                _data[Constants.AdOptionsPostPopupKey] = _showPostPopup;
            }
        }

        public AdOptions() {

        }

        public AdOptions(Hashtable values) : base(values) {
            if (values != null) {
                _data = new Hashtable(values);

                if (values.ContainsKey(Constants.AdOptionsPrePopupKey)) {
                    _showPrePopup = Convert.ToBoolean(values[Constants.AdOptionsPrePopupKey]);
                }
                if (values.ContainsKey(Constants.AdOptionsPostPopupKey)) {
                    _showPostPopup = Convert.ToBoolean(values[Constants.AdOptionsPostPopupKey]);
                }
            }
        }
    }
}
