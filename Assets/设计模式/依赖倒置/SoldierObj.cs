using System.Collections.Generic;
using UnityEngine;

public class SoldierObj : MonoBehaviour
{
    private SoldierActor _soldier = null;

    private void Start()
    {
        _soldier = new SoldierActor(new Dictionary<string, IAction>()
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
        if (GUI.Button(new Rect(0, 50, 100, 50), "�����ͨ�ƶ�"))
        {
            _soldier.AddAction("��ͨ�ƶ�", new NormalMove());
        }
        if (GUI.Button(new Rect(0, 100, 100, 50), "��������ƶ�"))
        {
            _soldier.AddAction("�����ƶ�", new SpecialMove());
        }
        if (GUI.Button(new Rect(0, 150, 100, 50), "�����ͨ��Ծ"))
        {
            _soldier.AddAction("��ͨ��Ծ", new NormalJump());
        }
        if (GUI.Button(new Rect(0, 200, 100, 50), "���������Ծ"))
        {
            _soldier.AddAction("������Ծ", new SpecialJump());
        }
    }
}
