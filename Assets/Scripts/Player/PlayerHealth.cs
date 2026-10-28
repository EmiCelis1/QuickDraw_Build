using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;

    public int maxArmor = 100;
    private int armor;

    [Header("Opcional: Hápticos en VR al recibir daño")]
    public bool enableHaptics = true;
    [Range(0f, 1f)] public float hapticAmplitude = 0.5f;
    public float hapticDuration = 0.2f;

    void Start()
    {
        health = maxHealth;
        armor = maxArmor; 

        UpdateUI();
    }

    void Update()
    {
        // Test rápido con teclado (puedes mantenerlo para pruebas en el editor)
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            DamagePlayer(30);
            Debug.Log("Player has received damage (Test)");
        }
    }

    public void DamagePlayer(int damage)
    {
        if (armor > 0)
        {
            if (armor >= damage)
            {
                armor -= damage;
            }
            else
            {
                int remainingDamage = damage - armor;
                armor = 0;
                health -= remainingDamage;
            }
        }
        else
        {
            health -= damage;
        }

        // Activar vibración en los mandos de VR para mayor inmersión
        if (enableHaptics)
        {
            TriggerVREffect();
        }

        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player is dead");

            // Reiniciar escena o activar pantalla de Game Over en VR
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        UpdateUI();
    }

    public void GiveHealth(int amount, GameObject pickup)
    {
        if (health < maxHealth)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
            Destroy(pickup);
        }
        UpdateUI();
    }   

    private void UpdateUI()
    {
        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.UpdateHealthIndicator(health);
            CanvasManager.Instance.UpdateArmorIndicator(armor);
        }
    }

    private void TriggerVREffect()
    {
        // Envía vibración a los controladores XR conectados
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(devices);

        foreach (var device in devices)
        {
            if (device.characteristics.HasFlag(UnityEngine.XR.InputDeviceCharacteristics.Controller))
            {
                device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
            }
        }
    }
}