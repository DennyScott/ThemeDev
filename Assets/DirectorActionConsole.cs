using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class DirectorActionConsole : Manager<DirectorActionConsole>
{
    private Text _output;

    void Awake()
    {
        _output = GetComponent<Text>();
    }

    public void WriteActionToConsole(string action)
    {
        _output.text = _output.text + '\n' + "- " + action;
    }
}
