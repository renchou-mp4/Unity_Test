using System;
using System.Collections.Generic;
using System.Numerics;
using Managers;
using Tools;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    //各层级已打开的UI缓存
    //private static Dictionary<UILayer, > _openedUICache = new();
    
    private UIAnimManager _uiAnimManager = new();

    public static T OpenUI<T>(string prefabName,UILayer layer) where T : UIBase
    {
        if(prefabName.IsNullOrEmpty())
            prefabName = typeof(T).Name;
        
        //BundleManagerYooAsset._Instance.LoadAssetSync<T>()
        return null;
    }

    public static bool CloseUI<TUILayer>() where TUILayer : UIBase
    {
        return false;
    }
}