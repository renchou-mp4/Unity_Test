using System.Collections.Generic;
using UnityEngine;

public class SoldierObj : MonoBehaviour
{
    private SoldierActor _soldier = null;

    private void Start()
    {
        _soldier = new SoldierActor(new Dictionary<string, IAction>()
        {
            { "普通攻击" ,new NormalAttack()},
        });
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "添加特殊攻击"))
        {
            _soldier.AddAction("特殊攻击", new SpecialAttack());
        }
        if (GUI.Button(new Rect(0, 50, 100, 50), "添加普通移动"))
        {
            _soldier.AddAction("普通移动", new NormalMove());
        }
        if (GUI.Button(new Rect(0, 100, 100, 50), "添加特殊移动"))
        {
            _soldier.AddAction("特殊移动", new SpecialMove());
        }
        if (GUI.Button(new Rect(0, 150, 100, 50), "添加普通跳跃"))
        {
            _soldier.AddAction("普通跳跃", new NormalJump());
        }
        if (GUI.Button(new Rect(0, 200, 100, 50), "添加特殊跳跃"))
        {
            _soldier.AddAction("特殊跳跃", new SpecialJump());
        }
    }
}
