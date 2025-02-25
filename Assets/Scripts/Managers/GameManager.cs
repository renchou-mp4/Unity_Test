using System.Collections;
using UI;
using UnityEngine;
using YooAsset;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private GameObject _dialogLoading = null;
        
        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            StartCoroutine(InitImportance());
            

            
            
        }

        //启动重要的程序
        private IEnumerator InitImportance()
        {
            yield return StartCoroutine(BundleManagerYooAsset._Instance.InitYooAssets()) ;
            
            //启动YooAssets后先将loading界面加载出来
            yield return StartCoroutine(LoadLoading());
        }

        private IEnumerator LoadLoading()
        {
            var handle = BundleManagerYooAsset._Instance.LoadAssetAsync<DialogLoading>(nameof(DialogLoading));
            yield return handle;
            if (handle.Status != EOperationStatus.Succeed)
            {
                LogTools.LogError("DialogLoading加载失败！");
                yield break;
            }

            _dialogLoading = handle.InstantiateSync(GameObject.Find("Canvass").transform);
        }
        
        private IEnumerator InitYooAsset()
        {
            //初始化资源系统
            YooAssets.Initialize();
            
            //设置默认Package
            BundleManagerYooAsset._Instance._Package = YooAssets.CreatePackage("TestPackage");
            YooAssets.SetDefaultPackage(BundleManagerYooAsset._Instance._Package);

            // _loading.gameObject.SetActive(true);
            // yield return BundleManagerYooAsset._Instance.InitPackage();
            // _loading.gameObject.SetActive(false);
            // AssetHandle handle = BundleManagerYooAsset._Instance.LoadAssetAsync<GameObject>("TestObj");
            // yield return handle;
            // handle.InstantiateSync(_canvas.transform);
            
            yield return null;
        }
    }
}