using UnityEngine;
// ReSharper disable CheckNamespace

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T _Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                System.Type type = typeof(T);
                _instance = new GameObject(type.Name, type).GetComponent<T>();
            }
            _instance.SingletonInit();
            return _instance;
        }
    }

    protected virtual void SingletonInit() { }
    protected virtual void SingletonAwake() { }
    protected virtual void SingletonDestroy() { }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();
        }
        SingletonAwake();

    }

    private void OnDestroy()
    {
        SingletonDestroy();
        _instance = null;
    }
}
