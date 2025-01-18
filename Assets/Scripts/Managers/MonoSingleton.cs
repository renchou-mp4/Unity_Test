using UnityEngine;

// ReSharper disable CheckNamespace

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    // ReSharper disable once StaticMemberInGenericType
    private static bool _isInit; //泛型中，不同子类的静态变量不共享。例：AssetManager.isInit与BundleManager.isInit不共享
    private static T    _instance;

    public static T _Instance
    {
        get
        {
            if (_instance != null)
            {
                if (!_isInit) _instance.SingletonInit();
                return _instance;
            }

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


    private void Awake()
    {
        if (_instance == null) _instance = GetComponent<T>();
        SingletonAwake();
    }

    private void OnDestroy()
    {
        SingletonDestroy();
        _instance = null;
    }

    protected virtual void SingletonInit()
    {
        _isInit = true;
    }

    protected virtual void SingletonAwake()
    {
    }

    protected virtual void SingletonDestroy()
    {
    }
}