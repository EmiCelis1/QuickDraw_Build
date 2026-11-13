using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("TextMeshPro References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;

    private IHealthProvider healthProvider;

    private void Start()
    {
        // Buscamos automáticamente el componente que maneja la salud en la escena (tu PlayerHealth)
        healthProvider = FindFirstObjectByType<PlayerHealth>();

        if (healthProvider != null)
        {
            // Nos suscribimos a los eventos del jugador
            healthProvider.OnHealthChanged += UpdateHealthUI;
            healthProvider.OnArmorChanged += UpdateArmorUI;

            // Actualizamos los textos de forma inicial con los valores actuales
            UpdateHealthUI(healthProvider.CurrentHealth, healthProvider.MaxHealth);
            UpdateArmorUI(healthProvider.CurrentArmor, healthProvider.MaxArmor);
        }
        else
        {
            Debug.LogError("PlayerHealthUI no pudo encontrar ningún IHealthProvider en la escena.");
        }
    }

    private void OnDestroy()
    {
        // Muy importante: nos desuscribimos al destruir el objeto para evitar fugas de memoria o errores
        if (healthProvider != null)
        {
            healthProvider.OnHealthChanged -= UpdateHealthUI;
            healthProvider.OnArmorChanged -= UpdateArmorUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"SALUD: {currentHealth} / {maxHealth}";
        }
    }

    private void UpdateArmorUI(int currentArmor, int maxArmor)
    {
        if (armorText != null)
        {
            armorText.text = $"ESCUDO: {currentArmor} / {maxArmor}";

            // Opcional: Ocultar o mostrar el texto del escudo si está en 0
            // armorText.gameObject.SetActive(currentArmor > 0);
        }
    }
}