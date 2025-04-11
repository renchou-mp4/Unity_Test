using System.Collections;
using UI;
using UnityEngine;
using YooAsset;

namespace Managers
{
    public class GameManager : BaseManager<GameManager>
    {
        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(this);
            StartCoroutine(InitImportance());
        }

        //启动重要的程序
        private IEnumerator InitImportance()
        {
            yield return StartCoroutine(BundleManagerYooAsset._Instance.InitYooAssets()) ;
            
            //启动YooAssets后先将loading界面加载出来
            UIManager._Instance.OpenUI<DialogLoading>(UILayer.Dialog,null);
            //yield return StartCoroutine(LoadLoading());
            yield return new WaitForSeconds(5);
            LogTools.Log("关闭Loading");
            UIManager._Instance.CloseUI();
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