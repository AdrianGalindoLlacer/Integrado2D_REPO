using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;

    Upgrade upgrade;
    LevelUpSystem system;
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(Upgrade u, LevelUpSystem lvlSystem)
    {
        upgrade = u;
        system = lvlSystem;

        title.text = u.title;
        description.text = u.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        system.ApplyUpgrade(upgrade);
    }
}
