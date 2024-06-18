using UnityEngine;

public class SpecialMove : IMove
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //特殊移动
        int distance = (int)args[0];
        Debug.Log($"特殊移动:{distance}");
    }
}
