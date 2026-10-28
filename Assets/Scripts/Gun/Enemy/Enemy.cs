using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IScoreable
{
    [Header("Estadísticas")]
    [SerializeField] private float health = 3f;
    [SerializeField] private int basePoints = 100;
    public int attackDamage = 10;

    private bool isDead = false;
    private EnemyManager enemyManager;


    public int BasePoints => basePoints;

    private void Start()
    {

        enemyManager = FindFirstObjectByType<EnemyManager>();
    }


    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }


    public void AwardScore()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.RegisterKill(this);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;


        AwardScore();


        Destroy(gameObject);
    }
}

 
  /*  private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(attackDamage);
        }
    }
}*/
