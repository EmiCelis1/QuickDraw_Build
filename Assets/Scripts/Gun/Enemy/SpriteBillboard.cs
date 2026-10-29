using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [Header("Configuración de Objetivo")]
    [Tooltip("La cámara del visor VR. Si se deja vacío, busca Camera.main automáticamente.")]
    public Transform playerCamera;

    [Header("Movimiento")]
    public float moveSpeed = 2.0f;

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

    void Update()
    {
        if (playerCamera == null) return;

        // --- 1. MOVIMIENTO HORIZONTAL (Eje Y fijo) ---
        Vector3 targetPosition = new Vector3(playerCamera.position.x, transform.position.y, playerCamera.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // --- 2. ROTACIÓN TIPO BILLBOARD SOLO EN EJE Y ---
        // Obtenemos el vector de dirección hacia la cámara
        Vector3 lookDirection = playerCamera.position - transform.position;

        // Invertimos la dirección si el Quad queda de espaldas (invisible)
        if (invertDirectionForQuad)
        {
            lookDirection = -lookDirection;
        }

        // Bloqueamos la inclinación vertical (Eje Y = 0)
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}