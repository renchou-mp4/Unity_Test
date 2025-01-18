using UnityEngine;

public class SpecialJump : IJump
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //������Ծ
        int distance = (int)args[0];
        Debug.Log($"������Ծ��{distance}");
    }
}