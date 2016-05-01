using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

namespace BuildOps.SceneCompiler
{
  public class SceneCompiler
  {
    [MenuItem("Tools/Build Scene")]
    private static void Run()
    {
      Debug.Log(EditorSceneManager.sceneCount);
      var scenes = EditorSceneManager.GetAllScenes();

      for(var i = 0; i < scenes.Length; i++)
      {
        Debug.Log(scenes[i].name);
        Debug.Log(scenes[i].path);
      }
    }
  }
}
