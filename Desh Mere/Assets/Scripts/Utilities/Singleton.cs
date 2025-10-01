using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindFirstObjectByType<T>();

                if(instance == null)
                {
                    Debug.LogError($"Instance {typeof(T)} not found. Please create one!");
                }
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public void Awake()
    {
        if( Instance != null && Instance != this as T)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}