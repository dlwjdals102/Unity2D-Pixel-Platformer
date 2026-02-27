using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player_Health : Entity_Health, ISaveable
{

    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();

        player.ui.OpenDeathScreenUI();
    }

    public void LoadData(GameData data)
    {
        if (Mathf.Approximately(data.playerCurrentHealth, 0))
            currentHealth = stats.GetMaxHealth();
        else
            currentHealth = data.playerCurrentHealth;

        UpdateHealthBar();
    }

    public void SaveData(ref GameData data)
    {
        data.playerCurrentHealth = currentHealth;
    }

}
