using UnityEngine;
using Zephyr.Singletons;

/// <summary>
/// Base Manager. All Managers should inherit from this class.
/// </summary>
public abstract class Manager<T> : SimpleSingleton<T>, IManager where T : MonoBehaviour
{
}
