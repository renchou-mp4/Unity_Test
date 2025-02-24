using UnityEngine;

namespace Managers
{
    public class BaseManager<T> : MonoSingleton<T> where T : BaseManager<T>
    {
        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            Transform parentTransform = GameObject.FindGameObjectWithTag("Managers")?.GetComponent<Transform>();
            if (parentTransform != null)
                transform.parent = parentTransform;
        }
    }
}
