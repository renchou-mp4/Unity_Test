using QFramework;

public class DecreaseCountCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<ICounterModel>().mCount.Value--;
    }
}
