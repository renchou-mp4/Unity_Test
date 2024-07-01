using QFramework;
using UnityEngine;

public class TypeEventSystemEvent : MonoBehaviour
{
    private void Start()
    {
        TypeEventSystem.Global.Register<TypeEvent>((e) =>
        {
            Debug.Log(gameObject.name + e.Count);
        }).UnRegisterWhenGameObjectDestroyed(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TypeEventSystem.Global.Send(new TypeEvent
            {
                Count = 10
            });
        }
    }
}
