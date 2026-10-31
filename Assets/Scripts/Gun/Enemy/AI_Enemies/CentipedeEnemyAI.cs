using UnityEngine;

public class CentipedeEnemyAI : EnemyAI
{
    [Header("Centipede Settings")]
    public Transform[] waypoints;
    public float waypointThreshold = 0.5f;
    private int currentWaypointIndex = 0;

    protected override bool IgnoresAggroByDefault() => true; // Se mueve aunque no esté en aggro

    protected override void ExecuteBehavior()
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
}