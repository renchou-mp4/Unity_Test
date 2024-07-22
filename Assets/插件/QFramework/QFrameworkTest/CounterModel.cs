using QFramework;

public interface ICounterModel : IModel
{
    BindableProperty<int> mCount { get; }
}

public class CounterModel : AbstractModel, ICounterModel
{
    public BindableProperty<int> mCount { get; } = new();

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
