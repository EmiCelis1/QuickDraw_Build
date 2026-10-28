using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GestureReload : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleGunShoot gunShoot;

    [Header("Movement Configuration")]
    [Tooltip("Minimum speed for Shake")]
    [SerializeField] private float shakeThreshold = 3.5f;
    [Tooltip("Reload Cooldown")]
    [SerializeField] private float reloadCooldown = 1.0f;

    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    private float nextReloadTime = 0f;
    private XRBaseInteractable interactable;

    void Start()
    {
        if (gunShoot == null)
            gunShoot = GetComponent<SimpleGunShoot>();

        interactable = GetComponent<XRBaseInteractable>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Solo permitir recargar si el arma está siendo sostenida en la mano VR
        if (interactable != null && !interactable.isSelected)
            return;

        // Calcular la velocidad actual del arma basándonos en su movimiento físico
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // Detectar si el control se sacudió con fuerza suficiente y pasó el tiempo de espera
        if (currentVelocity.magnitude > shakeThreshold && Time.time >= nextReloadTime)
        {
            PerformReload();
        }
    }

    private void PerformReload()
    {
        if (gunShoot != null)
        {
            // Solo recargamos si no está ya con el cargador lleno
            if (gunShoot.GetCurrentAmmo() < gunShoot.maxAmmo)
            {
                gunShoot.Reload();
                nextReloadTime = Time.time + reloadCooldown;
            }
        }
    }
}