using System.Collections;
using TMPro; 
using UnityEngine;

public class WaveUIManager : MonoBehaviour
{
    [Header("Round Reference")]
    [Tooltip("Round Text")]
    [SerializeField] private TextMeshProUGUI waveText;
    [Tooltip("Wave Spawner Class Reference")]
    [SerializeField] private EnemyWaveSpawner waveSpawner;

    [Header("HUD VR Settings")]
    [Tooltip("Distancia frente a la cámara a la que flotará el texto.")]
    [SerializeField] private float distanceFromCamera = 1.5f;
    [Tooltip("Height Offset")]
    [SerializeField] private float heightOffset = -0.2f;
    [Tooltip("Follow up text.")]
    [SerializeField] private float followSpeed = 5f;

    private Transform playerCameraTransform;
    private Canvas canvasComponent;

    private void Awake()
    {
        canvasComponent = GetComponent<Canvas>();
    }

    private void Start()
    {
        
        if (canvasComponent != null)
        {
            canvasComponent.renderMode = RenderMode.WorldSpace;
        }

        // Buscamos la cámara principal (VR Headset)
        if (Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No hay GameCamera en el VR");
        }

        // Nos suscribimos al evento de los Spawner
        if (waveSpawner != null)
        {
            waveSpawner.OnWaveCompleted += ShowWaveCompletedUI;
        }

        // Oculta el texto al iniciar 
        if (waveText != null)
        {
            waveText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar fugas de memoria
        if (waveSpawner != null)
        {
            waveSpawner.OnWaveCompleted -= ShowWaveCompletedUI;
        }
    }

    private void LateUpdate()
    {
        if (playerCameraTransform == null || waveText == null || !waveText.gameObject.activeSelf) return;

        // HUD flotante
        Vector3 targetPosition = playerCameraTransform.position +
                                 (playerCameraTransform.forward * distanceFromCamera) +
                                 (Vector3.up * heightOffset);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // El texto rote a la camara
        transform.rotation = Quaternion.LookRotation(transform.position - playerCameraTransform.position);
    }

    private void ShowWaveCompletedUI(int completedWaveNumber)
    {
        // La ronda que acaba de terminar fue 'completedWaveNumber', 
        // por lo que la siguiente se muestra en pantalla.
        StartCoroutine(DisplayWaveRoutine(completedWaveNumber));
    }

    private IEnumerator DisplayWaveRoutine(int waveNumber)
    {
        if (waveText != null)
        {
            waveText.text = $"ROUND {waveNumber} COMPLETE";
            waveText.gameObject.SetActive(true);

            // El texto se queda en pantalla durante un tiempo maximo
            yield return new WaitForSeconds(3f);

            waveText.gameObject.SetActive(false);
        }
    }
}