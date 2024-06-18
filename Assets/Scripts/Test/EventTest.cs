using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "Cube事件"))
        {
            EventManager.DispatchEvent(new EventData(EventType.Cube_Test, null), 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "Cube1事件"))
        {
            EventManager.DispatchEvent(new EventData(EventType.Cube1_Test, null), 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 300, 100, 100), "Cube2事件"))
        {
            EventManager.DispatchEvent(new EventData(EventType.Cube2_Test, null), 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 600, 100, 100), "移除Cube事件"))
        {
            EventManager.RemoveEvent(new EventData(EventType.Cube_Test));
        }
    }
}
