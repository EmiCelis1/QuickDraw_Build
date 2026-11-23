using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAwareness))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyAI : MonoBehaviour
{
    [Header("Línea de Visión (Común)")]
    public LayerMask visionObstacleLayers;

    protected EnemyAwareness enemyAwareness;
    protected Transform playerCameraTransform;
    protected NavMeshAgent enemyNavMeshAgent;
    protected Enemy enemyScript;

    protected virtual void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<Enemy>();

        if (Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No se encontró ninguna MainCamera en la escena.");
        }

        if (visionObstacleLayers == 0)
        {
            visionObstacleLayers = LayerMask.GetMask("Default", "MapTerrain");
        }

        InitializeEnemy();
    }

    protected virtual void InitializeEnemy() { }

    protected virtual void Update()
    {
        if (playerCameraTransform == null) return;

        // Si no está en modo aggro y no es un enemigo que se mueva por rutas (ej. centinela/centipede), se detiene
        if (!enemyAwareness.isAggro && !IgnoresAggroByDefault())
        {
            StopMovement();
            return;
        }

        ExecuteBehavior();
    }

    // Método abstracto que cada enemigo hijo implementará obligatoriamente
    protected abstract void ExecuteBehavior();

    protected virtual bool IgnoresAggroByDefault() => false;

    protected void StopMovement()
    {
        if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
        {
            enemyNavMeshAgent.SetDestination(transform.position);
        }
    }

    protected bool CanSeeTarget(Transform target, Transform origin, float maxDistance)
    {
        if (target == null || origin == null) return false;

        Vector3 targetDir = (target.position - origin.position).normalized;
        float distance = Vector3.Distance(origin.position, target.position);

        if (distance > maxDistance) return false;

        if (Physics.Raycast(origin.position, targetDir, out RaycastHit hit, distance, visionObstacleLayers))
        {
            return false;
        }
        return true;
    }
}