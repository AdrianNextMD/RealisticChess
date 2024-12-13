using NaughtyAttributes;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
#endif

using UnityEngine;
using UnityEngine.Assertions;

public class BuildsManager : MonoBehaviour
{
    [BoxGroup("Android Build")]
    public string keyName = "Assets/Template/GPlay/user.keystore";
    [BoxGroup("Android Build")]
    public string keyPassword;
    [BoxGroup("Android Build")]
    public string keyaliasName;
    [BoxGroup("Android Build")]
    public string keyaliasPass;
    [BoxGroup("Android Build")]
    public static string gameBundleID;

    public static string gameBundleId;
    public static string keystoreName = "Assets/Template/GPlay/user.keystore";
    public static string keystorePassword;
    public static string keyaliaName;
    public static string keyaliaPass;

    private void GetAndroidData()
    {
        keystoreName = keyName;
        keystorePassword = keyPassword;
        keyaliaName = keyaliasName;
        keyaliaPass = keyaliasPass;
        gameBundleID = gameBundleId;
    }

#if UNITY_EDITOR

    [Button()]
    public static void Build_iOS()
    {
        var path = Environment.GetEnvironmentVariable("BUILD_PATH");
        if (string.IsNullOrEmpty(path))
            return;

        PreBuild();
#if UNITY_IOS
            PlayerSettings.iOS.appleEnableAutomaticSigning = true;
            PlayerSettings.iOS.appleDeveloperTeamID = "HJ453WUND2";
#endif
        var b = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes
            , path, BuildTarget.iOS, BuildOptions.None);
        //PostBuildReport(b);
    }

    [Button()]
    public void Build_Android()
    {
        GetAndroidData();

        var path = Environment.GetEnvironmentVariable("BUILD_PATH") + "/AndroidBuild.apk";
        if (string.IsNullOrEmpty(path))
            return;

        PreBuild();
#if UNITY_ANDROID
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = keystoreName;
        PlayerSettings.Android.keystorePass = keystorePassword;
        PlayerSettings.Android.keyaliasName = keyaliaName;
        PlayerSettings.Android.keyaliasPass = keyaliaPass;
#endif
        var b = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes
            , path, BuildTarget.Android, BuildOptions.None);

        var fileOnFinish = Environment.GetEnvironmentVariable("UNITY_STATUS");
        if (b.summary.result == BuildResult.Succeeded)
        {
            Application.OpenURL("http://eloads.ddns.net/unity/build_status_send.php?name=Mad%20Racers&catch=Success!");
        }
        else
        {
            Application.OpenURL("http://eloads.ddns.net/unity/build_status_send.php?name=Mad%20Racers&catch=Error! > " + fileOnFinish);
        }
        //PostBuildReport(b);
    }

    private static void PreBuild()
    {
        Assert.IsTrue(EditorBuildSettings.scenes[0].path.Contains("Start"),
         "First Scene should be Start/Loading/FirstScene.unity");
        var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");

        //#if GEEKON_LIONSTUDIO
        //            PublisherIntegrator.SetIds();
        //            Assert.IsNotEmpty(LionStudios.LionSettings.Facebook.AppId, "Facebook is not set");
        //            Assert.IsNotEmpty(LionStudios.LionSettings.Adjust.Token, "Adjust is not set");
        //#endif

        var number = int.Parse(buildNumber ?? "0");
        PlayerSettings.bundleVersion = $"1.{number}";

        Assert.IsTrue(PlayerSettings.applicationIdentifier.Contains("com." + gameBundleId), "Bundle ID should be set!");

#if UNITY_ANDROID
        PlayerSettings.Android.bundleVersionCode = number;
#endif

#if UNITY_IOS
            PlayerSettings.iOS.buildNumber = number.ToString();
#endif
    }

    //    private static void PostBuildReport(BuildReport result)
    //    {
    //        var fileOnFinish = Environment.GetEnvironmentVariable("UNITY_STATUS");
    //        Debug.Log(fileOnFinish);

    //#if UNITY_ANDROID
    //        Debug.Log("BuildNumber: " + PlayerSettings.Android.bundleVersionCode);
    //#endif

    //#if UNITY_IOS
    //            Debug.Log("BuildNumber: " + PlayerSettings.iOS.buildNumber);
    //#endif

    //        if (result.summary.result == BuildResult.Succeeded)
    //        {
    //            File.WriteAllText(fileOnFinish, "Success");
    //        }
    //        else
    //        {
    //            File.WriteAllText(fileOnFinish, "Fail");
    //        }
    //    }
#endif

}
