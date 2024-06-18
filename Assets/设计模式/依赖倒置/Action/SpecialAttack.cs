using UnityEngine;

public class SpecialAttack : IAttack
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //ÌØÊâ¹¥»÷
        int damage = (int)args[0];
        Debug.Log($"ÌØÊâ¹¥»÷£¬ÉËº¦£º{damage}");
    }
}
