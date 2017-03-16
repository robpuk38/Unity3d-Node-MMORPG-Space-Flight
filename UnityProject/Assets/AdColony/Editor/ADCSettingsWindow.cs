using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AdColony.Editor {
    [InitializeOnLoad]
    public class ADCSettingsWindow : EditorWindow
    {
        private ADCConfig config;

        void OnEnable() {
            config = ADCConfig.Instance;
        }

        void OnGUI() {
            CreateSection("Push Notifications", () => {
                config.PushNotificationSupport = EditorGUILayout.Toggle("Enabled", config.PushNotificationSupport);
                GUI.enabled = config.PushNotificationSupport;
                config.GCMSenderId = EditorGUILayout.TextField("Android GCM Sender ID", config.GCMSenderId);
                GUI.enabled = true;
            });

            CreateSection("Deep Link", () => {
                config.DeepLinkSupport = EditorGUILayout.Toggle("Enabled", config.DeepLinkSupport);
                GUI.enabled = config.DeepLinkSupport;
                config.AndroidScheme = EditorGUILayout.TextField("Android Scheme", config.AndroidScheme);
                config.IOSScheme = EditorGUILayout.TextField("iOS Scheme", config.IOSScheme);
                GUI.enabled = true;
            });

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel", GUILayout.MaxWidth(100))) {
                Close();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Save", GUILayout.MaxWidth(100))) {
                if (config.IsValid) {
                    UnityEngine.Debug.Log("Saving AdColony config.");

                    config.Apply();
                    ADCConfig.SaveConfig(config);
                    ADCManifestProcessor.Process();
#if UNITY_ANDROID
                    ADCDependencies.ResetDependencies();
#endif
                    AssetDatabase.Refresh();
                    Close();
                } else {
                    UnityEngine.Debug.Log("Unable to save AdColony config.");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }

        private void CreateSection(string sectionTitle, Action body)
        {
            GUILayout.Label(sectionTitle, EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();

            body();

            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
        }
    }
}
