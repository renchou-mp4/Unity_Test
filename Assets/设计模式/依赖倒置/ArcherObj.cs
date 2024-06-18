using System.Collections.Generic;
using UnityEngine;

public class ArcherObj : MonoBehaviour
{
    private ArcherActor _archerActor;

    private void Start()
    {
        _archerActor = new ArcherActor(new Dictionary<string, IAction>()
        {
            {"普通攻击",new NormalAttack()},
        });
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 200, 50, 100, 50), "添加特殊攻击"))
        {
            _archerActor.AddAction("特殊攻击", new SpecialAttack());
        }
        if (GUI.Button(new Rect(Screen.width - 200, 100, 100, 50), "添加普通移动"))
        {
            _archerActor.AddAction("普通移动", new NormalMove());
        }
        if (GUI.Button(new Rect(Screen.width - 200, 150, 100, 50), "添加特殊移动"))
        {
            _archerActor.AddAction("特殊移动", new SpecialMove());
        }
        if (GUI.Button(new Rect(Screen.width - 200, 200, 100, 50), "添加普通跳跃"))
        {
            _archerActor.AddAction("普通跳跃", new NormalJump());
        }
        if (GUI.Button(new Rect(Screen.width - 200, 250, 100, 50), "添加特殊跳跃"))
        {
            _archerActor.AddAction("特殊跳跃", new SpecialJump());
        }



        if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 50), "使用普通攻击"))
        {
            _archerActor.DoAction("普通攻击", 100);
        }
        if (GUI.Button(new Rect(Screen.width - 100, 50, 100, 50), "使用特殊攻击"))
        {
            _archerActor.DoAction("特殊攻击", 200);
        }
        if (GUI.Button(new Rect(Screen.width - 100, 100, 100, 50), "使用普通移动"))
        {
            _archerActor.DoAction("普通移动", 10);
        }
        if (GUI.Button(new Rect(Screen.width - 100, 150, 100, 50), "使用特殊移动"))
        {
            _archerActor.DoAction("特殊移动", 1000);
        }
        if (GUI.Button(new Rect(Screen.width - 100, 200, 100, 50), "使用普通跳跃"))
        {
            _archerActor.DoAction("普通跳跃", 1);
        }
        if (GUI.Button(new Rect(Screen.width - 100, 250, 100, 50), "使用特殊跳跃"))
        {
            _archerActor.DoAction("特殊跳跃", 2);
        }
    }
}
