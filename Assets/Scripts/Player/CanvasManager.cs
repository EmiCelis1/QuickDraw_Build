using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;

    public Image healthIndicator;
    public Image GearIndicator;

    public Sprite health1; // Max health 
    public Sprite health2; // Medium 
    public Sprite health3; // Low 
    public Sprite health4; // Dead 

    private static CanvasManager _instance;
    public static CanvasManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void UpdateHealthIndicator(int healthValue)
    {
        // Actualizar texto 
        if (healthText != null)
        {
            healthText.text = healthValue.ToString();
        }

        // Actualizar indicadores de salud 
        if (healthValue >= 66)
        {
            healthIndicator.sprite = health1;
        }
        else if (healthValue >= 33 && healthValue < 66)
        {
            healthIndicator.sprite = health2;
        }
        else if (healthValue > 0 && healthValue < 33)
        {
            healthIndicator.sprite = health3;
        }
        else
        {
            healthIndicator.sprite = health4;
        }
    }

    public void UpdateArmorIndicator(int armorValue)
    {
        if (armorText != null)
        {
            armorText.text = armorValue.ToString();
        }
    }

    public void UpdateGearIndicator(Sprite weaponSprite)
    {
        if (GearIndicator != null && weaponSprite != null)
        {
            GearIndicator.sprite = weaponSprite;
        }
    }
}