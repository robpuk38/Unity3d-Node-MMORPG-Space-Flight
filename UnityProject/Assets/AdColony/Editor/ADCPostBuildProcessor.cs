#if UNITY_IPHONE || UNITY_ANDROID

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
#if UNITY_IPHONE
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

namespace AdColony.Editor {
    public class ADCPostBuildProcessor : MonoBehaviour {

#if UNITY_CLOUD_BUILD
        public static void OnPostprocessBuildiOS(string exportPath) {
            ProcessPostBuild(BuildTarget.iOS, exportPath);
        }
#endif

        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath) {
            if (buildTarget == BuildTarget.iOS) {
#if !UNITY_CLOUD_BUILD
                Debug.Log("AdColony: OnPostprocessBuild");
                UpdateProject(buildTarget, buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj");
                UpdateProjectPlist(buildTarget, buildPath + "/Info.plist");
#endif
                UpdatePreprocessorHeader(buildPath + "/Classes/Preprocessor.h");
            }
        }

        private static void UpdateProject(BuildTarget buildTarget, string projectPath) {
#if UNITY_IPHONE
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            string targetId = project.TargetGuidByName(PBXProject.GetUnityTargetName());

            // Required Frameworks
            project.AddFrameworkToProject(targetId, "AudioToolbox.framework", false);
            project.AddFrameworkToProject(targetId, "AVFoundation.framework", false);
            project.AddFrameworkToProject(targetId, "CoreGraphics.framework", false);
            project.AddFrameworkToProject(targetId, "CoreTelephony.framework", false);
            project.AddFrameworkToProject(targetId, "CoreMedia.framework", false);
            project.AddFrameworkToProject(targetId, "EventKit.framework", false);
            project.AddFrameworkToProject(targetId, "EventKitUI.framework", false);
            project.AddFrameworkToProject(targetId, "MediaPlayer.framework", false);
            project.AddFrameworkToProject(targetId, "MessageUI.framework", false);
            project.AddFrameworkToProject(targetId, "QuartzCore.framework", false);
            project.AddFrameworkToProject(targetId, "SystemConfiguration.framework", false);
            project.AddFrameworkToProject(targetId, "Security.framework", false);
            project.AddFrameworkToProject(targetId, "JavaScriptCore.framework", false);

            project.AddFileToBuild(targetId, project.AddFile("usr/lib/libz.1.2.5.dylib", "Frameworks/libz.1.2.5.dylib", PBXSourceTree.Sdk));

            // Optional Frameworks
            project.AddFrameworkToProject(targetId, "AdSupport.framework", true);
            project.AddFrameworkToProject(targetId, "Social.framework", true);
            project.AddFrameworkToProject(targetId, "StoreKit.framework", true);
            project.AddFrameworkToProject(targetId, "Webkit.framework", true);

            File.WriteAllText(projectPath, project.WriteToString());
#endif
        }

        private static void UpdateProjectPlist(BuildTarget buildTarget, string plistPath) {
#if UNITY_IPHONE
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            if (ADCConfig.Instance.PushNotificationSupport) {
                PlistElementDict root = plist.root;
                root.CreateArray("UIBackgroundModes").AddString("remote-notification");
            }

            if (ADCConfig.Instance.DeepLinkSupport && !string.IsNullOrEmpty(ADCConfig.Instance.IOSScheme)) {
                PlistElementDict root = plist.root;
                PlistElementArray urlTypes = root["CFBundleURLTypes"] as PlistElementArray;
                if (urlTypes == null) {
                    urlTypes = root.CreateArray("CFBundleURLTypes");
                }
                PlistElementDict adcolonyUrlScheme = urlTypes.AddDict();
                adcolonyUrlScheme.SetString("CFBundleTypeRole", "Editor");
                adcolonyUrlScheme.SetString("CFBundleURLName", "com.adcolony");
                PlistElementArray urlSchemes = adcolonyUrlScheme.CreateArray("CFBundleURLSchemes");
                urlSchemes.AddString(ADCConfig.Instance.IOSScheme);

            }

            File.WriteAllText(plistPath, plist.WriteToString());
            File.WriteAllText(plistPath + ".new", plist.WriteToString());
#endif
        }

        private static void UpdatePreprocessorHeader(string filePath) {
            if (!File.Exists(filePath)) {
                Debug.LogError("AdColony: Preprocessor file doesn't exist");
            }

            if (ADCConfig.Instance.PushNotificationSupport) {
                string[] AllLines = File.ReadAllLines(filePath);
                string[] AllNewLines = new string[AllLines.Length];

                for (int i = 0; i < AllLines.Length; i++) {
                    string line = AllLines[i];
                    if (line.Contains("UNITY_USES_REMOTE_NOTIFICATIONS")) {
                        AllNewLines[i] = "#define UNITY_USES_REMOTE_NOTIFICATIONS 1";
                    } else {
                        AllNewLines[i] = line;
                    }
                }

                File.WriteAllLines(filePath, AllNewLines);
            }
        }
    }
}

#endif
