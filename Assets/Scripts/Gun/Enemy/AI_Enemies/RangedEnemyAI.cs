using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float visionRange = 15f;
    private float nextFireTime;

    protected override void ExecuteBehavior()
    {
        StopMovement(); // Opcional: se detiene al disparar, o puedes quitarlo si dispares en movimiento

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