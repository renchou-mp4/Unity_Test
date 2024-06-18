using UnityEngine;

public class NormalJump : IJump
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //ÆÕÍ¨ÌøÔ¾
        int distance = (int)args[0];
        Debug.Log($"ÆÕÍ¨ÌøÔ¾£º{distance}");
    }
}
