using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged, Centipede }

    [Header("Enemy Variant")]
    public EnemyType typeOfEnemy = EnemyType.Melee;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float nextFireTime;

    [Header("Centipede Settings")]
    public Transform[] waypoints;
    public float waypointThreshold = 0.5f;
    private int currentWaypointIndex = 0;

    [Header("Línea de Visión")]
    public LayerMask visionObstacleLayers;

    private EnemyAwareness enemyAwareness;
    private Transform playerCameraTransform; // Referencia a la cabeza del jugador en VR
    private NavMeshAgent enemyNavMeshAgent;
    private Enemy enemyScript;

    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<Enemy>();

        // En VR, el "jugador" es la cámara principal (Head del XR Origin)
        if (Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No se encontró ninguna MainCamera en la escena. Asegúrate de que la cámara de VR tenga el tag MainCamera.");
        }

        if (visionObstacleLayers == 0)
        {
            visionObstacleLayers = LayerMask.GetMask("Default", "MapTerrain");
        }
    }

    private void Update()
    {
        if (playerCameraTransform == null) return;

        if (typeOfEnemy == EnemyType.Centipede)
        {
            ControlCentipedeMovement();
            return;
        }

        if (!enemyAwareness.isAggro)
        {
            if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
            {
                enemyNavMeshAgent.SetDestination(transform.position);
            }
            return;
        }

        switch (typeOfEnemy)
        {
            case EnemyType.Melee:
                ControlMeleeMovement();
                break;

            case EnemyType.Ranged:
                ControlRangedAttack();
                break;
        }
    }

    private void ControlMeleeMovement()
    {
        if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
        {
            // Persigue la posición del visor de VR en el plano horizontal del suelo
            Vector3 targetPosition = playerCameraTransform.position;
            enemyNavMeshAgent.SetDestination(targetPosition);
        }
    }

    private void ControlRangedAttack()
    {
        if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
        {
            enemyNavMeshAgent.SetDestination(transform.position);
        }

        if (Time.time >= nextFireTime)
        {
            if (CanSeePlayer())
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void ControlCentipedeMovement()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
        {
            enemyNavMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

            if (distanceToWaypoint <= waypointThreshold)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (playerCameraTransform == null || firePoint == null) return false;

        Vector3 targetDir = (playerCameraTransform.position - firePoint.position).normalized;
        float distanceToPlayer = Vector3.Distance(firePoint.position, playerCameraTransform.position);

        if (Physics.Raycast(firePoint.position, targetDir, out RaycastHit hit, distanceToPlayer, visionObstacleLayers))
        {
            return false;
        }
        return true;
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