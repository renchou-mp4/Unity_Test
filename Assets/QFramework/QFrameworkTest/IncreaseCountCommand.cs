using QFramework;

public class IncreaseCountCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<CounterModel>().mCount.Value++;
        this.SendEvent<CountChangedEvent>();
    }
}
