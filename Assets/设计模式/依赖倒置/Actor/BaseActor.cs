using System.Collections.Generic;
using UnityEngine;

public class BaseActor
{
    private readonly Dictionary<string, IAction> _actions = new();

    public BaseActor(Dictionary<string, IAction> actions)
    {
        foreach (var action in actions) AddAction(action.Key, action.Value);
    }

    public void AddAction(string actionName, IAction action)
    {
        if (_actions.ContainsKey(actionName))
        {
            //���error
        }
        else
        {
            _actions.Add(actionName, action);
            Debug.Log($"��{actionName}��������");
        }
    }

    public void DoAction(string actionName, params object[] args)
    {
        if (!_actions.ContainsKey(actionName))
        {
            //���error��û�и�action
        }
        else
        {
            _actions[actionName].Execute(this, args);
        }
    }
}