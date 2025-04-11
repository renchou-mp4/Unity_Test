using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DialogLoading : UIBaseDialog,IUpdate
    {
        public Transform _loadingTf;

        public override void OnUIInit(object[] initArguments)
        {
            base.OnUIInit(initArguments);
            LifeCycleManager._Instance.Register(this);
        }

        public override void OnUIDestroy()
        {
            base.OnUIDestroy();
            LifeCycleManager._Instance.UnRegister(this);
        }

        public void OnUpdate()
        {
            _loadingTf.Rotate(new Vector3(0f, 0f, -Time.deltaTime * 20f));
        }
    }
}
