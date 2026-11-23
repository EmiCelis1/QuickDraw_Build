using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float visionRange = 15f;

    [Header("Animación de Disparo (Opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string shootTriggerName = "Shoot";

    private float nextFireTime;

    protected override void InitializeEnemy()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    protected override void ExecuteBehavior()
    {
        StopMovement();

        if (Time.time >= nextFireTime)
        {
            if (CanSeeTarget(playerCameraTransform, firePoint != null ? firePoint : transform, visionRange))
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        if (animator != null && !string.IsNullOrEmpty(shootTriggerName))
        {
            animator.SetTrigger(shootTriggerName);
        }

        if (projectilePrefab != null && firePoint != null && playerCameraTransform != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector3 targetDir = (playerCameraTransform.position - firePoint.position).normalized;

            EnemyProjectile projectileScript = proj.GetComponent<EnemyProjectile>();
            if (projectileScript != null)
            {
                int damage = enemyScript != null ? enemyScript.attackDamage : 10;
                projectileScript.Setup(targetDir, damage);
            }
        }
    }
}