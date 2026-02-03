using UnityEngine;

public class Upgrade
{
    public UpgradeType type;
    public float value;
    public string title;
    public string description;
}

public enum UpgradeType
{
    AumentarDanio,
    AumentarVelocidadMovimiento,
    AumentarSaludMaxima,
    RalentizarEnemigos,
    AumentarVelocidadDisparo,
    ReducirDanioEnemigos,
    CurarSalud,
    AumentarDistanciaDash
}
