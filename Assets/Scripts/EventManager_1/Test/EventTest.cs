using AillieoUtils;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter
    }
    private void CallBack()
    {
        Debug.Log("eventDelegate add listener callback");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "Cube�¼�"))
        {
            EventManager.DispatchEvent("Cube", EventType.Cube_Test, 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "Cube1�¼�"))
        {
            EventManager.DispatchEvent("Cube1", EventType.Cube1_Test, 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 300, 100, 100), "Cube2�¼�"))
        {
            EventManager.DispatchEvent("Cube2", EventType.Cube2_Test, 1, 2.0f, 3d, "4");
        }

        if (GUI.Button(new Rect(100, 600, 100, 100), "�Ƴ�Cube�¼�"))
        {
            EventManager.RemoveEvent("Cube", EventType.Cube_Test);
        }
    }

}
