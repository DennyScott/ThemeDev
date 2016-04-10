  using System;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Returns a list of all Transforms that are children of this transform
    /// </summary>
    /// <param name="t">The transform to search</param>
    /// <returns>A list of all transforms that are children of this transform</returns>
    public static Transform[] GetChildren(this Transform t)
    {
        var returnArray = new Transform[t.childCount];
        for (var i = 0; i < t.childCount; i++)
        {
            returnArray[i] = t.GetChild(i);
        }
        return returnArray;
    }

    /// <summary>
    /// Returns a list of all gameobjects that are children of this Transform
    /// </summary>
    /// <param name="t">The transform to search</param>
    /// <returns>A List of all gameobjects that are children of this transform</returns>
    public static GameObject[] GetChildrenAsGameObjects(this Transform t)
    {
        var returnArray = new GameObject[t.childCount];
        for (var i = 0; i < t.childCount; i++)
        {
            returnArray[i] = t.GetChild(i).gameObject;
        }
        return returnArray;
    }

    /// <summary>
    /// Finds an immediate child of this transform by a given tag
    /// </summary>
    /// <param name="t">The transform to extend</param>
    /// <param name="tag">The tag to search by</param>
    /// <returns>The found gameobject</returns>
    public static Transform FindChildByTag(this Transform t, string tag)
    {
        for (var i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i);
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }

    public static Transform FindRootWithComponent<T>(this Transform t, int maxDepth = 20) where T : Component
    {
        var currentTransform = t;
        var index = 0;
        while (currentTransform != null && index < maxDepth)
        {
            var component = currentTransform.GetComponent<T>();
            if (component != null)
            {
                return currentTransform;
            }
            currentTransform = currentTransform.parent;
            index++;
        }

        return null;
    }

    /// <summary>
    /// Calls the delegate if it is not null
    /// </summary>
    /// <param name="action">The action to be called</param>
    /// <param name="go">The generic object passed</param>
    public static void Run<T>(this Action<T> action, T go)
    {
        if (action != null)
        {
            action(go);
        }
    }


    /// <summary>
    /// Calls the delegate if it is not null
    /// </summary>
    /// <param name="action">The action to be called</param>
    public static void Run(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    #region Func Extensions

    /// <summary>
    /// Run the specified func and returns true if all values where true.
    /// </summary>
    /// <param name="func">The Func to perform this on</param>
    public static bool Run(this Func<bool> func)
    {
        if (func == null)
        {
            return false;
        }
        // Store all attached Functions to this Func in an Array
        var functions = func.GetInvocationList();

        // For each Func in the Func Array...
        for (var i = 0; i < functions.Length; i++)
        {
            // ...Get the inidividul function at position i...
            var function = (Func<bool>) functions[i];

            // ...If the function attached returns false...
            if (function() == false)
            {
                // ...Then return false, as no single function must false for this function to return true.
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Run the specified func with the passedParameter and return true if all funcs returned true.
    /// </summary>
    /// <param name="func">Func to Run</param>
    /// <param name="passedParameter">Passed parameter to include with the Func as a parameter</param>
    /// <typeparam name="T">The type of parameter and first Func value.</typeparam>
    public static bool Run<T>(this Func<T, bool> func, T passedParameter)
    {
        // Store all attached Functions to this Func in an Array
        var functions = func.GetInvocationList();

        // For each Func in the Func Array...
        for (var i = 0; i < functions.Length; i++)
        {
            // ...Get the inidividul function at position i...
            var function = (Func<T, bool>) functions[i];

            // ...If the function attached returns false...
            if (function(passedParameter) == false)
            {
                // ...Then return false, as no single function must false for this function to return true.
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calls the delegate if it is not null and returns true if the func is null or if all invocations return true
    /// </summary>
    /// <param name="func">The func to be called</param>
    public static bool RunOrIsNull(this Func<bool> func)
    {
        // If the Func that called this Method is null...
        return func == null || func.Run();
    }

    /// <summary>
    /// Calls the delegate if it is not null and returns true if the func is null or if all invocations return true
    /// </summary>
    /// <param name="func">The func to be called</param>
    /// <param name="passedParameter">The passed parameter of the Func</param>
    public static bool RunOrIsNull<T>(this Func<T, bool> func, T passedParameter)
    {
        return func == null || func.Run(passedParameter);
    }

    #endregion

    /// <summary>
    /// Sets the alpha of this image to the passed value.
    /// </summary>
    /// <param name="image">Image to change the alpha of.</param>
    /// <param name="val">Value to change alpha to, between 1 and 0.</param>
    public static void SetAlpha(this UnityEngine.UI.Image image, float val)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, val);
    }


    /// <summary>
    /// Destroys the passed gameobject as long as it exists
    /// </summary>
    /// <param name="g"></param>
    public static void SafeDestroy(this GameObject g)
    {
        if (g != null)
        {
            UnityEngine.Object.Destroy(g);
        }
    }

    /// <summary>
    /// Sets the parent of the given gameobject as the other gameobject passed
    /// </summary>
    /// <param name="g">The gameobject to set the parent of</param>
    /// <param name="parent">The parent gameobject</param>
    public static void SetNewParent(this Transform t, Transform parent)
    {
        t.SetParent(parent, false);
    }

    /// <summary>
    /// Sets the parent of the given gameobject as the other gameobject passed
    /// </summary>
    /// <param name="g">The gameobject to set the parent of</param>
    /// <param name="parent">The parent gameobject</param>
    public static void SetNewParent(this GameObject g, GameObject parent)
    {
        g.transform.SetParent(parent.transform, false);
    }

    /// <summary>
    /// Sets the x position of the transform to the float passed
    /// </summary>
    /// <param name="t">The transform to set</param>
    /// <param name="x">The new x value</param>
    public static void SetPositionX(this Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
    }

    /// <summary>
    /// Sets the y position of the transform to the float passed
    /// </summary>
    /// <param name="t">The transform to set</param>
    /// <param name="y">The new y value</param>
    public static void SetPositionY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, y, t.position.z);
    }

    /// <summary>
    /// Sets the z position of the transform to the float passed
    /// </summary>
    /// <param name="t">The transform to set</param>
    /// <param name="z">The new z value</param>
    public static void SetPositionZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, z);
    }
}