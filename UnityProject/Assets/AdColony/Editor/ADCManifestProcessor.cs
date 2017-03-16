using System;
using System.Text;
using UnityEditor;
using System.Linq;
using UnityEngine;
using System.IO;

namespace AdColony.Editor {
    [InitializeOnLoad]
    public static class ADCManifestProcessor {
        private const string templateManifest = "AndroidManifestTemplate.xml";
        private const string manifest = "AndroidManifest.xml";

        static ADCManifestProcessor() {
            Process();
        }

        private static string EnableSection(string body, string tag, bool condition) {
            int startPos = -1;
            do {
                startPos = body.IndexOf("${" + tag + "}");
                if (startPos != -1) {
                    int endPos = body.IndexOf("${/" + tag + "}");
                    if (endPos == -1) {
                        throw new Exception("Invalid " + templateManifest + ": " + tag);
                    }

                    int len1 = ("${" + tag + "}").Length;
                    int len2 = len1 + 1;

                    var stringBuilder = new StringBuilder(body);
                    if (condition) {
                        // Just remove the tags
                        stringBuilder.Remove(endPos, len2);
                        stringBuilder.Remove(startPos, len1);
                    } else {
                        // Remove the entire chunk
                        stringBuilder.Remove(startPos, endPos - startPos + len2);
                    }
                    body = stringBuilder.ToString();
                }
            } while (startPos != -1);

            return body;
        }

        public static void CheckMinSDKVersion() {
            if (PlayerSettings.Android.minSdkVersion < ADCPluginInfo.RequiredAndroidVersion) {
                UnityEngine.Debug.LogError("AdColony requires " + ADCPluginInfo.RequiredAndroidVersion + " in PlayerSettings");
            }
        }

        public static void Process() {
            CheckMinSDKVersion();

            string outputPath = Path.Combine(Application.dataPath, "Plugins/Android/AdColony");
            string inputPath = Path.Combine(Application.dataPath, "AdColony/Editor");

            string original = Path.Combine(inputPath, ADCManifestProcessor.templateManifest);
            string manifest = Path.Combine(outputPath, ADCManifestProcessor.manifest);

            if (!File.Exists(original)) {
                UnityEngine.Debug.Log("AdColony manifest template missing in folder: " + inputPath);
                return;
            }

            if (File.Exists(manifest)) {
                File.Delete(manifest);
            }

            File.Copy(original, manifest);

            StreamReader sr = new StreamReader(manifest);
            string body = sr.ReadToEnd();
            sr.Close();

            body = body.Replace("${applicationId}", PlayerSettings.applicationIdentifier);
            body = body.Replace("${scheme}", ADCConfig.Instance.AndroidScheme);

            string[] conditionalStrings = {
                "messagingLaunchActivity",
                "pushNotificationSupport",
                "deepLinkSupport"
            };

            bool[] conditions = {
                ADCConfig.Instance.PushNotificationSupport || ADCConfig.Instance.DeepLinkSupport,
                ADCConfig.Instance.PushNotificationSupport,
                ADCConfig.Instance.DeepLinkSupport
            };

            for (int i = 0; i < conditionalStrings.Length; i++) {
                body = EnableSection(body, conditionalStrings[i], conditions[i]);
            }

            using (var wr = new StreamWriter(manifest, false)) {
                wr.Write(body);
            }
        }
    }
}
