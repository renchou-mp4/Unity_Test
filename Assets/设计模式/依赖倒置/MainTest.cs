using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour
{
    [SerializeField]
    private SoldierActor _soldier;
    [SerializeField]
    private ArcherActor _archer;

    private void Start()
    {
        _soldier = new SoldierActor(new Dictionary<string, IAction>()
        {
            { "��ͨ����" ,new NormalAttack()},
        });
        _archer = new ArcherActor(new Dictionary<string, IAction>()
        {
            { "��ͨ����" ,new NormalAttack()},
        });
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "������⹥��"))
        {
            _soldier.AddAction("���⹥��", new SpecialAttack());
        }
    }
}
