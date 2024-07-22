using QFramework;
using UnityEngine;

public interface IAchievementSystem : ISystem
{

}

public class Achievement : AbstractSystem, IAchievementSystem
{
    private ICounterModel _counterModel;

    protected override void OnInit()
    {
        _counterModel = this.GetModel<ICounterModel>();

        _counterModel.mCount.Register((e) =>
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
