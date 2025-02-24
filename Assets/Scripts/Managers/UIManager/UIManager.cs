using System.Collections.Generic;
using System.Numerics;
using Managers;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    //各层级已打开的UI缓存
    private Dictionary<string, UIBase> _dialogCache = new();
    private Dictionary<string, UIBase> _popupCache = new();
    
    private UIAnimManager _uiAnimManager = new();

    public static TUILayer OpenUI<TUILayer>() where TUILayer : UIBase
    {
        
        return null;
    }

    public static bool CloseUI<TUILayer>() where TUILayer : UIBase
    {
        return false;
    }
}