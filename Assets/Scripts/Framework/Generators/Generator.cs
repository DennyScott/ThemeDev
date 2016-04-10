using UnityEngine;

public abstract class Generator<T> : Singleton<T>, IGenerator where T : MonoBehaviour
{
}
