using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IScoreable
{
    [Header("Estadísticas")]
    [SerializeField] private float health = 3f;
    [SerializeField] private int basePoints = 100;
    public int attackDamage = 10;

    [Header("Animación (Opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string dieTriggerName = "Die";
    [SerializeField] private float fallbackDestroyDelay = 1.5f;

    [Header("Efecto Sombrero al Morir (Opcional)")]
    [SerializeField] private FlyingHat hatPrefab;
    [SerializeField] private Transform hatSpawnPoint;

    private bool isDead = false;
    private bool deathCleanupStarted = false;
    private EnemyManager enemyManager;
    private EnemyAI enemyAI;
    private NavMeshAgent navMeshAgent;

    public int BasePoints => basePoints;

    private void Start()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        enemyAI = GetComponent<EnemyAI>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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

        // 1. Puntos y registro de baja inmediatos
        AwardScore();

        if (enemyManager != null)
        {
            enemyManager.RemoveEnemy(this);
        }

        // 2. Bloquear IA, movimiento y colisiones durante la animación de derrota
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // 3. Lanzar sombrero al morir (si está asignado)
        SpawnFlyingHat();

        // 4. Reproducir animación de derrota si existe Animator
        if (animator != null && !string.IsNullOrEmpty(dieTriggerName))
        {
            animator.ResetTrigger(dieTriggerName);
            animator.SetTrigger(dieTriggerName);
            StartCoroutine(FallbackDestroyRoutine());
        }
        else
        {
            FinalizeDeathAndDestroy();
        }
    }

    private void SpawnFlyingHat()
    {
        if (hatPrefab == null) return;

        Vector3 spawnPos = hatSpawnPoint != null
            ? hatSpawnPoint.position
            : transform.position + Vector3.up * 1.4f;

        FlyingHat hatInstance = Instantiate(hatPrefab, spawnPos, transform.rotation);
        hatInstance.Launch(transform.right);
    }

    // Puedes llamar a este método desde un Animation Event al final del clip de muerte
    public void OnDeathAnimationFinished()
    {
        FinalizeDeathAndDestroy();
    }

    private IEnumerator FallbackDestroyRoutine()
    {
        yield return new WaitForSeconds(fallbackDestroyDelay);
        FinalizeDeathAndDestroy();
    }

    private void FinalizeDeathAndDestroy()
    {
        if (deathCleanupStarted) return;
        deathCleanupStarted = true;

        // Hook reservado para tu efecto especial futuro al ser derrotado
        OnSpecialDeathEffect();

        Destroy(gameObject);
    }

    protected virtual void OnSpecialDeathEffect()
    {
        // Aquí agregaremos más adelante tu mecánica/efecto especial de derrota
    }
}