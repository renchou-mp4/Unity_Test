using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private T _instance;

    public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    System.Type type = typeof(T);
                    _instance = new GameObject(type.Name, type).GetComponent<T>();
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    protected virtual void Init() { }
    protected virtual void SingletonAwake() { }
    protected virtual void SingletonDestory() { }


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
        SingletonDestory();
        _instance = null;
    }
}
