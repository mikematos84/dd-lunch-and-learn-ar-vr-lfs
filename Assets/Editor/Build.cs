using UnityEditor;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

public class Build
{
    const string COMPANY_NAME = "Deloitte Digital";
    const string PRODUCT_NAME = "Lunch and Learn AR VR";
    const string APP_ID = "com.dd.lalarvr";
    const string VERSION = "1.0.1";
    const string RELATIVE_PATH = "/../Builds";
    
    [MenuItem("Custom Menu/Remove builds from device")]
    public static void RemoveBuildFromDevice()
    {
        Process.Start("adb", "devices");
        UnityEngine.Debug.Log(string.Format("<color=red>{0}</color>", "Removing builds from device"));
        string[] tags = new string[] { "Dev", "Test", "Prod", "" };
        foreach (string tag in tags)
        {
            string app = APP_ID + tag;
            Process.Start("adb", String.Format("uninstall {0}", app));
            UnityEngine.Debug.Log(string.Format("<color=red>Removed {0} from device</color>", app));
        }
    }
    
    static void ProcessBuild(string tag, string[] levels)
    {
        UnityEngine.Debug.Log(string.Format("<color=blue>{0}</color>", "Building to device... Please wait."));

        // Setup
        string datestamp = Regex.Replace(DateTime.Now.ToLongDateString().ToString(), @"[/]", "-");
        string filename = String.Format("{0}-{1}-{2}.apk", PRODUCT_NAME.Replace(" ", ""), tag.Replace(" ", ""), datestamp);
        string buildPath = String.Format("{0}/{1}", UnityEngine.Application.dataPath + RELATIVE_PATH, filename);

        // Update Player Settings
        PlayerSettings.companyName = COMPANY_NAME;
        PlayerSettings.productName = String.Format("{0} {1}", PRODUCT_NAME, tag); ;
        PlayerSettings.applicationIdentifier = APP_ID + tag;
        PlayerSettings.bundleVersion = VERSION;
        PlayerSettings.Android.bundleVersionCode = int.Parse(VERSION.Replace(".", ""));

        // Build player.
        BuildPipeline.BuildPlayer(levels, buildPath, BuildTarget.Android, BuildOptions.AutoRunPlayer);

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = buildPath;
        proc.Start();
        UnityEngine.Debug.Log(string.Format("<color=blue>Completed Build at {0}</color>", DateTime.Now.ToUniversalTime()));

    }
    static void ProcessBuild(string tag) { ProcessBuild(tag, new string[] { }); }

    [MenuItem("Custom Menu/Build/Dev")]
    public static void BuildDev()
    {
        UnityEngine.Debug.Log(string.Format("<color=blue>Started {0} Build at {1}</color>", "Development", DateTime.Now.ToUniversalTime()));
        ProcessBuild("Dev", new string[]
        {
            "Assets/Scenes/SampleScene.unity"
        });
    }

    [MenuItem("Custom Menu/Build/Test")]
    public static void BuildTest()
    {
        UnityEngine.Debug.Log(string.Format("<color=blue>Started {0} Build  at {1}</color>", "Test", DateTime.Now.ToUniversalTime()));
    }

    [MenuItem("Custom Menu/Build/Production")]
    public static void BuildProd()
    {
        UnityEngine.Debug.Log(string.Format("<color=blue>Started {0} Build  at {1}</color>", "Production", DateTime.Now.ToUniversalTime()));
    }
}