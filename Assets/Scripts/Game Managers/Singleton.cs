using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T Instance = null;

    protected Singleton()
    {
    }

    /// <summary>
    /// Gets the instance
    /// </summary>
    protected virtual void Awake()
    {
        GetInstance();
    }

    /// <summary>
    /// Destroys the instance
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Getter
    public static T GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
        }

        return Instance;
    }

    /// <summary>
    /// Checks if the singleton is instantiated
    /// </summary>
    /// <returns></returns>
    public static bool IsInstantiated()
    {
        return Instance != null;
    }
}
