#if UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class ADCUnityPostBuildProcessor {

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath) {
		if (buildTarget == BuildTarget.iOS) {
			UpdateProject(buildTarget, buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj");
		}
	}

	private static void UpdateProject(BuildTarget buildTarget, string projectPath) {
		PBXProject project = new PBXProject();
		project.ReadFromString(File.ReadAllText(projectPath));

		string targetId = project.TargetGuidByName(PBXProject.GetUnityTargetName());

		// Other Linker Flags - For some reason, the SDK in the sample app needs this to work.
		project.AddBuildProperty(targetId, "OTHER_LDFLAGS", "-ObjC");

		File.WriteAllText(projectPath, project.WriteToString());
	}
}

#endif