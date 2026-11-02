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

        // 1. Otorgamos los puntos al sistema de score (Arcade)
        AwardScore();

        // 2. Avisamos al EnemyManager que este enemigo murió para que lo remueva de la lista
        if (enemyManager != null)
        {
            enemyManager.RemoveEnemy(this);
        }

        // 3. Destruimos el objeto de la escena
        Destroy(gameObject);
    }
}