using System.Collections;
using UI;
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
            yield return StartCoroutine(BundleManagerYooAsset._Instance.InitYooAssets()) ;
            
            //启动YooAssets后先将loading界面加载出来
            yield return StartCoroutine(LoadLoading());
        }

        private IEnumerator LoadLoading()
        {
            //加载loading界面
            var handle = BundleManagerYooAsset._Instance.LoadAssetAsync<GameObject>(nameof(DialogLoading));
            yield return handle;
            if (handle.Status != EOperationStatus.Succeed)
            {
                LogTools.LogError("DialogLoading加载失败！");
                yield break;
            }

 
            handle.InstantiateSync(GameObject.Find("Canvas").transform);
        }
    }
}