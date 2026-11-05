using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System

public interface IDamageable
{
    void TakeDamage(int damage);
}

public interface IHealthProvider
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    int CurrentArmor { get; }
    int MaxArmor { get; }

    event Action<int, int> OnHealthChanged;
    event Action<int, int> OnArmorChanged;
    event Action OnPlayerDeath;
}

public class PlayerHealth : MonoBehaviour, IDamageable, IHealthProvider
{
    [Header("Configuración de Salud")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Configuración de Escudo (Combo)")]
    [SerializeField] private int maxArmor = 100;
    private int currentArmor;

    [Header("Hápticos en VR al recibir daño")]
    [SerializeField] private bool enableHaptics = true;
    [Range(0f, 1f)][SerializeField] private float hapticAmplitude = 0.5f;
    [SerializeField] private float hapticDuration = 0.2f;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnArmorChanged;
    public event Action OnPlayerDeath;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentArmor => currentArmor;
    public int MaxArmor => maxArmor;

    private void Start()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        NotifyUI();
    }

    private void Update()
    {
        // Tu tecla de debug intacta, adaptada al nuevo Input System para evitar errores
        if (Keyboard.current != null && Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            TakeDamage(30);
            Debug.Log("Player has received damage (Test Debug)");
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentArmor > 0)
        {
            if (currentArmor >= damage)
            {
                currentArmor -= damage;
            }
            else
            {
                int remainingDamage = damage - currentArmor;
                currentArmor = 0;
                currentHealth = Mathf.Max(0, currentHealth - remainingDamage);
            }
        }
        else
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
        }

        if (enableHaptics)
        {
            TriggerVREffect();
        }

        NotifyUI();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void GiveHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            NotifyUI();
        }
    }

    public void AddArmor(int amount)
    {
        currentArmor = Mathf.Min(maxArmor, currentArmor + amount);
        NotifyUI();
    }

    private void NotifyUI()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnArmorChanged?.Invoke(currentArmor, maxArmor);
    }

    private void HandleDeath()
    {
        Debug.Log("Player is dead");
        OnPlayerDeath?.Invoke();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void TriggerVREffect()
    {
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