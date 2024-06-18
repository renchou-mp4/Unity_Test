using UnityEngine;

public class NormalAttack : IAttack
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //ÆÕÍ¨¹¥»÷
        int damage = (int)args[0];
        Debug.Log($"ÆÕÍ¨¹¥»÷£¬ÉËº¦£º{damage}");
    }
}
