using UnityEngine;

public class SpecialMove : IMove
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //�����ƶ�
        int distance = (int)args[0];
        Debug.Log($"�����ƶ�:{distance}");
    }
}