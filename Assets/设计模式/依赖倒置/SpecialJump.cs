using UnityEngine;

public class SpecialJump : IJump
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //ÌØÊâÌøÔ¾
        int distance = (int)args[0];
        Debug.Log($"ÌØÊâÌøÔ¾£º{distance}");
    }
}
