using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Zephyr.BuildOps.SceneCompiler;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Zephyr.BuildOps.Tests.SceneCompilerTest
{
    [TestFixture]
    public class SceneReaderTest
    {
        private static readonly string TestFolder = Settings.BuildOps + "Editor/Tests/";
        private static readonly string SceneCompilerPath = TestFolder + "Scenes/";
        private static readonly string XmlTestFile = SceneCompilerPath + "testFile.xml";
        private static readonly string sceneContainerXML = TestFolder + "Fixtures/Resources/testData.xml";
        private static readonly string platformContainerXML = TestFolder + "Fixtures/Resources/testData_build.xml";
        private static readonly string BaseScene = SceneCompilerPath + "Base.unity";
        private static readonly string SceneWithCube = SceneCompilerPath + "SceneWithCube.unity";
        private static readonly string SceneWithSphere = SceneCompilerPath + "SceneWithSphere.unity";
        private static readonly string platform = "mobile";
        private SceneReader _reader;

        [SetUp]
        public void Init()
        {
            _reader = new SceneReader();
        }

        [Test]
        [Category("LoadSceneContainerFromXml")]
        public void DoesLoadContainerReadFromXml()
        {
            //Act
            var container = _reader.LoadSceneContainerFromXml(sceneContainerXML);

            //Assert
            Assert.AreEqual(2, container.Scenes.Count);
        }

        [Test]
        [Category("LoadSceneContainerFromXml")]
        public void DoesLoadContainerNotReturnNull()
        {
            //Act
            var container = _reader.LoadSceneContainerFromXml(sceneContainerXML);

            //Assert
            Assert.IsNotNull(container);
        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateCorrectNumberOfNestedScenes()
        {
            //Arrange
            var container = _reader.LoadSceneContainerFromXml(sceneContainerXML);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(2, EditorSceneManager.sceneCount);
        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateBaseScene()
        {
            //Arrange
            var container = _reader.LoadSceneContainerFromXml(sceneContainerXML);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(container.Scenes[0].Name, EditorSceneManager.GetSceneAt(0).name);

        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateNestedScene()
        {
            //Arrange
            var container = _reader.LoadSceneContainerFromXml(sceneContainerXML);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(container.Scenes[1].Name, EditorSceneManager.GetSceneAt(1).name);

        }

        [Test]
        [Category("LoadPlatformContainerFromXML")]
        public void DoesLoadPlatformContainerCreateScenes()
        {
            //Act
            var container = _reader.LoadPlatformContainerFromXml(platformContainerXML);

            //Assert
            Assert.AreEqual(3, container.Scenes.Count);
            
        }

        [Test]
        [Category("CollectBuildSceneSettings")]
        public void DoesCollectBuildSceneSettingsCreateCorrectAmountOfScenes()
        {
            //Arrage
            var container = _reader.LoadPlatformContainerFromXml(platformContainerXML);

            //Act
            var scenes = _reader.CollectBuildSceneSettings(container);

            //Assert
            Assert.AreEqual(3, scenes.Length);
        }

        [Test]
        [Category("CollectBuildSceneSettings")]
        public void DoesCollectSettingsHaveCorrectPaths()
        {
            //Arrage
            var container = _reader.LoadPlatformContainerFromXml(platformContainerXML);

            //Act
            var scenes = _reader.CollectBuildSceneSettings(container);

            //Assert
            Assert.AreEqual(container.Scenes[0].Scenes[0].Path, scenes[0].path);
            Assert.AreEqual(container.Scenes[1].Scenes[0].Path, scenes[1].path);
        }

        [Test]
        [Category("LoadBuildSettings")]
        public void DoesLoadBuildSettingsUpdateBuildSettingScenes()
        {
            //Act
            _reader.LoadBuildSettings(sceneContainerXML);

            //Assert
            Assert.AreEqual(BaseScene, EditorBuildSettings.scenes[0].path);
        }

        [Test]
        [Category("LoadScene")]
        public void DoesLoadSceneLoadCorrectNumberOfScenes()
        {
            //Act
            _reader.LoadScene(sceneContainerXML);

            //Assert
            Assert.AreEqual(2, EditorSceneManager.sceneCount);
        }
    }
}
