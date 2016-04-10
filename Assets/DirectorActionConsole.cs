using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DirectorActionConsole : Manager<DirectorActionConsole>
{
    private Text _output;

    /// <summary>
    /// Runs on Awake
    /// </summary>
    private void Awake()
    {
        _output = GetComponent<Text>();
    }

    /// <summary>
    /// Writes an action to Console
    /// </summary>
    /// <param name="actionType">The type of action that was done.  This will appear before the text describing what happened</param>
    /// <param name="action">The action that was just performed.</param>
    public void WriteActionToConsole(string actionType, string action)
    {
        _output.text = _output.text + '\n' + actionType.ToUpper() + ": " + action;
    }
}
