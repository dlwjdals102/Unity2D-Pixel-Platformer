using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_SFX sfx;
    private Entity_VFX vfx;
    private EntityStats stats;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<EntityStats>();
        sfx = GetComponent<Entity_SFX>();
    }

    public void PerformAttack()
    {
        bool targetGotHit = false;

        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;
            
            targetGotHit = damageable.TakeDamage(stats.GetDamage(), transform);

            if (targetGotHit)
            {
                vfx?.CreateOnHitVFX(target.transform);
                sfx?.PlayAttackHit();
            }
        }

        if (!targetGotHit)
            sfx?.PlayAttackMiss();
    }

    public void PerformAttackOnTarget(Transform target)
    {
        bool targetGotHit = false;

        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
            return; // skip target, go to next target

        targetGotHit = damageable.TakeDamage(stats.GetDamage(), transform);

        if (targetGotHit)
        {
            vfx?.CreateOnHitVFX(target.transform);
            sfx?.PlayAttackHit();
        }

        if (!targetGotHit)
            sfx?.PlayAttackMiss();
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
