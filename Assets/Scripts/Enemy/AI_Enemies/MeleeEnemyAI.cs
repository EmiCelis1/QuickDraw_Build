using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    protected override void ExecuteBehavior()
    {
        if (enemyNavMeshAgent != null && enemyNavMeshAgent.isOnNavMesh)
        {
            Vector3 targetPosition = playerCameraTransform.position;
            enemyNavMeshAgent.SetDestination(targetPosition);
        }
    }
}