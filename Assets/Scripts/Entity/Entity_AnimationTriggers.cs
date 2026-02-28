using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    public virtual void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }

    public virtual void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

    
}
