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
                Debug.Log("�ɾͣ����5��");
            }
            else if (_counterModel.mCount.Value == 10)
            {
                Debug.Log("�ɾͣ����10��");
            }
            else if (_counterModel.mCount.Value == -3)
            {
                Debug.Log("�ɾ�");
            }
        });
    }
}
