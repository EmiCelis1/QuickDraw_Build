using UnityEngine;

public class FlyingHat : MonoBehaviour
{
    [Header("Impulso y física simple")]
    [SerializeField] private Vector3 launchVelocity = new Vector3(1.5f, 4.5f, 1.0f);
    [SerializeField] private float gravity = 12f;
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private float lifeTime = 2.5f;

    private Vector3 currentVelocity;
    private Vector3 randomSpinAxis;

    public void Launch(Vector3 directionHint)
    {
        // Mezcla un impulso hacia arriba y hacia los lados con una ligera variación
        float sideSign = Random.value < 0.5f ? -1f : 1f;
        Vector3 horizontalDir = directionHint.sqrMagnitude > 0.01f
            ? directionHint.normalized
            : transform.right * sideSign;

        currentVelocity = new Vector3(
            horizontalDir.x * Mathf.Abs(launchVelocity.x) * sideSign + Random.Range(-0.6f, 0.6f),
            launchVelocity.y + Random.Range(-0.5f, 0.8f),
            horizontalDir.z * Mathf.Abs(launchVelocity.z) + Random.Range(-0.6f, 0.6f)
        );

        randomSpinAxis = new Vector3(
            Random.Range(-0.4f, 0.4f),
            Random.Range(-0.4f, 0.4f),
            1f
        ).normalized;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        currentVelocity.y -= gravity * Time.deltaTime;
        transform.position += currentVelocity * Time.deltaTime;
        transform.Rotate(randomSpinAxis, spinSpeed * Time.deltaTime, Space.Self);
    }
}