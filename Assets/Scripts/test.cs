using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddEvent(null, "testEventName", Callback);
        EventManager.AddEvent(null, "testEventName", Callback);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Callback(object[] objs)
    {
        LogManager.Log("Callback");
    }
}
