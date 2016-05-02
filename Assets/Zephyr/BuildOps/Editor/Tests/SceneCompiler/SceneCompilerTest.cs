using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using NUnit.Framework;

namespace BuildsOps.Tests.SceneCompiler
{
    [TestFixture]
    [Category("Scene Compiler Tests")]
    public class SceneCompilerTest
    {

        [SetUp]
        public void Init()
        {
            
        }

        [Test]
        public void DoesSceneCountCollectAllCurrentScenes()
        {
//            var script = MonoScript.FromScriptableObject(this);
//            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(script));
            Debug.Log(EditorApplication.applicationContentsPath);
        }

        [Test]
        public void EditorTest()
        {
            //Arrange
            var gameObject = new GameObject();

            //Act
            //Try to rename the GameObject
            var newGameObjectName = "My game object";
            gameObject.name = newGameObjectName;

            //Assert
            //The object has a new name
            Assert.AreEqual(newGameObjectName, gameObject.name);
        }
    }
}
