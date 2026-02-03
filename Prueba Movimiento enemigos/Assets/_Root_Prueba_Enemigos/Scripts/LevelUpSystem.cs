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

    // Modificadores acumulables (NO stats base)
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
            title = "Más daño",
            description = "+20% daño"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarVelocidadMovimiento,
            value = 1f,
            title = "Velocidad",
            description = "+1 movimiento"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarSaludMaxima,
            value = 5f,
            title = "Más vida",
            description = "+5 salud máxima"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.RalentizarEnemigos,
            value = 0.9f,
            title = "Ralentizar enemigos",
            description = "-10% velocidad enemigos"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarVelocidadDisparo,
            value = 0.9f,
            title = "Disparo rápido",
            description = "Mayor velocidad de disparo"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.ReducirDanioEnemigos,
            value = 0.9f,
            title = "Menos daño enemigo",
            description = "-10% daño enemigo"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.CurarSalud,
            value = 5f,
            title = "Curar",
            description = "Recupera 5 de vida"
        });

        allUpgrades.Add(new Upgrade
        {
            type = UpgradeType.AumentarDistanciaDash,
            value = 1.2f,
            title = "Dash largo",
            description = "Aumenta la distancia del dash"
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
    }

    public void PausePlayer() { }
    public void UnpausePlayer() { }
}
