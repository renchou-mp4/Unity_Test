using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Image _image;
    void Start()
    {
        // Sprite s = BundleManager._Instance.LoadAsset<Sprite>(
        //     GameDefine.IconPath.Sprites,
        //     "bg_wheel_of_fortune_box_open");
        //
        // _image.sprite = s;

        //BundleManager._Instance.LoadBundle("Sprites");
        GameObject go = BundleManager._Instance.LoadAsset<GameObject>("Prefabs/TestObj", "TestObj");
        Transform t = GameObject.Find("Canvas").GetComponent<RectTransform>();
        Instantiate(go, t);
    }
}

