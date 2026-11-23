using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 8f;
    public bool isAggro;
    private Transform playerCameraTransform;

    private void Start()
    {
        if (Camera.main != null)
        {
            playerCameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (playerCameraTransform == null) return;

        var dist = Vector3.Distance(transform.position, playerCameraTransform.position);

        if (dist < awarenessRadius)
        {
            isAggro = true;
        }
        else
        {
            isAggro = false;
        }
    }
}