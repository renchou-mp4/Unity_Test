using UnityEngine;

public class Cube : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddEvent(EventData.Add(EventType.Cube_Test, this.name, CallBack));
        EventManager.AddEvent(EventData.Add(EventType.Cube1_Test, this.name, CallBack));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CallBack(object[] args)
    {
        Debug.Log("cube1事件回调");
    }

    private void RemoveCallBack(object[] args)
    {
        Debug.Log("移除cube1事件回调");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 400, 100, 100), "移除Cube1事件"))
        {
            EventManager.RemoveEvent(EventData.Remove(EventType.Cube1_Test, this.name, CallBack));
        }
    }
}
