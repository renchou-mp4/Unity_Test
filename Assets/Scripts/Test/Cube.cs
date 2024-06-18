using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    Guid guid = Guid.NewGuid();

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddEvent(new EventData(EventType.Cube_Test, guid, "CallBack", CallBack));
        EventManager.AddEvent(new EventData(EventType.Cube1_Test, guid, "CallBack", CallBack));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CallBack(object[] args)
    {
        Debug.Log("cube1�¼��ص�");
    }

    private void RemoveCallBack(object[] args)
    {
        Debug.Log("�Ƴ�cube1�¼��ص�");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 400, 100, 100), "�Ƴ�Cube1�¼�"))
        {
            EventManager.RemoveEvent(new EventData(EventType.Cube1_Test, guid, "CallBack", RemoveCallBack));
        }
    }
}