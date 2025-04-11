using System;
using UnityEngine;

namespace Managers
{
    // ReSharper disable once InconsistentNaming
    public abstract class FSMBase<TState> :Component,IUpdate,IDestroy where TState : Enum
    {
        protected FSMBase()
        {
            LifeCycleManager._Instance.Register(this);
        }
        
        public abstract void ChangeState(TState newState);
        public abstract void OnUpdate();
        public void OnDestroy()
        {
            LifeCycleManager._Instance.UnRegister(this);
        }
    }
}