using UI;
using System.Collections;
using UnityEngine;

namespace Managers
{
    [ProcessState(ProcessFSMState.Launch)]
    // ReSharper disable once InconsistentNaming
    public class ProcessFSM_LaunchState : IState<ProcessFSMState>
    {
        public ProcessFSMState _State { get; set; } = ProcessFSMState.Launch;

        public void OnEnterState()
        {
            
        }

        public void OnUpdateState()
        {
            
        }

        public void OnExitState()
        {
            
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
    }
}