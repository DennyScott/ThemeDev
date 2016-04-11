using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T InnerInstance;

	/**
	   Returns the instance of this singleton.
	*/
	public static T Instance {
		get {
			if(InnerInstance != null)
			{
				return InnerInstance;
			}

			InnerInstance = (T)FindObjectOfType(typeof(T));

			if(InnerInstance == null)
			{
				Debug.LogWarning("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
			}

			return InnerInstance;
		}
	}
}
