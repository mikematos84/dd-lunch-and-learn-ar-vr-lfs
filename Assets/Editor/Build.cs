using UnityEditor;
using System.Diagnostics;
using System.Collections.Generic;

public class Build
{   
    protected static string path = "Builds";
    protected static string filename = "DDLunchAndLearnARVR";

    public static string buildPath
    {
        get
        {
            return string.Format("{0}/{1}.apk", path, filename);
        }
    }

    [MenuItem("Custom Menu/Development")]
    public static void DevelopmentBuild()
    {
        UnityEngine.Debug.Log("!!! Development Build Initialized");
        string[] levels = new string[] 
        { 
            "Assets/Oculus/VR/Scenes/GearVrControllerTest.unity"
        };

        // Build player.
        BuildPipeline.BuildPlayer(levels, buildPath, BuildTarget.Android, BuildOptions.AutoRunPlayer);

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = buildPath;
        proc.Start();
    }

    [MenuItem("Custom Menu/Production")]
    public static void ProductionBuild(){
        UnityEngine.Debug.Log("!!! Production Build Initialized");
    }
}