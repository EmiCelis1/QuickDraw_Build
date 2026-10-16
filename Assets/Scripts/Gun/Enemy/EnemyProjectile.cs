using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
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

    
    /*private void OnTriggerEnter(Collider other)
    {
        
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.DamagePlayer(damageToDeal);
            Destroy(gameObject);
            return; 
        }

        
        if (!other.isTrigger)
        {
            
            Destroy(gameObject); 
        }
    } */
}