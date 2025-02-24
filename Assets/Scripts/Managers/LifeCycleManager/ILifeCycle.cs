namespace Managers
{
    public interface ILifeCycle
    {
    }

    public interface IAwake : ILifeCycle
    {
        public abstract void OnAwake();
    }

    public interface IEnable : ILifeCycle
    {
        public abstract void OnEnable();
    }

    public interface IStart : ILifeCycle
    {
        public abstract void OnStart();
    }

    public interface IUpdate : ILifeCycle
    {
        public abstract void OnUpdate();
    }

    public interface IFixedUpdate : ILifeCycle
    {
        public abstract void OnFixedUpdate();
    }

    public interface IDisable : ILifeCycle
    {
        public abstract void OnDisable();
    }

    public interface IDestroy : ILifeCycle
    {
        public abstract void OnDestroy();
    }
}