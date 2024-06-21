using QFramework;

public class Counter : Architecture<Counter>
{
    protected override void Init()
    {
        this.RegisterModel(new CounterModel());
    }
}
