using System;
using System.Collections.Generic;
using System.Reflection;

namespace Managers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProcessStateAttribute : Attribute
    {
        public ProcessFSMState _State { get; }

        public ProcessStateAttribute(ProcessFSMState state)
        {
            _State = state;
        }
    }
    
    // ReSharper disable once InconsistentNaming
    public class ProcessFSM : FSMBase<ProcessFSMState>
    {
        /// <summary>
        /// ProcessFSM状态对象缓存
        /// </summary>
        private Dictionary<ProcessFSMState, IState<ProcessFSMState>> _states = new();
        /// <summary>
        /// 当前ProcessFSM状态
        /// </summary>
        private ProcessFSMState _currentState;
        
        public ProcessFSM(Assembly curAssembly,ProcessFSMState initState)
        {
            //获取程序集中所有类型
            foreach (var type in curAssembly.GetTypes())
            {
                //尝试从该类型中获取指定的自定义特性
                var attribute = type.GetCustomAttribute<ProcessStateAttribute>();
                //若结果不为空,且该类型可以被赋值给当前类型(IState<ProcessFSMState>)
                if (attribute == null || !typeof(IState<ProcessFSMState>).IsAssignableFrom(type))
                    continue;
                //创建该类型实例并赋值,添加到状态机中
                IState<ProcessFSMState> state = (IState<ProcessFSMState>)Activator.CreateInstance(type);
                _states.Add(attribute._State, state);
            }

            _currentState = ProcessFSMState.Launch;
            _states[_currentState].OnEnterState();
        }

        public override void ChangeState(ProcessFSMState newState)
        {
            _states[_currentState].OnExitState();
            _currentState = newState;
            _states[_currentState].OnEnterState();
        }

        public override void OnUpdate()
        {
            _states[_currentState].OnUpdateState();
        }
    }
}