using System;
using System.Collections.Generic;
using UnityEditor;
using BuildOps.BuildActions;

namespace BuildOps
{
    public class BuildOps
    {
        //xvfb-run --server-args="-screen 0 1024x768x24" /opt/Unity/Editor/Unity -batchmode -logfile -force-opengl - quit -projectPath /var/lib/jenkins/workspace/ThemeDev/ -executeMethod BuildOps.PerformWindowsBuild

        static string[] SCENES = FindEnabledEditorScenes();

        static string APP_NAME = PlayerSettings.productName;
        static string TARGET_DIR = "/unity";

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
            string target_dir = APP_NAME + ".exe";
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

        private static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
        {
            BuildActionRunner.Instance.TriggerPreBuild();
            BuildActionRunner.Instance.TriggerOnBuild();
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            if (res.Length > 0)
            {
                throw new Exception("BuildPlayer failure: " + res);
            }
            BuildActionRunner.Instance.TriggerPostBuild();
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
}
