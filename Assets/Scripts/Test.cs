using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        AssetBundle ab = AssetBundle.LoadFromFile("D:/bg_wheel_of_fortune_box.png.ab");
        string[] names = ab.GetAllAssetNames();
        Sprite sprite = ab.LoadAsset<Sprite>(names[0]);
        _image.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
