using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using YooAsset;

namespace Managers
{
    // public class UIData
    // {
    //     /// <summary>
    //     /// UI脚本引用
    //     /// </summary>
    //     public UIBase _UI { get; set; }
    //     /// <summary>
    //     /// UI界面加载的预制体
    //     /// </summary>
    //     public GameObject _Prefab { get; set; }
    //     /// <summary>
    //     /// UI层级
    //     /// </summary>
    //     public UILayer _Layer { get; set; }
    //     /// <summary>
    //     /// UI界面加载的预制体名称
    //     /// </summary>
    //     public string _PrefabName { get; set; }
    //     /// <summary>
    //     /// 该UI界面的父节点
    //     /// </summary>
    //     public GameObject _UICanvas { get; set; }
    // }
    
    public class UIStack
    {
        /// <summary>
        /// 已打开的界面缓存
        /// </summary>
        private readonly List<UIBase> _uiStack = new();
        /// <summary>
        /// 栈顶指针
        /// </summary>
        private UIBase _topPointer;
        
        /// <summary>
        /// 判断是否已打开某个界面
        /// </summary>
        /// <param name="uiName">ui脚本名称</param>
        /// <returns></returns>
        public bool Contains(string uiName)
        {
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            return _uiStack.Any(x=>
            {
                if (x is null) throw new ArgumentNullException(nameof(x));
                return nameof(x) == uiName;
            });
        }

        /// <summary>
        /// 判断是否已打开某个界面,并返回该界面之后入栈的界面列表
        /// </summary>
        /// <param name="uiName">ui脚本名称</param>
        /// <param name="frontUI">该界面之后入栈的界面列表</param>
        /// <returns></returns>
        private bool Contains(string uiName,out List<UIBase> frontUI)
        {
            frontUI = new List<UIBase>();
            foreach (var uiBase in _uiStack)
            {
                if (uiBase.ToString() == uiName)
                    return true;
                frontUI.Add(uiBase);
            }
            frontUI = null;
            return false;
        }
        
        public UIBase Peek()
        {
            return _uiStack.Count == 0 ? null : _topPointer;
        }
        
        public bool TryPeek(out UIBase targetUI)
        {
            targetUI = Peek();
            return _uiStack.Count > 0;
        }
        
        public UIBase Pop()
        {
            if(_topPointer is null) return null;

            UIBase popUI = _topPointer;
            _uiStack.Remove(_topPointer);
            _topPointer.OnUIClose();
            _topPointer.OnUIDestroy();
            _topPointer = _uiStack.Count > 0 ? _uiStack[^1] : null;
            _topPointer?.OnUIShow();
            return popUI;
        }
        
        public UIBase Push<T>(UILayer layer,string prefabName,params object[] initArguments) where T : UIBase
        {
            if(prefabName.IsNullOrEmpty())
                prefabName = typeof(T).Name;
            
            if (Contains(typeof(T).Name,out var frontUI))
            {
                for (int i = 0; i < frontUI.Count; i++)
                    Pop();
                return Peek();
            }
            
            //加载新界面
            AssetHandle handle = BundleManagerYooAsset._Instance.LoadAssetSync<GameObject>(prefabName);
            if(handle.AssetObject is null)
                throw new NullReferenceException();

            Transform  parentTransform = UIManager._Instance._UICanvas[layer.ToString()]?.transform;
            GameObject uiObj           = handle.InstantiateSync(parentTransform);
            UIBase     uiBase          = uiObj?.GetComponent<UIBase>();

            _uiStack.Add(uiBase);
            _topPointer?.OnUIHide();
            _topPointer = uiBase;
            _topPointer?.OnUIInit(initArguments);
            _topPointer?.OnUIOpen();
            return _topPointer;
        }

        
    }
}