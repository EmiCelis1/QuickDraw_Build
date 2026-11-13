using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [Header("Configuración de Objetivo")]
    [Tooltip("La cámara del visor VR. Si se deja vacío, busca Camera.main automáticamente.")]
    public Transform playerCamera;

    [Header("Ajuste de Quad")]
    [Tooltip("Los Quads de Unity suelen necesitar invertirse 180° para mostrar la cara frontal.")]
    public bool invertDirectionForQuad = true;

    void Start()
    {
        // Asigna automáticamente la cámara de RV
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    void LateUpdate() // Usamos LateUpdate para que rote justo después de que el NavMeshAgent mueva al enemigo
    {
        if (playerCamera == null) return;

        // --- ROTACIÓN TIPO BILLBOARD SOLO EN EJE Y ---
        // Obtenemos el vector de dirección hacia la cámara
        Vector3 lookDirection = playerCamera.position - transform.position;

        // Invertimos la dirección si el Quad queda de espaldas (invisible)
        if (invertDirectionForQuad)
        {
            lookDirection = -lookDirection;
        }

        // Bloqueamos la inclinación vertical (Eje Y = 0 para que no se incline hacia arriba o abajo)
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}