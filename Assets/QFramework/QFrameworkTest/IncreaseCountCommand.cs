using QFramework;

public class IncreaseCountCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<ICounterModel>().mCount.Value++;
    }
}
