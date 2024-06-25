using QFramework;

public class CounterModel : AbstractModel
{
    public BindableProperty<int> mCount = new BindableProperty<int>();

    private Storage _storage;

    protected override void OnInit()
    {
        _storage = this.GetUtility<Storage>();

        mCount.Value = _storage.LoadInt(nameof(mCount), 0);

        mCount.Register((count) =>
        {
            _storage.SaveInt(nameof(mCount), count);
        });
    }
}
