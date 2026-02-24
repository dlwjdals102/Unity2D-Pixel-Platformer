using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Defalut Stat Setup", fileName = "Default Stat Setup")]
public class StatSetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;

    [Header("Offense")]
    public float damage = 10;
    public float attackSpeed = 1;

    [Header("Defense")]
    public float armor;
}
