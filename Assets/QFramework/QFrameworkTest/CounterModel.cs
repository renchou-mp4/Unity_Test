using QFramework;

public class CounterModel : AbstractModel
{
    public int Count;

    protected override void OnInit()
    {
        Count = 0;
    }
}
