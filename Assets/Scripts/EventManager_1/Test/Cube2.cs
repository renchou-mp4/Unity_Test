using UnityEngine;

public class Cube2 : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddEvent("Cube", EventType.Cube_Test, CallBack);
        EventManager.AddEvent("Cube2", EventType.Cube2_Test, CallBack);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CallBack(object[] args)
    {
        Debug.Log("cube2事件回调");
    }

    private void RemoveCallBack(object[] args)
    {
        Debug.Log("移除cube2事件回调");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 500, 100, 100), "移除Cube2事件"))
        {
            EventManager.RemoveEvent("Cube2", EventType.Cube2_Test);
        }
    }
}
