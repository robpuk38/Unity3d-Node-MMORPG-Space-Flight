using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdColony {
    public class UserMetadata {
        private Hashtable _data = new Hashtable();

        private int _age;
        public int Age {
            get {
                return _age;
            }
            set {
                if (value <= 0) {
                    Debug.Log("Tried to set user metadata age with an invalid value. Value will not be included.");
                    return;
                }

                _age = value;
                _data[Constants.UserMetadataAgeKey] = _age;
            }
        }

        private List<string> _interests;
        public List<string> Interests {
            get {
                return _interests;
            }
            set {
                _interests = value;
                _data[Constants.UserMetadataInterestsKey] = new ArrayList(_interests);
            }
        }

        private string _gender;
        public string Gender {
            get {
                return _gender;
            }
            set {
                if (value == null) {
                    Debug.Log("Tried to set user metadata gender with an invalid string. Value will not be included.");
                    return;
                }

                string setGender = value;
                _gender = setGender;
                _data[Constants.UserMetadataGenderKey] = _gender;
            }
        }

        private double _latitude;
        public double Latitude {
            get {
                return _latitude;
            }
            set {
                _latitude = value;
                _data[Constants.UserMetadataLatitudeKey] = _latitude;
            }
        }

        private double _longitude;
        public double Longitude {
            get {
                return _longitude;
            }
            set {
                _longitude = value;
                _data[Constants.UserMetadataLongitudeKey] = _longitude;
            }
        }

        private string _zipCode;
        public string ZipCode {
            get {
                return _zipCode;
            }
            set {
                if (value == null) {
                    Debug.Log("Tried to set user metadata zip code with an invalid string. Value will not be included.");
                    return;
                }

                string setZipCode = value as string;
                _zipCode = setZipCode;
                _data[Constants.UserMetadataZipCodeKey] = _zipCode;
            }
        }

        private int _householdIncome;
        public int HouseholdIncome {
            get {
                return _householdIncome;
            }
            set {
                _householdIncome = value;
                _data[Constants.UserMetadataHouseholdIncomeKey] = _householdIncome;
            }
        }

        private string _maritalStatus;
        public string MaritalStatus {
            get {
                return _maritalStatus;
            }
            set {
                if (value == null) {
                    Debug.Log("Tried to set user metadata marital status with an invalid string. Value will not be included.");
                    return;
                }

                string setMaritalStatus = value as string;
                _maritalStatus = setMaritalStatus;
                _data[Constants.UserMetadataMaritalStatusKey] = _maritalStatus;
            }
        }

        private string _educationLevel;
        public string EducationLevel {
            get {
                return _educationLevel;
            }
            set {
                if (value == null) {
                    Debug.Log("Tried to set user metadata education level with an invalid string. Value will not be included.");
                    return;
                }

                string setEducationLevel = value as string;
                _educationLevel = setEducationLevel;
                _data[Constants.UserMetadataEducationLevelKey] = _educationLevel;
            }
        }

        public UserMetadata() {

        }

        public UserMetadata(Hashtable values) {
            _data = new Hashtable(values);

            if (values != null) {
                if (values.ContainsKey(Constants.UserMetadataAgeKey)) {
                    _age = Convert.ToInt32(values[Constants.UserMetadataAgeKey]);
                }
                if (values.ContainsKey(Constants.UserMetadataInterestsKey)) {
                    ArrayList interests = values[Constants.UserMetadataInterestsKey] as ArrayList;
                    Interests = new List<string>();
                    foreach (string interest in interests) {
                        Interests.Add(interest);
                    }
                }
                if (values.ContainsKey(Constants.UserMetadataGenderKey)) {
                    _gender = values[Constants.UserMetadataGenderKey] as string;
                }
                if (values.ContainsKey(Constants.UserMetadataLatitudeKey)) {
                    _latitude = Convert.ToDouble(values[Constants.UserMetadataLatitudeKey]);
                }
                if (values.ContainsKey(Constants.UserMetadataLongitudeKey)) {
                    _longitude = Convert.ToDouble(values[Constants.UserMetadataLongitudeKey]);
                }
                if (values.ContainsKey(Constants.UserMetadataZipCodeKey)) {
                    _zipCode = values[Constants.UserMetadataZipCodeKey] as string;
                }
                if (values.ContainsKey(Constants.UserMetadataHouseholdIncomeKey)) {
                    _householdIncome = Convert.ToInt32(values[Constants.UserMetadataHouseholdIncomeKey]);
                }
                if (values.ContainsKey(Constants.UserMetadataMaritalStatusKey)) {
                    _maritalStatus = values[Constants.UserMetadataMaritalStatusKey] as string;
                }
                if (values.ContainsKey(Constants.UserMetadataEducationLevelKey)) {
                    _educationLevel = values[Constants.UserMetadataEducationLevelKey] as string;
                }
            }
        }

        public void SetMetadata(string key, string value) {
            if (key == null) {
                return;
            }
            _data[key] = value;
        }

        public void SetMetadata(string key, int value) {
            if (key == null) {
                return;
            }
            _data[key] = value;
        }

        public void SetMetadata(string key, double value) {
            if (key == null) {
                return;
            }
            _data[key] = value;
        }

        public void SetMetadata(string key, bool value) {
            if (key == null) {
                return;
            }
            _data[key] = value;
        }

        public string GetStringMetadata(string key) {
            return _data.ContainsKey(key) ? _data[key] as string : null;
        }

        public int GetIntMetadata(string key) {
            return _data.ContainsKey(key) ? Convert.ToInt32(_data[key]) : 0;
        }

        public double GetDoubleMetadata(string key) {
            return _data.ContainsKey(key) ? Convert.ToDouble(_data[key]) : 0.0;
        }

        public bool GetBoolMetadata(string key) {
            return _data.ContainsKey(key) ? Convert.ToBoolean(Convert.ToInt32(_data[key])) : false;
        }

        public Hashtable ToHashtable() {
            return new Hashtable(_data);
        }

        public string ToJsonString() {
            return AdColonyJson.Encode(_data);
        }
    }
}
