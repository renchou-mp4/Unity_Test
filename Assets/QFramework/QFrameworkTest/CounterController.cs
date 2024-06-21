using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour, IController
{
    public TextMeshProUGUI Text;
    public Button BtnIncrease;
    public Button BtnDecrease;
    private int count = 0;

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
            model.Count++;

            UpdateView();
        });

        BtnDecrease.onClick.AddListener(() =>
        {
            model.Count--;

            UpdateView();
        });

    }

    private void UpdateView()
    {
        Text.text = model.Count.ToString();
    }
}
