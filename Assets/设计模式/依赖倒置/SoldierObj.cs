using System.Collections.Generic;
using UnityEngine;

public class SoldierObj : MonoBehaviour
{
    private SoldierActor _soldier = null;

    private void Start()
    {
        _soldier = new SoldierActor(new Dictionary<string, IAction>()
        {
            { "��ͨ����", new NormalAttack() }
        });
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 50, 100, 50), "�������⹥��")) _soldier.AddAction("���⹥��", new SpecialAttack());
        if (GUI.Button(new Rect(0, 100, 100, 50), "������ͨ�ƶ�")) _soldier.AddAction("��ͨ�ƶ�", new NormalMove());
        if (GUI.Button(new Rect(0, 150, 100, 50), "���������ƶ�")) _soldier.AddAction("�����ƶ�", new SpecialMove());
        if (GUI.Button(new Rect(0, 200, 100, 50), "������ͨ��Ծ")) _soldier.AddAction("��ͨ��Ծ", new NormalJump());
        if (GUI.Button(new Rect(0, 250, 100, 50), "����������Ծ")) _soldier.AddAction("������Ծ", new SpecialJump());


        if (GUI.Button(new Rect(100, 0, 100, 50), "ʹ����ͨ����")) _soldier.DoAction("��ͨ����", 100);
        if (GUI.Button(new Rect(100, 50, 100, 50), "ʹ�����⹥��")) _soldier.DoAction("���⹥��", 200);
        if (GUI.Button(new Rect(100, 100, 100, 50), "ʹ����ͨ�ƶ�")) _soldier.DoAction("��ͨ�ƶ�", 10);
        if (GUI.Button(new Rect(100, 150, 100, 50), "ʹ�������ƶ�")) _soldier.DoAction("�����ƶ�", 1000);
        if (GUI.Button(new Rect(100, 200, 100, 50), "ʹ����ͨ��Ծ")) _soldier.DoAction("��ͨ��Ծ", 1);
        if (GUI.Button(new Rect(100, 250, 100, 50), "ʹ��������Ծ")) _soldier.DoAction("������Ծ", 2);
    }
}