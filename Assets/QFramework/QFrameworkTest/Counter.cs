using QFramework;

public class Counter : Architecture<Counter>
{
    protected override void Init()
    {
        this.RegisterSystem(new Achievement());
        this.RegisterUtility(new Storage());
        this.RegisterModel(new CounterModel());
    }
}
