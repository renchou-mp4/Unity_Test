using UnityEngine;

public class NormalMove : IMove
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //��ͨ�ƶ�
        int distance = (int)args[0];
        Debug.Log($"��ͨ�ƶ�:{distance}");
    }
}