using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : BaseManager<UIManager>
    {
        //已打开的UI缓存
        private readonly UIStack _openedUICache = new();

        public Dictionary<string, GameObject> _UICanvas { get; private set; } = new();

        private UIAnimManager _uiAnimManager = new();

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            //创建各层级Canvas的GameObject
            foreach (var layerName in Enum.GetNames(typeof(UILayer)))
            {
                GameObject canvas = new GameObject(layerName, typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.transform.SetParent(transform);
                _UICanvas[layerName] = canvas;
            }
        }

        public T OpenUI<T>(UILayer layer,string prefabName,params object[] initArguments) where T : UIBase
        {
            UIBase ui = _openedUICache.Push<T>(layer, prefabName,initArguments);
            
            return ui as T;
        }                       

        public bool CloseUI()
        {
            UIBase ui     = _openedUICache.Pop();
            bool   result = ui is not null;
            //TODO: 应该在动画结束后再关闭
            Destroy(ui?.gameObject);
            
            return result;
        }
    }
}