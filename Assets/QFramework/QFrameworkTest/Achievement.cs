using QFramework;
using UnityEngine;

public class Achievement : AbstractSystem
{
    private CounterModel _counterModel;

    protected override void OnInit()
    {
        _counterModel = this.GetModel<CounterModel>();

        this.RegisterEvent<CountChangedEvent>((e) =>
        {
            if (_counterModel.mCount.Value == 5)
            {
                Debug.Log("成就：点击5次");
            }
            else if (_counterModel.mCount.Value == 10)
            {
                Debug.Log("成就：点击10次");
            }
            else if (_counterModel.mCount.Value == -3)
            {
                Debug.Log("成就");
            }
        });
    }
}
