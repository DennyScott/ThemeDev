using UnityEditor;
using UnityEngine;
using System.Xml.Serialization;

namespace BuildOps.SceneCompiler
{
  public class Scene
  {
    [XmlAttribute("Path")]
    public string Path;

    [XmlAttribute("Name")]
    public string Name;
  }
}
