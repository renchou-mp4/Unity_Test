using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public  Canvas        _canvas;
        public DialogLoading _loading;

        private void Awake()
        {
            StartCoroutine(InitPackage());
        }

        private IEnumerator InitPackage()
        {
            _loading.gameObject.SetActive(true);
            yield return BundleManagerYooAsset._Instance.InitPackage();
            _loading.gameObject.SetActive(false);
            AssetHandle handle = BundleManagerYooAsset._Instance.LoadAssetAsync<GameObject>("TestObj");
            yield return handle;
            handle.InstantiateSync(_canvas.transform);
        }
    }
}