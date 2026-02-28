using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
    }

    public virtual void SpecialAttackTrigger()
    {
        enemy.SpecialAttack();
    }
}
