using UnityEngine;

public class NormalAttack : IAttack
{
    public void Execute(BaseActor actor, params object[] args)
    {
        //��ͨ����
        int damage = (int)args[0];
        Debug.Log($"��ͨ�������˺���{damage}");
    }
}