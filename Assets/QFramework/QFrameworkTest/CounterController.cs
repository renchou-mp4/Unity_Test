using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour, IController
{
    public TextMeshProUGUI Text;
    public Button BtnIncrease;
    public Button BtnDecrease;

    private CounterModel model;

    public IArchitecture GetArchitecture()
    {
        return Counter.Interface;
    }

    private void Start()
    {
        model = this.GetModel<CounterModel>();
        BtnIncrease.onClick.AddListener(() =>
        {
            this.SendCommand<IncreaseCountCommand>();

        });

        BtnDecrease.onClick.AddListener(() =>
        {
            this.SendCommand<DecreaseCountCommand>();

        });

        this.RegisterEvent<CountChangedEvent>((e) =>
        {
            UpdateView();

        }).UnRegisterWhenGameObjectDestroyed(gameObject);

    }

    private void UpdateView()
    {
        Text.text = model.mCount.Value.ToString();
    }
}
