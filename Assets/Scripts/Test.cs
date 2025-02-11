using Managers;
using Managers.LifeCycleManager;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour,IAwake,IUpdate
{
    public Image _image;

    private void Start()
    {
        // Sprite s = BundleManager._Instance.LoadAsset<Sprite>(
        //     GameDefine.IconPath.Sprites,
        //     "bg_wheel_of_fortune_box_open");
        //
        // _image.sprite = s;

        //BundleManager._Instance.LoadBundle("Sprites");
        //GameObject go = BundleManager._Instance.LoadAsset<GameObject>("TestObj");
        GameObject go = BundleManager._Instance.LoadAsset<GameObject>("TestObj");
        Transform  t  = GameObject.Find("Canvas").GetComponent<RectTransform>();
        Instantiate(go, t);
    }

    public void OnAwake()
    {
        LogTools.Log("Awake调用");
    }

    public void OnUpdate()
    {
        LogTools.Log("Update调用");
    }

    public void Register()
    {
        LifeCycleManager._Instance.Register(this);
    }

    public void Unregister()
    {
        LifeCycleManager._Instance.UnRegister(this);
    }
}