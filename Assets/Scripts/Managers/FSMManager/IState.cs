using System;

namespace Managers
{
    // ReSharper disable once InconsistentNaming
    public interface IState<TState> where TState : Enum
    {
        public TState _State { get; set; }
        
        public void OnEnterState();
        public void OnUpdateState();
        public void OnExitState();
    }
}
