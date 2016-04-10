using UnityEngine;

/// <summary>
/// Base Manager. All Managers should inherit from this class.
/// </summary>
public abstract class Manager<T> : Singleton<T>, IManager where T : MonoBehaviour
{
}
