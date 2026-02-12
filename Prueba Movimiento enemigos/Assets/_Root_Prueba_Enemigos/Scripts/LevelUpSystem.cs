using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public WaveManager waveManager;

    [Header("Player References")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public Weapon weapon;
    public HealthBar healthBar;

    [Header("Upgrade UI")]
    public UpgradeButton[] upgradeButtons;

    
    float damageMultiplier = 1f;
    float moveSpeedBonus = 0f;
    float enemySlowMultiplier = 1f;
    float enemyDamageMultiplier = 1f;

    List<Upgrade> allUpgrades = new List<Upgrade>();

    void Awake()
    {
        CreateUpgrades();
    }

    void CreateUpgrades()
    {
        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarDanio,
            value = 0.2f,
            title = "More Damage",
            description = "Damage +20%"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarVelocidadMovimiento,
            value = 1f,
            title = "Speed Upgrade",
            description = "Speed Movement +1"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarSaludMaxima,
            value = 5f,
            title = "Increase Max Health",
            description = "Max Health +5"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.RalentizarEnemigos,
            value = 0.9f,
            title = "Slower Enemies",
            description = "Enemies Speed -10%"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarVelocidadDisparo,
            value = 0.9f,
            title = "Faster Gun",
            description = "Faster Fire Rate"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.ReducirDanioEnemigos,
            value = 0.9f,
            title = "Reduce Enemy Damage",
            description = "Enemy Damage -10%"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.CurarSalud,
            value = 5f,
            title = "Heal Up",
            description = "Recover 5 Health Points"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarDistanciaDash,
            value = 1.2f,
            title = "Better Dash",
            description = "Increase Dash Distance"
        });
    }

    public void ShowUpgrades()
    {
        List<Upgrade> temp = new List<Upgrade>(allUpgrades);

        foreach (UpgradeButton button in upgradeButtons)
        {
            int index = Random.Range(0, temp.Count);
            button.Setup(temp[index], this);
            temp.RemoveAt(index);
        }
        Time.timeScale = 0;
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.AumentarDanio:
                damageMultiplier += upgrade.value;
                break;

            case UpgradeType.AumentarVelocidadMovimiento:
                playerMovement.moveSpeed += upgrade.value;
                break;

            case UpgradeType.AumentarSaludMaxima:
                playerHealth.maxHealth += upgrade.value;
                playerHealth.currentHealth += upgrade.value;
                healthBar.UpdateHealthBar(playerHealth.currentHealth, playerHealth.maxHealth);
                break;

            case UpgradeType.CurarSalud:
                playerHealth.currentHealth = Mathf.Min(
                    playerHealth.currentHealth + upgrade.value,
                    playerHealth.maxHealth
                );
                healthBar.UpdateHealthBar(playerHealth.currentHealth, playerHealth.maxHealth);
                break;

            case UpgradeType.AumentarVelocidadDisparo:
                weapon.fireRate *= upgrade.value;
                break;

            case UpgradeType.AumentarDistanciaDash:
                playerMovement.dashSpeed *= upgrade.value;
                break;
        }

        
        waveManager.upgradeClicked = true;
        Time.timeScale = 1;
    }

    public void PausePlayer()
    {
        playerMovement.enabled = false;
        weapon.enabled = false;      
    }
    public void UnpausePlayer()
    {
        playerMovement.enabled = true;
        weapon.enabled = true;
    }
}
