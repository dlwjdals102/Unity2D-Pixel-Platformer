using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public StatSetupSO defaultStatSetup;

    public Stat maxHealth;
    public Stat damage;
    public Stat armor;
    public Stat attackSpeed;

    public float GetMaxHealth()
    {
        return maxHealth.GetValue();
    }

    public float GetDamage()
    {
        return damage.GetValue();
    }

    public float GetArmor()
    {
        return armor.GetValue();
    }

    public float GetAttackSpeed()
    {
        return attackSpeed.GetValue();
    }


    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        maxHealth.SetValue(defaultStatSetup.maxHealth);
        damage.SetValue(defaultStatSetup.damage);
        armor.SetValue(defaultStatSetup.armor);
        attackSpeed.SetValue(defaultStatSetup.attackSpeed);
    }
}
