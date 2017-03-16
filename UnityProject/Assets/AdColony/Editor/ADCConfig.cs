using System;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

using UnityEditor;
using System.Xml;

namespace AdColony.Editor {
    [Serializable]
    public class ADCConfig {
        private const string filePath = "ProjectSettings/" + ADCPluginInfo.Name + ".xml";
        private static ADCConfig cachedInstance;

        [SerializeField]
        public bool PushNotificationSupport { get; set; }

        [SerializeField]
        public string GCMSenderId { get; set; }

        [SerializeField]
        public bool DeepLinkSupport { get; set; }

        [SerializeField]
        public string IOSScheme { get; set; }

        [SerializeField]
        public string AndroidScheme { get; set; }

        public bool IsValid {
            get { return Validate(); }
        }

        public static ADCConfig Instance {
            get {
                if (cachedInstance == null) {
                    cachedInstance = LoadConfig();
                }
                return cachedInstance;
            }
        }

        private ADCConfig() {
        }

        private ADCConfig(ADCConfig config) {
            this.PushNotificationSupport = config.PushNotificationSupport;
            this.GCMSenderId = config.GCMSenderId;
            this.DeepLinkSupport = config.DeepLinkSupport;
            this.IOSScheme = config.IOSScheme;
            this.AndroidScheme = config.AndroidScheme;
        }

        [InitializeOnLoadMethod]
        private static ADCConfig LoadConfig() {
            try {
                if (File.Exists(filePath)) {
                    using (Stream fileStream = File.OpenRead(filePath)) {
                        XmlSerializer serializer = new XmlSerializer(typeof(ADCConfig));
                        ADCConfig config =(ADCConfig)serializer.Deserialize(fileStream);
                        if (config.IsValid) {
                            cachedInstance = config;
                        }
                    }
                }
            } catch(Exception e) {
                UnityEngine.Debug.Log("Failed to load ADCConfig: " + e.Message);
                File.Delete(filePath);
            }

            if (cachedInstance == null) {
                cachedInstance = new ADCConfig();
                SaveConfig(cachedInstance);
            }

            CreateRuntimeConfig(cachedInstance);

            return new ADCConfig(cachedInstance);
        }

        public static void SaveConfig(ADCConfig config) {
            if (config.IsValid) {
                using (Stream fileStream = File.Open(filePath, FileMode.Create)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(ADCConfig));
                    serializer.Serialize(fileStream, config);
                }

                cachedInstance = config;
                CreateRuntimeConfig(config);
            }
        }

        public bool Apply() {
            if (IsValid) {
                return true;
            }

            return false;
        }

        private bool Validate() {
            bool ret = true;

#if UNITY_ANDROID
            if (PushNotificationSupport) {
                ret = ret && !string.IsNullOrEmpty(GCMSenderId);
            }
#endif
            if (DeepLinkSupport) {
#if UNITY_ANDROID
                ret = ret && !string.IsNullOrEmpty(AndroidScheme);
#elif UNITY_IOS
                ret = ret && !string.IsNullOrEmpty(IOSScheme);
#endif
            }

            return ret;
        }

        private static void CreateRuntimeConfig(ADCConfig config) {
            string fullPath = Application.dataPath + "/AdColony/Scripts/AdColonyRuntimeConfig.cs";
            string code = GenerateCode(config);
            if (System.String.IsNullOrEmpty(code)) {
                Debug.LogError("Error saving config, no code to write.");
            }
            System.IO.File.WriteAllText(fullPath, code);
        }


        private static string GenerateCode(ADCConfig config) {
            string code = "// GENERATED CODE\n";
            code += "namespace AdColony {\n";
            code += "    public class AdColonyRuntimeConfig {\n";
            code += "        public static string GCMSenderId = \"" + config.GCMSenderId + "\";\n";
            code += "    }\n";
            code += "}\n";
            return code;
        }
    }
}
