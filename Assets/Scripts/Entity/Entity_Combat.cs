using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
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
    }

    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            damageable.TakeDamage(stats.GetDamage(), transform);
            vfx.CreateOnHitVFX(target.transform);
        }
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
