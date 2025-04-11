using UnityEngine;

namespace Managers
{
    //UI生命周期管理
    public class UIBase : MonoBehaviour
    {
        /// <summary>
        /// UI层级
        /// </summary>
        public UILayer _Layer { get; set; }
        
        //UI行为管理
        public virtual void OnUIInit(object[] initArguments)    {}
        public virtual    void OnUIDestroy() {}
        public virtual    void OnUIOpen()    {}
        public virtual    void OnUIClose()   {}
        public virtual    void OnUIShow()    {}
        public virtual    void OnUIHide()    {}
    }
}