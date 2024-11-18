using GameDefineSpace;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Image _image;
    void Start()
    {
        Sprite s = BundleManager._Instance.LoadAsset<Sprite>(
            GameDefine.IconPath.Test,
            "bg_wheel_of_fortune_box_open");

        _image.sprite = s;
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Bundle/Sprites/Test.ab");
        //string[] names = ab.GetAllAssetNames();
        //Sprite s = ab.LoadAsset(names[1], typeof(Sprite)) as Sprite;
        //_image.sprite = s;
    }
}
