using UnityEngine;

namespace Managers
{
    //UI生命周期管理
    public class UIBase : MonoBehaviour
    {
        protected virtual void OnUIInit(){}
        protected virtual void OnUIDestroy(){}
        protected virtual void OnUIOpen(){}
        protected virtual void OnUIClose(){}
        protected virtual void OnUIShow(){}
        protected virtual void OnUIHide(){}
    }
}