using System.Collections.Generic;
using UnityEngine;

public class BaseActor
{
    private Dictionary<string, IAction> _actions = new();

    public BaseActor(Dictionary<string, IAction> actions)
    {
        foreach (var action in actions)
        {
            AddAction(action.Key, action.Value);
        }
    }

    public void AddAction(string actionName, IAction action)
    {
        if (_actions.ContainsKey(actionName))
        {
            //输出error
        }
        else
        {
            _actions.Add(actionName, action);
            Debug.Log($"【{actionName}】已添加");
        }
    }

    public void DoAction(string actionName, params object[] args)
    {
        if (!_actions.ContainsKey(actionName))
        {
            //输出error，没有该action
        }
        else
        {
            _actions[actionName].Execute(this, args);
        }
    }

}
