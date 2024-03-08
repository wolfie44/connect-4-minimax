using UnityEngine;

// Utils
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T Instance = null;
    protected virtual void Awake()
    {
        GetInstance();
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public static T GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
        }

        return Instance;
    }
}
