using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public float speed = 12f;
    public float lifetime = 4f;

    private Vector3 moveDirection;
    private int damageToDeal;

    public void Setup(Vector3 direction, int damage)
    {
        moveDirection = direction;
        damageToDeal = damage;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos el PlayerHealth en el objeto o en sus padres (por si choca con un hijo del XR Origin)
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("El proyectil impactó al jugador aplicando daño: " + damageToDeal);
            player.TakeDamage(damageToDeal);
            Destroy(gameObject);
            return;
        }

        // Si choca con paredes, suelo u otros objetos que no sean triggers, se destruye
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}