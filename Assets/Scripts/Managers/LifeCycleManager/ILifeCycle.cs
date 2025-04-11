namespace Managers
{
    public interface ILifeCycle
    {
    }

    public interface IAwake : ILifeCycle
    {
        public void OnAwake();
    }

    public interface IEnable : ILifeCycle
    {
        public void OnEnable();
    }

    public interface IStart : ILifeCycle
    {
        public void OnStart();
    }

    public interface IUpdate : ILifeCycle
    {
        public void OnUpdate();
    }

    public interface IFixedUpdate : ILifeCycle
    {
        public void OnFixedUpdate();
    }

    public interface IDisable : ILifeCycle
    {
        public void OnDisable();
    }

    public interface IDestroy : ILifeCycle
    {
        public void OnDestroy();
    }
}