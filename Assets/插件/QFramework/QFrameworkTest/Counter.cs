using QFramework;

public class Counter : Architecture<Counter>
{
    protected override void Init()
    {
        RegisterSystem<IAchievementSystem>(new Achievement());
        RegisterModel<IModel>(new CounterModel());
        RegisterUtility<IStorage>(new Storage());
    }
}
