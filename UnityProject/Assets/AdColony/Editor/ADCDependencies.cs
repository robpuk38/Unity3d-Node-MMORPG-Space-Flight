#if UNITY_ANDROID

using Google.JarResolver;
using UnityEditor;
using GooglePlayServices;

namespace AdColony.Editor {
    [InitializeOnLoad]
    public static class ADCDependencies {
        public static PlayServicesSupport svcSupport;

        static ADCDependencies() {
            svcSupport = PlayServicesSupport.CreateInstance(ADCPluginInfo.Name, EditorPrefs.GetString("AndroidSdkRoot"), "ProjectSettings");
            RegisterDependencies();
        }

        public static void RegisterDependencies() {
            svcSupport.DependOn("com.google.android.gms", "play-services-ads", "9.0.1");

            if (ADCConfig.Instance.PushNotificationSupport) {
                svcSupport.DependOn("com.google.android.gms", "play-services-gcm", "9.0.1");
            }

            svcSupport.DependOn("com.android.support", "appcompat-v7", "23.4.0");
        }

        public static void ResetDependencies() {
            svcSupport.ClearDependencies();
            RegisterDependencies();
        }
    }
}

#endif
