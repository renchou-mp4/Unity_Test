using QFramework;

public class DecreaseCountCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<CounterModel>().mCount.Value--;
        this.SendEvent<CountChangedEvent>();
    }
}
