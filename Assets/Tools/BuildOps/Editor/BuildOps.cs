using System;
using System.Collections.Generic;
using UnityEditor;

public class BuildOps
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "ThemeDev";
        static string TARGET_DIR = "/unity/";

    static void PerformBuild()
    {
        GenericBuild(
            SCENES,
            GetArg("-targetPath"),
            (BuildTarget)Enum.Parse(typeof(BuildTarget), GetArg("-buildTarget")),
            (BuildOptions)Enum.Parse(typeof(BuildOptions), GetArg("-buildOptions") ?? "None"));
    }

    public static void PerformWindowsBuild()
    {
        string target_dir = APP_NAME + ".app";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }

    // Helper function for getting the command line arguments
    // Taken from: https://effectiveunity.com/articles/making-most-of-unitys-command-line.html
    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}