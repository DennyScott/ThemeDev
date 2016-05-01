using UnityEngine;
using Zephyr.Singletons;

public abstract class Generator<T> : SimpleSingleton<T>, IGenerator where T : MonoBehaviour
{
}
