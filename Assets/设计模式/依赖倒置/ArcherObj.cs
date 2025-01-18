using System.Collections.Generic;
using UnityEngine;

public class ArcherObj : MonoBehaviour
{
    private ArcherActor _archerActor;

    private void Start()
    {
        _archerActor = new ArcherActor(new Dictionary<string, IAction>()
        {
            { "��ͨ����", new NormalAttack() }
        });
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 200, 50, 100, 50), "�������⹥��")) _archerActor.AddAction("���⹥��", new SpecialAttack());
        if (GUI.Button(new Rect(Screen.width - 200, 100, 100, 50), "������ͨ�ƶ�")) _archerActor.AddAction("��ͨ�ƶ�", new NormalMove());
        if (GUI.Button(new Rect(Screen.width - 200, 150, 100, 50), "���������ƶ�")) _archerActor.AddAction("�����ƶ�", new SpecialMove());
        if (GUI.Button(new Rect(Screen.width - 200, 200, 100, 50), "������ͨ��Ծ")) _archerActor.AddAction("��ͨ��Ծ", new NormalJump());
        if (GUI.Button(new Rect(Screen.width - 200, 250, 100, 50), "����������Ծ")) _archerActor.AddAction("������Ծ", new SpecialJump());


        if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 50), "ʹ����ͨ����")) _archerActor.DoAction("��ͨ����", 100);
        if (GUI.Button(new Rect(Screen.width - 100, 50, 100, 50), "ʹ�����⹥��")) _archerActor.DoAction("���⹥��", 200);
        if (GUI.Button(new Rect(Screen.width - 100, 100, 100, 50), "ʹ����ͨ�ƶ�")) _archerActor.DoAction("��ͨ�ƶ�", 10);
        if (GUI.Button(new Rect(Screen.width - 100, 150, 100, 50), "ʹ�������ƶ�")) _archerActor.DoAction("�����ƶ�", 1000);
        if (GUI.Button(new Rect(Screen.width - 100, 200, 100, 50), "ʹ����ͨ��Ծ")) _archerActor.DoAction("��ͨ��Ծ", 1);
        if (GUI.Button(new Rect(Screen.width - 100, 250, 100, 50), "ʹ��������Ծ")) _archerActor.DoAction("������Ծ", 2);
    }
}