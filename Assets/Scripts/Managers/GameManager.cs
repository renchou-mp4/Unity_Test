using System.Collections;
using UnityEngine;
using YooAsset;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            StartCoroutine(InitImportance());
        }

        //启动重要的程序
        private IEnumerator InitImportance()
        {
            yield return StartCoroutine(InitYooAsset()) ;
        }
        
        private IEnumerator InitYooAsset()
        {
            //初始化资源系统
            YooAssets.Initialize();
            yield return null;
            //设置默认Package
            // _package = YooAssets.CreatePackage("TestPackage");
            // YooAssets.SetDefaultPackage(_package);

            // _loading.gameObject.SetActive(true);
            // yield return BundleManagerYooAsset._Instance.InitPackage();
            // _loading.gameObject.SetActive(false);
            // AssetHandle handle = BundleManagerYooAsset._Instance.LoadAssetAsync<GameObject>("TestObj");
            // yield return handle;
            // handle.InstantiateSync(_canvas.transform);
        }
    }
}