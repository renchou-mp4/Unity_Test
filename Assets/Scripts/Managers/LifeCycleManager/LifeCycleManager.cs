using System.Collections.Generic;

namespace Managers.LifeCycleManager
{
    public class LifeCycleManager : MonoSingleton<LifeCycleManager>
    {
        private readonly List<IAwake>       _awakeList       = new List<IAwake>();
        private readonly List<IEnable>      _enableList      = new List<IEnable>();
        private readonly List<IStart>       _startList       = new List<IStart>();
        private readonly List<IUpdate>      _updateList      = new List<IUpdate>();
        private readonly List<IFixedUpdate> _fixedUpdateList = new List<IFixedUpdate>();
        private readonly List<IDisable>     _disableList     = new List<IDisable>();
        private readonly List<IDestroy>     _destroyList     = new List<IDestroy>();


        public void Register(ILifeCycle lifeCycle)
        {
            if (lifeCycle is IAwake awake) _awakeList.Add(awake);
            if(lifeCycle is IEnable enable) _enableList.Add(enable);
            if(lifeCycle is IStart start) _startList.Add(start);
            if(lifeCycle is IUpdate update) _updateList.Add(update);
            if(lifeCycle is IFixedUpdate fixedUpdate) _fixedUpdateList.Add(fixedUpdate);
            if(lifeCycle is IDisable disable) _disableList.Add(disable);
            if(lifeCycle is IDestroy destroy) _destroyList.Add(destroy);
        }

        public void UnRegister(ILifeCycle lifeCycle)
        {
            if(lifeCycle is IAwake awake) _awakeList.Remove(awake);
            if(lifeCycle is IEnable enable) _enableList.Remove(enable);
            if(lifeCycle is IStart start) _startList.Remove(start);
            if(lifeCycle is IUpdate update) _updateList.Remove(update);
            if(lifeCycle is IFixedUpdate fixedUpdate) _fixedUpdateList.Remove(fixedUpdate);
            if(lifeCycle is IDisable disable) _disableList.Remove(disable);
            if(lifeCycle is IDestroy destroy) _destroyList.Remove(destroy);
        }
        
        public void Awake() => _awakeList.ForEach(obj => obj.OnAwake());
        public void OnEnable() => _enableList.ForEach(obj => obj.OnEnable());
        public void Start() => _startList.ForEach(obj => obj.OnStart());
        public void Update() => _updateList.ForEach(obj => obj.OnUpdate());
        public void FixedUpdate() => _fixedUpdateList.ForEach(obj => obj.OnFixedUpdate());
        public void OnDisable() => _disableList.ForEach(obj => obj.OnDisable());
        public void OnDestroy() => _destroyList.ForEach(obj => obj.OnDestroy());
    }
}
