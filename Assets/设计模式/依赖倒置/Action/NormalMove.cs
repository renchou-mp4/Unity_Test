using UnityEngine;

public class NormalMove : IMove
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //普通移动
        int distance = (int)args[0];
        Debug.Log($"普通移动:{distance}");
    }
}
